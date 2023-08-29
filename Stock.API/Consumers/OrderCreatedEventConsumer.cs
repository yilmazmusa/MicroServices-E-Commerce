using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Stock.API.Models.Entities.Stock> _stockCollection;

        readonly ISendEndpointProvider _sendEndpointProvider; // Stock-Payment Servisleri arasında iletişimde Send/kuyruk metodunu kullanacağımız için ekledik.



        public OrderCreatedEventConsumer(MongoDBService mongoDBService, ISendEndpointProvider sendEndpointProvider)
        {
            _stockCollection = mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                stockResult.Add((await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any()); // Şimdi burda siparişin alınabilmesi için gönderilen ProductId ye karşılık VT nında bir ProductId olması ve  VT  da siparişte istenen Counttan  daha fazla Count ta ürün olması lazım.Eğer bu iki case sağlanırsa stockResult a true basıcak.Bizde stockResult un içine bakarak hangi sipariş başarılı hangisi başarısız anlayacağız.
            }

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Stock.API.Models.Entities.Stock stock = await (await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync(); //Aslında burda eşleşen ürünü bulduk.Sonrasında bu ürünün Count tudan siparişte istenen Count u çıkarıcaz ki Stok güncellensin.

                    stock.Count -= orderItem.Count;

                    await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock); // Güncel Count bilgisine göre VT ını güncelledik.

                    //Siparişi aldık bir sorun yok o ürün var ve stoğuda var sıkıntı yok artık, stoktan düşüldü o ürün Payment e geçebiliriz.

                    StockReservedEvent stockReservedEvent = new() //Eventi oluşturduk.Şimdi bunu Payment in dinlediği kuyruğa göndericez.
                    {
                        BuyerId = context.Message.BuyerId,
                        OrderId = context.Message.OrderId,
                        TotalPrice = context.Message.TotalPrice,
                    };

                   ISendEndpoint sendEndpoint= await  _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue} ")); //Hedef kuyruğu belirledik.Bu kuyruğu dinleyen Services/Consumer ların olaydan haberi olacak sadace.

                    sendEndpoint.Send(stockReservedEvent); //Yayınlama işlemini tamamladık.

                }
            }
            else
            {
                //Sipariş hatalı/ Hatalı Ürün girdiniz./Stok Yok
            }

        }
    }
}

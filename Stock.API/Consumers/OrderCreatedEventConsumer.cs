using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Stock.API.Models.Entities.Stock> _stockCollection;


        public OrderCreatedEventConsumer(MongoDBService mongoDBService)
        {
           _stockCollection = mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>();
        }

        public  async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

            foreach(OrderItemMessage orderItem in context.Message.OrderItems)
            {
                stockResult.Add(( await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any()); // Şimdi burda siparişin alınabilmesi için gönderilen ProductId ye karşılık VT nında bir ProductId olması ve  VT  da siparişte istenen Counttan  daha fazla Count ta ürün olması lazım.Eğer bu iki case sağlanırsa stockResult a true basıcak.Bizde stockResult un içine bakarak hangi sipariş başarılı hangisi başarısız anlayacağız.
            }

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                // Sipariş için bir engel yok.Devam edilebilir.Gerekli işlemler yapılsın
            }
            else
            {
                //Sipariş hatalı/ Hatalı Ürün girdiniz./Stok Yok
            }

        }
    }
}

using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public  Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //StockReserved işlemlerinde bir sorun yok ya yani o ürünün stoğu istenenden fazlaysa sorn yok demektir.Artık ödeme işlemlerine geçilebilir.
            //Ödeme işlemleri

            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    //Burda ödeme için TotalPrice ve BuyerId gerekiyor aslında mış gibi yaptık burda.
                };
                _publishEndpoint.Publish(paymentCompletedEvent); // Publish ettik artık.

                Console.WriteLine("Ödeme Başarılı");
            }
            else
            {
                //Ödeme sırasında hata varsa
                //Aslında buraya hiç düşmeyecek çünkü yukardaki if in içi true :) mış gibi yaptık.
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Ödeme sırasında bir hata oluştu.Bakiye yetersiz."
                };
                Console.WriteLine("Ödeme Başarısız");


            }
            return Task.CompletedTask;
        }

       
    }
}

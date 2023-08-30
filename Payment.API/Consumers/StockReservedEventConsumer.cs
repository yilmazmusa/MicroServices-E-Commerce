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
            }
            else
            {
                //Ödeme sırasında hata
            }
            return Task.CompletedTask;
        }

       
    }
}

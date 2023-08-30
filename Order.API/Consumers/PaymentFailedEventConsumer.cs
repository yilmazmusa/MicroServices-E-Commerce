using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDBContext _orderAPIDBContext;

        public PaymentFailedEventConsumer(OrderAPIDBContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
           Order.API.Models.Entities.Order? order =  await  _orderAPIDBContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatu = Models.Enums.OrderStatus.Failed;
            await _orderAPIDBContext.SaveChangesAsync(); // Değişikliği VT yansıtıyoruz.

        }
    }
}

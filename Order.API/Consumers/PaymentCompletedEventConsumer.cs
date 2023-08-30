using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDBContext _orderAPIDBContext;

        public PaymentCompletedEventConsumer(OrderAPIDBContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
          Order.API.Models.Entities.Order? order =   await _orderAPIDBContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);

            order.OrderStatu = Models.Enums.OrderStatus.Completed;//İlgili Order ın statusunu Completed'a çektk.
            await _orderAPIDBContext.SaveChangesAsync();
        }
    }
}

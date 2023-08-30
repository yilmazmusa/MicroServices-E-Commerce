using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {

        readonly OrderAPIDBContext _orderAPIDBContext;

        public StockNotReservedEventConsumer(OrderAPIDBContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Order.API.Models.Entities.Order? order = await _orderAPIDBContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);

            order.OrderStatu = Models.Enums.OrderStatus.Failed; // Siparişin duurmunu Failed e çektik.
            await _orderAPIDBContext.SaveChangesAsync();
        }
    }
}

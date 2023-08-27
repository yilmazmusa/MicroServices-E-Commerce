namespace Order.API.Models.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid  BuyerId { get; set; }
        public long TotalPrice { get; set; }
        public string Description { get; set; }

        }
    }
}

namespace Order.API.Models.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid ProductId{ get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }  
    }
}

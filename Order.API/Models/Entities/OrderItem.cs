namespace Order.API.Models.Entities
{
    public class OrderItem // Sipariş-Ürün İlişkisi
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public String ProductId{ get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }  
        public Order Order { get; set; }
    }
}

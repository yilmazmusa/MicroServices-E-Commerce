using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entities;

namespace Order.API.Models
{
    public class OrderAPIDBContext : DbContext
    {
        public OrderAPIDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order.API.Models.Entities.Order> Orders { get; set; } // Normalde sadece Order yazardık ama namespace de de Order oludğu için karıştı o yüzden bizde burda bu şekilde uzun verdik.
        public DbSet<OrderItem> OrderItems { get; set; }
    }


}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.ViewModels;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDBContext _context;

        public OrdersController(OrderAPIDBContext context)
        {
            _context = context;
        } 

        [HttpPost]
        public async Task<IActionResult> CreateOrder( CreateOrderVM createOrder) //Kullanıcıdan gelen bilgilere göre bir Order oluşturan Servis.Kullanıcıdan gelen bilgileri ViemModel la karşılıycaz.Daha sonra o ViewModel dan gelen verileri Entitylere dönüştürücem. O Entity leride Veritabanına ekliycez.
        {
            Order.API.Models.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = createOrder.BuyerId,
                CreateDate = DateTime.Now,
                OrderStatu = Models.Enums.OrderStatus.Suspand
            };


            order.OrderItems = createOrder.OrderItems.Select(oi => new Models.Entities.OrderItem // Burda View Model eşliğinde kullanıcından gelen orderItem verilerini OrderItem Entity sininin türüne dönüştürüyorum.Sonrasında da zaten veritabanı işlemlerini yapıcaz.
            {
                Count= oi.Count, //Kullanıcıdan  VM ile gelen OrderItems bilgilerini OrderItems lara verdik.
                Price = oi.Price,
                ProductId= oi.ProductId,
                
            }).ToList();

            order.TotalPrice = createOrder.OrderItems.Sum(oi => (oi.Price * oi.Count));
            
            await _context.Orders.AddAsync(order); 
            await  _context.SaveChangesAsync();

            return Ok();
        }
    }
}

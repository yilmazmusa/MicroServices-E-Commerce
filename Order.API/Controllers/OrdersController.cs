using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.ViewModels;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDBContext _context;
        readonly IPublishEndpoint _publishEndpoint; // Bu MassTransit üzerinden bir Eventi Publish edebilmemizi sağlayan instance'ı IOC Container den bize getirecek.

        public OrdersController(OrderAPIDBContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder( CreateOrderVM createOrder) //Kullanıcıdan gelen bilgilere göre bir Order(Sipariş) oluşturan Servis.Kullanıcıdan gelen bilgileri ViemModel la karşılıycaz.Daha sonra o ViewModel dan gelen verileri Entitylere dönüştürücem. O Entity leride Veritabanına ekliycez.
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

            OrderCreatedEvent orderCreatedEvent = new() // Burda Shared projesi altındaki OrderCreatedEvent den bir instance üretmek istiyorsan Order.API de Add Project Reference deyip Shared i eklemek gerekiyor.
            {
                BuyerId = order.BuyerId,//burda order danda verebiliriz VM den gelen createOrder da da veririz
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
                {
                    Count = oi.Count,
                    ProductId = oi.ProductId,
                }).ToList()
            };

            // Bir siparişin oluşturulduğunu  gösteren OrderCreatedEvet nesnesini oluşturduk artık bu nesnesi publish edicez sadece.Onu da aşağıda yapıcaz.

            await _publishEndpoint.Publish(orderCreatedEvent);

                    
            return Ok();
        }
    }
}

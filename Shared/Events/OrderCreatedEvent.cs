using Shared.Events.Common;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderCreatedEvent : IEvent // Order(Sipariş) oluşurken gerekli olan propertyler. Biz Eventimizi oluşturduk sipariş oluştuğunda biz bu Eventi fırlatıcaz.
        //Yani diyoruz ki şu BuyerId li kullanıcının yapmış olduğu şu  OrderId li siparişin içerisinde  OrderItemMessage ın içindeki şu Üründen(ProductId) şu kadar tane(Count)  ürün siparişe eklendi bunları stoktan düş diyeceğiz Event ile.
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}

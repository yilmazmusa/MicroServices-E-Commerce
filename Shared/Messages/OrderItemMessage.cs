using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class OrderItemMessage //Order oluştururken asenkron bir şekilde Stok servisine işi teslim ederken bize ürünün stoğunu güncelleyebilmemiz için Order(Sipariş) te hangi Üründen(ProductId) şu kadar tane(Count) alındı bunları stoktan düş diyeceğiz.
    {
        public String ProductId { get; set; }
        public int Count { get; set; }
    }
}

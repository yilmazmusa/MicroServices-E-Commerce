namespace Order.API.ViewModels
{
    public class CreateOrderVM
    {
        //Bu Entity i güzel modellememiz lazım mesela bunun içerisinde OrderId olmayacak çünkü burası müşteriden gelen bilgiler müşteri Order oluştururken onun oluşacak Id sini bilmez bilmeside gerekmez.OrderId si müşteri istekte bulunup sipariş oluşurken kendi oluşacak. O yüzden OrderId burda yok.

        public Guid BuyerId { get; set; }

        public List<CreateOrderItemVM> OrderItems { get; set; }

        //Yani müşteri diyorki ben şu BuyerId li müşteriyim şu ProductId li üründen şu kadar(Count) şu fiyattan(Price) istiyorum diyor.Burda aslında Price da olmaması gerekiyor bizim Product servisimiz olmadığı için ordan ürünün fiyat bilgisini çekemediği için burda bu şekilde alıyoruz.

    }


}

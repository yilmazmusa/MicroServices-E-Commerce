namespace Order.API.ViewModels
{
    public class CreateOrderItemVM
    {
        public String ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        //Yani müşteri diyorki ben şu BuyerId li müşteriyim şu ProductId li üründen şu kadar(Count) şu fiyattan(Price) istiyorum diyor.Burda aslında Price da olmaması gerekiyor bizim Product servisimiz olmadığı için ordan ürünün fiyat bilgisini çekemediği için burda bu şekilde alıyoruz.
    }
}

using MongoDB.Driver;

namespace Stock.API.Services  
{
    public class MongoDBService // Bu MongoDBService' in IOC ile irtibatlı olması için bunu gidip Program.cs de builder.Services.AddSingleton<MongoDBService>(); şeklinde IOC Container a    ekliyoruz.
    {
        readonly IMongoDatabase _mongoDatabase; // IMongoDatabase interfacesinden bir referans aldık.

        public MongoDBService(IConfiguration configuration)
        {
                MongoClient   client = new(configuration.GetConnectionString("MongoDB"));//MongoClient sınıfı bizim MongoDB nin sunucusuna bağlanmamızı sağlıyo zaten MongoDB  sunucusunun ConnectionStringini alıyor. client sunucuya bağlanıyor ve bize veritabanını getiriyor.
            _mongoDatabase = client.GetDatabase("StockDB"); //StockDB adındaki Veritabanımızı _mongoDatabase referansına verdik.
        }



        //MongoDB de bir DATABASE(StockAPIDB) den  veri okumak için  IMongoCollection türünden bir func oluşturduk.GetCollection fonksiyonu bize  StockAPIDB(_mongoDatabase) den GetCollection fonksiyonu ile T ye karşılık gelen collectionu getirecek.Yani hangi Entity veriliyorsa o entity e karşılık gelen collection getirilecek.
        public IMongoCollection<T> GetCollection<T>() => _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}

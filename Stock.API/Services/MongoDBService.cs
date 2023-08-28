using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDBService // Bu MongoDBService' in IOC ile irtibatlı olması için bunu gidip Program.cs de builder.Services.AddSingleton<MongoDBService>(); şeklinde IOC Container a    ekliyoruz.
    {
        readonly IMongoDatabase _mongoDatabase; // IMongoDatabase interfacesinden bir referans aldık.

        public MongoDBService(IConfiguration configuration)
        {
                MongoClient   client = new(configuration.GetConnectionString("MongoDB"));
                _mongoDatabase = client.GetDatabase("StockAPIDB");
        }
    }
}

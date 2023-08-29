using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>(); //Consumer'ý tanýmladýk.

    configurator.UsingRabbitMq((context, _configurator) =>
    {

        _configurator.Host(builder.Configuration["RabbitMQ"]);

        _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));  // RabbitMQ içerisindeki hangi kuyruktan bu dinleme iþlemini yapacaðýný belirtiyoruz.Stock_OrderCreatedEventQueue isimli kuyruktan.kuyruk adresinini zaten 21.satýrda belittik.KUyruk adýnýda  RabbitMQSettings da ki  Stock_OrderCreatedEventQueue da elirtmiþtik ordan çektik.
    });
});

builder.Services.AddSingleton<MongoDBService>(); // MongoDBService' nin IOC ile irtibatlý olmasý için bunu gelip bu þekilde IOC Container a ekliyoruz.


#region Harici Bir yer burasý -MongoDB ye Seed Data Eklemek Ýçin yaptýk.Bunu Stock.API ye bir arayüz oluþturup arayüz üzerinden veritabanýna verileri ekleyebilirdik.

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope(); // Burada IServiceScope IDisposable türünden olduðu için yani tek kullanýmlýk baþýna using koyduk. Bir scope oluþturduk.

MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>(); // Scope a   MongoDBService ini ekledik.
var collection = mongoDBService.GetCollection<Stock.API.Models.Entities.Stock>(); // Stock Entitysini collection a ekledik.

if (!collection.FindSync(s => true).Any())
{
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 2000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 1000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 3000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 5000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 500 });


}

#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

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
    configurator.AddConsumer<OrderCreatedEventConsumer>(); //Consumer'� tan�mlad�k.

    configurator.UsingRabbitMq((context, _configurator) =>
    {

        _configurator.Host(builder.Configuration["RabbitMQ"]);

        _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));  // RabbitMQ i�erisindeki hangi kuyruktan bu dinleme i�lemini yapaca��n� belirtiyoruz.Stock_OrderCreatedEventQueue isimli kuyruktan.kuyruk adresinini zaten 21.sat�rda belittik.KUyruk ad�n�da  RabbitMQSettings da ki  Stock_OrderCreatedEventQueue da elirtmi�tik ordan �ektik.
    });
});

builder.Services.AddSingleton<MongoDBService>(); // MongoDBService' nin IOC ile irtibatl� olmas� i�in bunu gelip bu �ekilde IOC Container a ekliyoruz.


#region Harici Bir yer buras� -MongoDB ye Seed Data Eklemek ��in yapt�k.Bunu Stock.API ye bir aray�z olu�turup aray�z �zerinden veritaban�na verileri ekleyebilirdik.

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope(); // Burada IServiceScope IDisposable t�r�nden oldu�u i�in yani tek kullan�ml�k ba��na using koyduk. Bir scope olu�turduk.

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

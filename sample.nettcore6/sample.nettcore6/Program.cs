using sample.nettcore6;
using sample.nettcore6.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICake, ChocolateCake>();
builder.Services.AddSingleton<ICakeMessageDecorator, CakeMessageDecorator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/decorate", (
    ICakeMessageDecorator cakeMessageDecorator,
    ICake cake) =>
{
    cakeMessageDecorator.Decorate("Happy Birthday!");
    cake.PrintLayers();
})
.WithName("Decorate");

app.Run();

/*
 * http://localhost:26672/decorate
 Console.WriteLine
 Chocolate Layer: Message for the cake: Happy Birthday!
 ---------- 
Chocolate Layer: Message for the cake: Happy Birthday!
 ---------- 
 */
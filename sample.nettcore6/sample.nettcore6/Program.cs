using sample.nettcore6;
using sample.nettcore6.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ClientIpCheckActionFilter>(container =>
{
    var loggerFactory = container.GetRequiredService<ILoggerFactory>();
    var configuration= container.GetRequiredService<IConfiguration>();
    var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();
    var str = configuration["AdminSafeList"];
    return new ClientIpCheckActionFilter(
        str, logger);
});
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
app.UseRouting();   

app.MapGet("/decorate", (
    ICakeMessageDecorator cakeMessageDecorator,
    ICake cake) =>
{
    cakeMessageDecorator.Decorate("Happy Birthday!");
    cake.PrintLayers();
})
.WithName("Decorate");
app.MapGet("/", () =>
{
    var barry = GenerateAndSelectABarry();
    GC.Collect();  //And launchSettings.json set "DOTNET_gcServer": "0"
    return $"{barry}{Environment.NewLine}{GC.GetTotalMemory(false)/1024/1024}Mb managed memory{Environment.NewLine}{System.Environment.WorkingSet/1024/1024}Mb total used";
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();

/*
 * http://localhost:26672/decorate
 Console.WriteLine
 Chocolate Layer: Message for the cake: Happy Birthday!
 ---------- 
Chocolate Layer: Message for the cake: Happy Birthday!
 ---------- 
 */

/*
 * no gc.collect and DOTNET_gcServer 0
 Barryb2a6cba5-854f-46dd-a68f-f8aaa8805456
1520Mb managed memory
2024Mb total used
 */

/*
    GC.Collect();  //And launchSettings.json set "DOTNET_gcServer": "0"

Barry3e1d7aa5-db43-4dd4-b55b-aaeab001695e
1Mb managed memory
267Mb total used
 */

/*
[ServiceFilter(typeof(ClientIpCheckActionFilter))]
 
 */
static string GenerateAndSelectABarry()
{
    var barryGenerator = new BarryGenerator();
    var barrays = barryGenerator.GenerateBarrays();
    return barrays[new Random().Next(barrays.Count)];
}

internal class BarryGenerator
{
    public List<string> GenerateBarrays()
    {
        return Enumerable.Range(1, 10000000).Select(i=>$"Barry{Guid.NewGuid().ToString()}").ToList();
    }
}
using DefenseAPI;
using DefenseAPI.Controllers;
using System.Globalization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var cultureInfo = new CultureInfo("en-GB");
cultureInfo.NumberFormat.CurrencySymbol = "�";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
builder.Services.AddControllers();
builder.Services.AddScoped<DAL>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(option =>
{
    option.Listen(IPAddress.Loopback, 7132, listOptions =>
    {
        listOptions.UseHttps();
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200") // Replace with your Angular app's URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // if needed for cookies/auth
});

var app = builder.Build();
app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Setup database connection...
builder.Services.AddDbContext<WestcoastCarsContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database...
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<WestcoastCarsContext>();
    await context.Database.MigrateAsync();

    await SeedData.LoadManufacturerData(context);
    await SeedData.LoadFuelTypeData(context);
    await SeedData.LoadTransmissionData(context);
    await SeedData.LoadVehicleData(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

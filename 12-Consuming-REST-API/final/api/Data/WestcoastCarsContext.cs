using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Data;

public class WestcoastCarsContext : IdentityDbContext<UserModel>
{
    public DbSet<VehicleModel> Vehicles { get; set; }
    public DbSet<ManufacturerModel> Manufacturers { get; set; }
    public DbSet<TransmissionTypeModel> TransmissionTypes { get; set; }
    public DbSet<FuelTypeModel> FuelTypes => Set<FuelTypeModel>();

    public WestcoastCarsContext(DbContextOptions options) : base(options)
    {
    }
}

using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Data;

public class WestcoastCarsContext : DbContext
{
    public DbSet<VehicleModel> Vehicles => Set<VehicleModel>();
    public DbSet<ManufacturerModel> Manufacturers => Set<ManufacturerModel>();
    public DbSet<TransmissionTypeModel> TransmissionTypes => Set<TransmissionTypeModel>();
    public DbSet<FuelTypeModel> FuelTypes => Set<FuelTypeModel>();

    public WestcoastCarsContext(DbContextOptions options) : base(options)
    {
    }
}

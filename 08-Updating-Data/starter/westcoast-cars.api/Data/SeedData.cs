using System.Text.Json;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Data;

public static class SeedData
{
    public static async Task LoadVehicleData(WestcoastCarsContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Vehicles.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/vehicles.json");
        var vehicles = JsonSerializer.Deserialize<List<VehicleModel>>(json, options);

        if (vehicles is not null && vehicles.Count > 0)
        {
            await context.Vehicles.AddRangeAsync(vehicles);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadTransmissionData(WestcoastCarsContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.TransmissionTypes.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/transmissions.json");
        var transmissionTypes = JsonSerializer.Deserialize<List<TransmissionTypeModel>>(json, options);

        if (transmissionTypes is not null && transmissionTypes.Count > 0)
        {
            await context.TransmissionTypes.AddRangeAsync(transmissionTypes);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadFuelTypeData(WestcoastCarsContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.FuelTypes.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/fueltypes.json");
        var fueltypes = JsonSerializer.Deserialize<List<FuelTypeModel>>(json, options);

        if (fueltypes is not null && fueltypes.Count > 0)
        {
            await context.FuelTypes.AddRangeAsync(fueltypes);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadManufacturerData(WestcoastCarsContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Manufacturers.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/manufacturers.json");
        var manufacturers = JsonSerializer.Deserialize<List<ManufacturerModel>>(json, options);

        if (manufacturers is not null && manufacturers.Count > 0)
        {
            await context.Manufacturers.AddRangeAsync(manufacturers);
            await context.SaveChangesAsync();
        }
    }
}

using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Data;

public static class SeedData
{
    public static async Task LoadRolesAndUsers(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var admin = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            var sales = new IdentityRole { Name = "Sales", NormalizedName = "SALES" };
            var user = new IdentityRole { Name = "User", NormalizedName = "USER" };

            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(sales);
            await roleManager.CreateAsync(user);
        }

        if (!userManager.Users.Any())
        {
            var admin = new UserModel
            {
                UserName = "michael@gmail.com",
                Email = "michael@gmail.com",
                FirstName = "Michael",
                LastName = "Gustavsson"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Sales", "User" });

            var user = new UserModel
            {
                UserName = "eva@gmail.com",
                Email = "eva@gmail.com",
                FirstName = "Eva",
                LastName = "Nilsson"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "User");

            var sales = new UserModel
            {
                UserName = "oscar@gmail.com",
                Email = "oscar@gmail.com",
                FirstName = "Oscar",
                LastName = "Olsson"
            };

            await userManager.CreateAsync(sales, "Pa$$w0rd");
            await userManager.AddToRoleAsync(sales, "Sales");
        }
    }
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

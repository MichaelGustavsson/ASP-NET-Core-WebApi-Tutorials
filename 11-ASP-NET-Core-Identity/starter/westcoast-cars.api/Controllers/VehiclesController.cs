using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;
using westcoast_cars.api.Models;
using westcoast_cars.api.ViewModels;

namespace westcoast_cars.api.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly WestcoastCarsContext _context;
    public VehiclesController(WestcoastCarsContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult> List()
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Select(v => new VehicleListViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            Manufacturer = v.Manufacturer.Name ?? "",
            Model = v.Model,
            ModelYear = v.ModelYear,
            Mileage = v.Mileage,
            FuelType = v.FuelType.Name ?? "",
            TransmissionType = v.TransmissionType.Name ?? ""
        })
        .ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Select(v => new VehicleDetailsViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            Manufacturer = v.Manufacturer.Name ?? "",
            Model = v.Model,
            ModelYear = v.ModelYear,
            Mileage = v.Mileage,
            FuelType = v.FuelType.Name ?? "",
            TransmissionType = v.TransmissionType.Name ?? "",
            Description = v.Description,
            Value = v.Value,
            ImageUrl = v.ImageUrl ?? "no-car.png"
        })
        .SingleOrDefaultAsync(v => v.Id == id);

        return Ok(result);
    }

    [HttpGet("regno/{regNo}")]
    public async Task<ActionResult> GetByRegistrationNumber(string regNo)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Select(v => new VehicleDetailsViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            Manufacturer = v.Manufacturer.Name ?? "",
            Model = v.Model,
            ModelYear = v.ModelYear,
            Mileage = v.Mileage,
            FuelType = v.FuelType.Name ?? "",
            TransmissionType = v.TransmissionType.Name ?? "",
            Description = v.Description,
            Value = v.Value,
            ImageUrl = v.ImageUrl ?? "no-car.png"
        })
        .SingleOrDefaultAsync(v => v.RegistrationNumber!.ToUpper().Trim() == regNo.ToUpper().Trim());

        return Ok(result);
    }

    [HttpGet("model/{model}")]
    public async Task<ActionResult> GetByModel(string model)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Where(c => c.Model!.ToUpper().Trim() == model.ToUpper().Trim())
        .Select(v => new VehicleDetailsViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            Manufacturer = v.Manufacturer.Name ?? "",
            Model = v.Model,
            ModelYear = v.ModelYear,
            Mileage = v.Mileage,
            FuelType = v.FuelType.Name ?? "",
            TransmissionType = v.TransmissionType.Name ?? "",
            Description = v.Description,
            Value = v.Value
        })
        .ToListAsync();

        return Ok(result);
    }

    [HttpGet("modelyear/{year}")]
    public async Task<ActionResult> GetByModelYear(string year)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Where(c => c.ModelYear!.Trim() == year.Trim())
        .Select(v => new VehicleDetailsViewModel
        {
            Id = v.Id,
            RegistrationNumber = v.RegistrationNumber,
            Manufacturer = v.Manufacturer.Name ?? "",
            Model = v.Model,
            ModelYear = v.ModelYear,
            Mileage = v.Mileage,
            FuelType = v.FuelType.Name ?? "",
            TransmissionType = v.TransmissionType.Name ?? "",
            Description = v.Description,
            Value = v.Value
        })
        .ToListAsync();

        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult> AddVehicle(VehicleAddViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lagra bilen i systemet");

        //Vi måste kontrollera så att bilen inte redan är registrerad i systemet...
        var exists = await _context.Vehicles.SingleOrDefaultAsync(c => c.RegistrationNumber!.ToUpper().Trim() == model.RegistrationNumber!.ToUpper().Trim());

        if (exists is not null) return BadRequest($"Vi har redan registrerat en bil med registreringsnummer {model.RegistrationNumber}");

        // Kontrollera att tillverkaren finn i systemet...
        var make = await _context.Manufacturers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Manufacturer.ToUpper().Trim());
        if (make is null) return NotFound($"Vi kunde inte hitta någon tillverkare med namnet {model.Manufacturer} i vårt system");

        // Kontrollera så att växellådetypen finns i systemet...
        var gearType = await _context.TransmissionTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.TransmissionType.ToUpper().Trim());
        if (gearType is null) return NotFound($"Vi kunde inte hitta en växellåda med namnet {model.TransmissionType} i vårt system");

        // Kontrollera så att bränsletypen finns i systemet...
        var fueltype = await _context.FuelTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.FuelType.ToUpper().Trim());
        if (fueltype is null) return NotFound($"Vi kunde inte hitta någon bränsletyp med namnet {model.FuelType} i systemet");

        var vehicle = new VehicleModel
        {
            RegistrationNumber = model.RegistrationNumber,
            Model = model.Model,
            ModelYear = model.ModelYear,
            Mileage = model.Mileage,
            Manufacturer = make,
            FuelType = fueltype,
            TransmissionType = gearType,
            Description = model.Description,
            ImageUrl = model.ImageUrl
        };

        await _context.Vehicles.AddAsync(vehicle);

        if (await _context.SaveChangesAsync() > 0)
        {
            return Created(nameof(GetById), new { id = vehicle.Id });
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateVehicle(int id, VehicleUpdateViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdater bilen");

        //Vi måste kontrollera så att bilen inte redan är registrerad i systemet...
        // var vehicle = await _context.Vehicles.SingleOrDefaultAsync(c => c.RegistrationNumber!.ToUpper().Trim() == model.RegistrationNumber!.ToUpper().Trim());
        var vehicle = await _context.Vehicles.FindAsync(id);

        if (vehicle is null) return BadRequest($"Vi kan inte hitta en bil i systemet med {model.RegistrationNumber}");

        // Kontrollera att tillverkaren finn i systemet...
        var make = await _context.Manufacturers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Manufacturer.ToUpper().Trim());
        if (make is null) return NotFound($"Vi kunde inte hitta någon tillverkare med namnet {model.Manufacturer} i vårt system");

        // Kontrollera så att växellådetypen finns i systemet...
        var gearType = await _context.TransmissionTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.TransmissionType.ToUpper().Trim());
        if (gearType is null) return NotFound($"Vi kunde inte hitta en växellåda med namnet {model.TransmissionType} i vårt system");

        // Kontrollera så att bränsletypen finns i systemet...
        var fueltype = await _context.FuelTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.FuelType.ToUpper().Trim());
        if (fueltype is null) return NotFound($"Vi kunde inte hitta någon bränsletyp med namnet {model.FuelType} i systemet");

        vehicle.RegistrationNumber = model.RegistrationNumber;
        vehicle.Model = model.Model;
        vehicle.Manufacturer = make;
        vehicle.FuelType = fueltype;
        vehicle.TransmissionType = gearType;
        vehicle.ModelYear = model.ModelYear;
        vehicle.Mileage = model.Mileage;
        vehicle.Description = model.Description;
        vehicle.ImageUrl = model.ImageUrl;

        _context.Vehicles.Update(vehicle);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> MarkAsSold(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);

        if (vehicle is null) return NotFound($"Vi kan inte hitta någon bil med id: {id}");

        vehicle.Status = VehicleStatusEnum.Sold;

        _context.Vehicles.Update(vehicle);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("valued/{id}")]
    public async Task<ActionResult> SetValue(int id, VehicleSetValueViewModel model)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);

        if (vehicle is null) return NotFound($"Vi kan inte hitta någon bil med id: {id}");

        vehicle.Value = model.Value;

        _context.Vehicles.Update(vehicle);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVehicle(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);

        if (vehicle is null) return NotFound($"Vi kan inte hitta någon bil med id: {id}");

        _context.Vehicles.Remove(vehicle);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }
}

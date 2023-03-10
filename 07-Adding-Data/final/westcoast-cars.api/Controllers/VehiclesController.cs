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
            Value = v.Value
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
            Value = v.Value
        })
        .SingleOrDefaultAsync(v => v.RegistrationNumber!.ToUpper().Trim() == regNo.ToUpper().Trim());

        return Ok(result);
    }

    [HttpGet("make/{make}")]
    public async Task<ActionResult> GetByManufacturer(string make)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Where(c => c.Manufacturer.Name!.ToUpper().Trim() == make.ToUpper().Trim())
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

    [HttpGet("fueltype/{fueltype}")]
    public async Task<ActionResult> GetByFuelType(string fueltype)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Where(c => c.FuelType.Name!.ToUpper().Trim() == fueltype.ToUpper().Trim())
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

    [HttpGet("transmission/{transmissiontype}")]
    public async Task<ActionResult> GetByTransmissionType(string transmissiontype)
    {
        var result = await _context.Vehicles
        .Include(m => m.Manufacturer)
        .Include(f => f.FuelType)
        .Include(t => t.TransmissionType)
        .Where(c => c.TransmissionType.Name!.ToUpper().Trim() == transmissiontype.ToUpper().Trim())
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
        if (!ModelState.IsValid) return BadRequest("Information saknas f??r att kunna lagra bilen i systemet");

        //Vi m??ste kontrollera s?? att bilen inte redan ??r registrerad i systemet...
        var exists = await _context.Vehicles.SingleOrDefaultAsync(c => c.RegistrationNumber!.ToUpper().Trim() == model.RegistrationNumber!.ToUpper().Trim());

        if (exists is not null) return BadRequest($"Vi har redan registrerat en bil med registreringsnummer {model.RegistrationNumber}");

        // Kontrollera att tillverkaren finn i systemet...
        var make = await _context.Manufacturers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Manufacturer.ToUpper().Trim());
        if (make is null) return NotFound($"Vi kunde inte hitta n??gon tillverkare med namnet {model.Manufacturer} i v??rt system");

        // Kontrollera s?? att v??xell??detypen finns i systemet...
        var gearType = await _context.TransmissionTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.TransmissionType.ToUpper().Trim());
        if (gearType is null) return NotFound($"Vi kunde inte hitta en v??xell??da med namnet {model.TransmissionType} i v??rt system");

        // Kontrollera s?? att br??nsletypen finns i systemet...
        var fueltype = await _context.FuelTypes.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.FuelType.ToUpper().Trim());
        if (fueltype is null) return NotFound($"Vi kunde inte hitta n??gon br??nsletyp med namnet {model.FuelType} i systemet");

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
    public ActionResult UpdateVehicle(int id)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteVehicle(int id)
    {
        // G?? till databasen och ta bort bilen...
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult MarkAsSold(int id)
    {
        // G?? till databasen f??r att markera bilen som s??ld...
        return NoContent();
    }
}

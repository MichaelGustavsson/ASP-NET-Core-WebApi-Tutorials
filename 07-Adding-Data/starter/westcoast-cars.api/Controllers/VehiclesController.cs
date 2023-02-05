using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;
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
    public ActionResult AddVehicle()
    {
        return Created(nameof(GetById), new { message = "AddVehicle fungerar" });
    }

    [HttpPut("{id}")]
    public ActionResult UpdateVehicle(int id)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteVehicle(int id)
    {
        // Gå till databasen och ta bort bilen...
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult MarkAsSold(int id)
    {
        // Gå till databasen för att markera bilen som såld...
        return NoContent();
    }
}

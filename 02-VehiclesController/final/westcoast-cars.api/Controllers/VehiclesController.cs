using Microsoft.AspNetCore.Mvc;

namespace westcoast_cars.api.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
public class VehiclesController : ControllerBase
{
    [HttpGet()]
    public ActionResult List()
    {
        // Ok skapar ett json resultat med statuskoden 200...
        return Ok(new { message = "Lista bilar fungerar" });
    }

    //http://localhost:????/api/vehicles/5
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        return Ok(new { message = $"GetById fungerar {id}" });
    }

    [HttpGet("regno/{regNo}")]
    public ActionResult GetByRegistrationNumber(string regNo)
    {
        return Ok(new { message = $"GetByRegistrationNumber fungerar {regNo}" });
    }

    [HttpGet("make/{make}")]
    public ActionResult GetByManufacturer(string make)
    {
        return Ok(new { message = $"GetByManufacturer fungerar {make}" });
    }

    [HttpGet("model/{model}")]
    public ActionResult GetByModel(string model)
    {
        return Ok(new { message = $"GetByModel fungerar {model}" });
    }

    [HttpGet("modelyear/{year}")]
    public ActionResult GetByModelYear(string year)
    {
        return Ok(new { message = $"GetByModelYear fungerar {year}" });
    }

    [HttpGet("fueltype/{fueltype}")]
    public ActionResult GetByFuelType(string fueltype)
    {
        return Ok(new { message = $"GetByFuelType fungerar {fueltype}" });
    }

    [HttpGet("transmission/{transmissiontype}")]
    public ActionResult GetByTransmissionType(string transmissiontype)
    {
        return Ok(new { message = $"GetByTransmissionType fungerar {transmissiontype}" });
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

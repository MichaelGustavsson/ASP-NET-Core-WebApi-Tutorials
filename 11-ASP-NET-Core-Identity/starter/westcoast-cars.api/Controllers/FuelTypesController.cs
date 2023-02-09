using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Controllers
{
    [ApiController]
    [Route("api/v1/fueltypes")]
    public class FuelTypesController : ControllerBase, IBaseApiController
    {
        private readonly WestcoastCarsContext _context;
        public FuelTypesController(WestcoastCarsContext context)
        {
            _context = context;
        }

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.FuelTypes
            .Select(f => new
            {
                Id = f.Id,
                Name = f.Name
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.FuelTypes
            .Select(f => new
            {
                Id = f.Id,
                Name = f.Name
            })
            .SingleOrDefaultAsync(c => c.Id == id);

            return Ok(result);
        }

        [HttpGet("{name}/vehicles")]
        public async Task<IActionResult> List(string name)
        {
            var result = await _context.FuelTypes
            .Select(f => new
            {
                Id = f.Id,
                Name = f.Name,
                Vehicles = f.Vehicles!.Select(v => new
                {
                    Id = v.Id,
                    Name = $"{v.Manufacturer.Name} {v.Model} {v.ModelYear}"
                }).ToList()
            })
            .SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == name.ToUpper().Trim());

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(string name)
        {
            if (await _context.FuelTypes.SingleOrDefaultAsync(f => f.Name.ToLower().Trim() == name.ToLower().Trim()) is not null)
            {
                return BadRequest($"Det finns redan en bränsletyp med namnet {name} i systemet");
            }

            var fueltype = new FuelTypeModel { Name = name.Trim() };

            await _context.FuelTypes.AddAsync(fueltype);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = fueltype.Id }, new { Id = fueltype.Id, Name = fueltype.Name });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, string name)
        {
            var fueltype = await _context.FuelTypes.FindAsync(id);

            if (fueltype is null) return NotFound("Vi kunde inte hitta någon bränsletyp med id {id}");

            fueltype.Name = name.Trim();

            _context.FuelTypes.Update(fueltype);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fueltype = await _context.FuelTypes.FindAsync(id);

            if (fueltype is null) return NotFound($"Vi kunde inte hitta någon bränsletyp med id {id}");

            _context.FuelTypes.Remove(fueltype);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
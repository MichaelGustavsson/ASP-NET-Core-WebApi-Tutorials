using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Controllers
{
    [ApiController]
    [Route("api/v1/transmissions")]
    public class TransmissionsController : ControllerBase, IBaseApiController
    {
        private readonly WestcoastCarsContext _context;
        public TransmissionsController(WestcoastCarsContext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(string name)
        {
            if (await _context.TransmissionTypes.SingleOrDefaultAsync(f => f.Name.ToLower().Trim() == name.ToLower().Trim()) is not null)
            {
                return BadRequest($"Det finns redan en växellåde typ med namnet {name} i systemet");
            }

            var gearType = new TransmissionTypeModel { Name = name.Trim() };

            await _context.TransmissionTypes.AddAsync(gearType);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = gearType.Id }, new { Id = gearType.Id, Name = gearType.Name });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.TransmissionTypes
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
            var result = await _context.TransmissionTypes
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

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.TransmissionTypes
            .Select(f => new
            {
                Id = f.Id,
                Name = f.Name
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, string name)
        {
            var gearType = await _context.TransmissionTypes.FindAsync(id);

            if (gearType is null) return NotFound("Vi kunde inte hitta någon växellåde typ med id {id}");

            gearType.Name = name.Trim();

            _context.TransmissionTypes.Update(gearType);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
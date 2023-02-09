using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_cars.api.Data;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Controllers
{
    [ApiController]
    [Route("api/v1/manufacturers")]
    public class ManufacturersController : ControllerBase, IBaseApiController
    {
        private readonly WestcoastCarsContext _context;
        public ManufacturersController(WestcoastCarsContext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(string name)
        {
            if (await _context.Manufacturers.SingleOrDefaultAsync(f => f.Name.ToLower().Trim() == name.ToLower().Trim()) is not null)
            {
                return BadRequest($"Det finns redan en tillverkare med namnet {name} i systemet");
            }

            var make = new ManufacturerModel { Name = name.Trim() };

            await _context.Manufacturers.AddAsync(make);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = make.Id }, new { Id = make.Id, Name = make.Name });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var make = await _context.Manufacturers.FindAsync(id);

            if (make is null) return NotFound($"Vi kunde inte hitta någon tillverkare med id {id}");

            _context.Manufacturers.Remove(make);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Manufacturers
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
            var result = await _context.Manufacturers
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
            var result = await _context.Manufacturers
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
            var make = await _context.Manufacturers.FindAsync(id);

            if (make is null) return NotFound("Vi kunde inte hitta någon tillverkare med id {id}");

            make.Name = name.Trim();

            _context.Manufacturers.Update(make);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Data.Models;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly EducationContext _context;
        public SkillsController(EducationContext context)
        {
            _context = context;
        }

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Skills
            .Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var skill = await _context.Skills.Include(s => s.Teachers).SingleOrDefaultAsync(c => c.Id == id);

            if (skill is null) return NotFound($"Vi kunde inte hitta någon kompetens med id {id}");

            var result = new
            {
                Id = skill.Id,
                Name = skill.Name,
                Teachers = skill.Teachers?.Select(t => new
                {
                    Name = $"{t.FirstName} {t.LastName}",
                    Email = t.Email
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost("addskill")]
        public async Task<IActionResult> Add(string skillName)
        {
            var exists = await _context.Skills.SingleOrDefaultAsync(c => c.Name.ToUpper() == skillName.ToUpper());

            if (exists is not null) return BadRequest($"Kompetens {skillName} finns redan i systemet");

            var skill = new Skill { Name = skillName };

            await _context.Skills.AddAsync(skill);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { Id = skill.Id }, new { Id = skill.Id, Name = skill.Name });

            }
            return StatusCode(500, "Internal Server Error");
        }
    }
}
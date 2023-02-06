using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Data.Models;
using westcoast_education.api.ViewModels;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly EducationContext _context;
        public TeachersController(EducationContext context)
        {
            _context = context;
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Teachers
                .Select(
                    s => new
                    {
                        Id = s.Id,
                        BirthOfDate = s.BirthOfDate.ToShortDateString(),
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Phone = s.Phone,
                        Address = s.Address,
                        PostalCode = s.PostalCode,
                        City = s.City,
                        Country = s.Country,
                        Skills = s.Skills.Select(c => new { SkillName = c.Name }).ToList()
                    }
                ).SingleOrDefaultAsync(s => s.Id == id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeacherPostViewModel model)
        {
            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                BirthOfDate = model.BirthOfDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                PostalCode = model.PostalCode,
                City = model.City,
                Country = model.Country
            };

            foreach (var item in model.Skills)
            {
                var skill = await _context.Skills.SingleOrDefaultAsync(c => c.Name == item.SkillName);

                teacher.Skills = new List<Skill>();
                if (skill is not null)
                {
                    teacher.Skills.Add(skill);
                }
            }

            await _context.Teachers.AddAsync(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { Id = teacher.Id }, new
                {
                    Id = teacher.Id,
                    BirthOfDate = teacher.BirthOfDate.ToShortDateString(),
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    Address = teacher.Address,
                    PostalCode = teacher.PostalCode,
                    City = teacher.City,
                    Country = teacher.Country
                });
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
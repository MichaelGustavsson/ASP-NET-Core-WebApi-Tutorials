using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Data.Models;
using westcoast_education.api.ViewModels;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : ControllerBase
    {
        private readonly EducationContext _context;
        public StudentsController(EducationContext context)
        {
            _context = context;
        }

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Students
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
                        Country = s.Country
                    }
                ).ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Students
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
                        Country = s.Country
                    }
                ).SingleOrDefaultAsync(s => s.Id == id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonPostViewModel model)
        {

            var exists = await _context.Students.SingleOrDefaultAsync(s => s.Email == model.Email);

            if (exists is not null) BadRequest($"Studenten med e-post {model.Email} är redan registrerad i systemet.");

            var student = new Student
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

            await _context.Students.AddAsync(student);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { Id = student.Id }, new
                {
                    Id = student.Id,
                    BirthOfDate = student.BirthOfDate.ToShortDateString(),
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    Phone = student.Phone,
                    Address = student.Address,
                    PostalCode = student.PostalCode,
                    City = student.City,
                    Country = student.Country
                });
            }
            return StatusCode(500, "Internal Server Error");
        }
    }
}
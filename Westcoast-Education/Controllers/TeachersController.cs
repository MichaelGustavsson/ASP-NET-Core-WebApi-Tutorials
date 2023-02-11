using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wescoast_education.api.Data;
using wescoast_education.api.Models;
using wescoast_education.api.ViewModels.Teachers;

namespace wescoast_education.api.Controllers
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

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Teachers
                .Select(t => new
                {
                    TeacherId = t.Id,
                    Name = $"{t.FirstName} {t.LastName}",
                    Email = t.Email
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Teachers
                .Select(t => new
                {
                    TeacherId = t.Id,
                    Name = $"{t.FirstName} {t.LastName}",
                    Email = t.Email,
                    Phone = t.Phone,
                    Address = t.Address,
                    PostalCode = t.PostalCode,
                    City = t.City,
                    Courses = t.Courses.Select(c => new
                    {
                        CourseId = c.CourseId,
                        Title = c.Title
                    }).ToList(),
                    Skills = t.Skills.Select(s => new
                    {
                        SkillId = s.Id,
                        Name = s.SkillName
                    }).ToList()
                })
                .SingleOrDefaultAsync(c => c.TeacherId == id);

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(TeacherPostViewModel model)
        {
            var exists = await _context.Teachers.SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToUpper().Trim());

            if (exists is not null) return BadRequest($"En lärare med e-post {model.Email} är redan registrerad i systemet.");

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
                City = model.City
            };

            await _context.Teachers.AddAsync(teacher);

            foreach (var skill in model.Skills)
            {
                var s = await _context.Skill.SingleOrDefaultAsync(c => c.SkillName.ToLower().Trim() == skill.ToLower().Trim());
                if (s is null)
                {
                    s = new Skill
                    {
                        SkillName = skill.Trim()
                    };

                    await _context.Skill.AddAsync(s);
                }
                if (teacher.Skills is null) teacher.Skills = new List<Skill>();
                teacher.Skills.Add(s);
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return StatusCode(201);
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addskill/{teacherId}/{skillId}")]
        public async Task<IActionResult> AddSkill(Guid teacherId, Guid skillId)
        {
            var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Id == teacherId);
            if (teacher is null) return NotFound("Tyvärr vi kunde inte hitta läraren");

            var skill = await _context.Skill.SingleOrDefaultAsync(c => c.Id == skillId);
            if (skill is null) return NotFound("Tyvärr kunder vi inte hitta kunskapsområdet");

            if (teacher.Skills is null) teacher.Skills = new List<Skill>();

            teacher.Skills.Add(skill);

            try
            {
                _context.Teachers.Update(teacher);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorMessage", ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "");
                return ValidationProblem();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
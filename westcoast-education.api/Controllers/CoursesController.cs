using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Data.Models;
using westcoast_education.api.ViewModels;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly EducationContext _context;
        public CoursesController(EducationContext context)
        {
            _context = context;
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Courses
                .Include(c => c.Students)
                .SingleOrDefaultAsync(c => c.CourseId == id);

            var course = new
            {
                Id = result.CourseId,
                Title = result.Title,
                CourseNumber = result.CourseNumber,
                Duration = result.Duration,
                StartDate = result.StartDate,
                Students = result.Students.Select(s => new
                {
                    Id = s.Id,
                    Name = $"{s.FirstName} {s.LastName}"
                }).ToList()
            };

            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CoursePostViewModel model)
        {
            var exists = await _context.Courses.SingleOrDefaultAsync(
                c => c.CourseNumber == model.CourseNumber &&
                c.StartDate == model.StartDate
            );

            if (exists is not null) return BadRequest($"Kurs med kursnummer {model.CourseNumber} och kursstart {model.StartDate.ToShortDateString()} finns redan i systemet.");

            var course = new Course
            {
                CourseId = Guid.NewGuid(),
                Title = model.Title,
                CourseNumber = model.CourseNumber,
                Duration = model.Duration,
                StartDate = model.StartDate,
                EndDate = model.StartDate.AddDays(model.Duration * 7)
            };

            await _context.Courses.AddAsync(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                // Genererar ett 201 resultat med en location header som 
                // ger sökvägen till den nyligen tillagda resursen...
                return CreatedAtAction(nameof(GetById), new { Id = course.CourseId }, new
                {
                    Id = course.CourseId,
                    Title = course.Title,
                    CourseNumber = course.CourseNumber,
                    Duration = course.Duration,
                    StartDate = course.StartDate
                });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addstudent")]
        public async Task<IActionResult> AddStudentToCourse(Guid courseId, Guid studentId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {courseId}");

            var student = await _context.Students.FindAsync(studentId);
            if (student is null) return NotFound($"Tyvärr kunde vi inte hitta någon student med id {studentId}");

            if (course.Students is null) course.Students = new List<Student>();

            course.Students.Add(student);

            _context.Update(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addteacher")]
        public async Task<IActionResult> AddTeacherToCourse(Guid courseId, Guid teacherId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {courseId}");

            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher is null) return NotFound($"Tyvärr kunde vi inte hitta någon lärare med id {teacherId}");

            course.Teacher = teacher;

            _context.Update(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
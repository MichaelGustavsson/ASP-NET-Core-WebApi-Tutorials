using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wescoast_education.api.Data;
using wescoast_education.api.Models;
using wescoast_education.api.ViewModels.Students;

namespace wescoast_education.api.Controllers
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
        public async Task<IActionResult> List()
        {
            var result = await _context.Students
                .Select(s => new
                {
                    StudentId = s.Id,
                    Name = $"{s.FirstName} {s.LastName}",
                    Email = s.Email
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Students
                .Select(s => new
                {
                    StudentId = s.Id,
                    BirthOfDate = s.BirthOfDate.ToShortDateString(),
                    Name = $"{s.FirstName} {s.LastName}",
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    PostalCode = s.PostalCode,
                    City = s.City,
                    Courses = s.StudentCourses.Select(c => new
                    {
                        CourseId = c.CourseId,
                        Title = c.Course.Title,
                        Status = ((CourseStatusEnum)c.Status).ToString()
                    }).ToList(),
                })
                .SingleOrDefaultAsync(c => c.StudentId == id);

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(StudentPostViewModel model)
        {
            var exists = await _context.Students.SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToUpper().Trim());

            if (exists is not null) return BadRequest($"En student med e-post {model.Email} är redan registrerad i systemet.");

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
                City = model.City
            };

            await _context.Students.AddAsync(student);

            foreach (var item in model.Courses)
            {
                var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == item);

                if (course is not null)
                {
                    if (student.StudentCourses is null) student.StudentCourses = new List<StudentCourse>();
                    student.StudentCourses.Add(new StudentCourse { Course = course, Student = student, Status = CourseStatusEnum.NoStatus });
                }
            }


            if (await _context.SaveChangesAsync() > 0)
            {
                return StatusCode(201);
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addcourse/{studentId}/{courseId}")]
        public async Task<IActionResult> AddStudentToCourse(Guid studentId, Guid courseId)
        {

            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);
            if (student is null) return NotFound("Vi kunde inte hitta studenten");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            if (course is null) return NotFound("Vi  kunde inte hitta kursen");

            await _context.StudentCourse.AddAsync(new StudentCourse { Course = course, Student = student, Status = CourseStatusEnum.NoStatus });

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("changestatus/{studentId}/{courseId}/{status}")]
        public async Task<IActionResult> ChangeStatus(Guid studentId, Guid courseId, CourseStatusEnum status)
        {

            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);
            if (student is null) return NotFound("Vi kunde inte hitta studenten");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            if (course is null) return NotFound("Vi  kunde inte hitta kursen");

            var studentCourse = await _context.StudentCourse.SingleOrDefaultAsync(c => c.StudentId == studentId && c.CourseId == courseId);
            if (studentCourse is null) return NotFound("Vi kunde inte hitta kursen för vald student");

            studentCourse.Status = status;

            _context.StudentCourse.Update(studentCourse);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
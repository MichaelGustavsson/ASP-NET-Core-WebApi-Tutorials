using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wescoast_education.api.Data;
using wescoast_education.api.Models;
using wescoast_education.api.ViewModels.Courses;

namespace wescoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    [Produces("application/json")]
    public class CoursesController : ControllerBase
    {
        private readonly EducationContext _context;
        public CoursesController(EducationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Listar alla kurser i systemet.
        /// </summary>
        /// <returns>En lista med kurser</returns>
        /// <response code="200">Returnerar en lista med kurser</response>        
        [HttpGet("listall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Courses
            .Select(c => new
            {
                CourseId = c.CourseId,
                Title = c.Title,
                StartDate = c.StartDate.ToShortDateString(),
                Teacher = c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : "Saknas",
                Students = c.StudentCourses.Select(s => new
                {
                    StudentId = s.StudentId,
                    Name = s.Student.FirstName + " " + s.Student.LastName
                }).ToList()
            })
            .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Hämtar en kurs baserat på kurs id(courseId).
        /// </summary>
        /// <returns>En kurs med information och lärare och studenter</returns>
        /// <response code="200">Returnerar en kurs med information</response> 
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Courses
                .Select(c => new
                {
                    CourseId = c.CourseId,
                    StartDate = c.StartDate.ToShortDateString(),
                    EndDate = c.EndDate.ToShortDateString(),
                    Teacher = c.Teacher != null ? new
                    {
                        Id = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName,
                        Email = c.Teacher.Email,
                        Phone = c.Teacher.Phone
                    } : null,
                    Students = c.StudentCourses.Select(s => new
                    {
                        Id = s.Student.Id,
                        FirstName = s.Student.FirstName,
                        LastName = s.Student.LastName,
                        Email = s.Student.Email,
                        Phone = s.Student.Phone,
                        Status = ((CourseStatusEnum)s.Status).ToString()
                    })
                })
                .SingleOrDefaultAsync(c => c.CourseId == id);

            return Ok(result);
        }

        /// <summary>
        /// Skapar och lägger till en ny kurs i systemet.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>En länk till den nya kursen och ett objekt med kursens information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/courses
        ///     {
        ///        "title": "Kurstitle",
        ///        "courseNumber": 12345,
        ///        "duration": 5,
        ///        "startDate": "2023-09-11"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returnerar den tillagda kursen</response>
        /// <response code="400">Om kursen redan existerar eller om det saknas information i anropet</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCourse(CoursePostViewModel model)
        {
            var exists = await _context.Courses.SingleOrDefaultAsync(
                c => c.CourseNumber == model.CourseNumber &&
                c.StartDate == model.StartDate);

            if (exists is not null) return BadRequest($"Kurs med kursnummer {model.CourseNumber} och kurs start {model.StartDate.ToShortDateString()} existerar redan");

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
                var result = new
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    StartDate = course.StartDate.ToShortDateString(),
                    EndDate = course.EndDate.ToShortDateString()
                };
                return CreatedAtAction(nameof(GetById), new { Id = course.CourseId }, result);

            }

            return StatusCode(500, "Internal Server Error");
        }

        /// <summary>
        /// Lägger till en ny student till en befintlig kurs.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns>Inget</returns>
        /// <response code="204"></response>
        /// <response code="404">Om kurs eller student inte finns i systemet</response>
        [HttpPatch("addstudent/{courseId}/{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddStudent(Guid courseId, Guid studentId)
        {
            // Check if the course exists...
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            if (course is null) return NotFound("Kunde inte hitta kursen");

            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);
            if (student is null) return NotFound("Kunde inte hitta studenten");

            var studentCourse = new StudentCourse
            {
                Course = course,
                Student = student
            };

            await _context.StudentCourse.AddAsync(studentCourse);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}
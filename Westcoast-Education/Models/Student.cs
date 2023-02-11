using System.ComponentModel.DataAnnotations.Schema;

namespace wescoast_education.api.Models
{
    public class Student : Person
    {

        // Navigering...

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
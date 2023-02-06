using System.ComponentModel.DataAnnotations.Schema;

namespace westcoast_education.api.Data.Models
{
    public class Student : Person
    {
        // Navigerings egenskaper...
        public Guid? CourseId { get; set; }

        // Composition
        public Course Course { get; set; }
    }
}
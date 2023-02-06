using System.ComponentModel.DataAnnotations.Schema;

namespace westcoast_education.api.Data.Models
{
    public class Course
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public int CourseNumber { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Navigerings egenskaper...
        // Aggregation
        public ICollection<Student> Students { get; set; }

        public Guid? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

    }
}
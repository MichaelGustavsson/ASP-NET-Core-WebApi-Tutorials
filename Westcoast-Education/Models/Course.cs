namespace wescoast_education.api.Models
{
    public class Course
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public int CourseNumber { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigering...
        // Aggregation...
        public ICollection<StudentCourse> StudentCourses { get; set; }

        // Composition
        public Guid? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
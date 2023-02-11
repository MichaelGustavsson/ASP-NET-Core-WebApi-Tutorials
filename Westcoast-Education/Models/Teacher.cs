namespace wescoast_education.api.Models
{
    public class Teacher : Person
    {
        // Navigation...
        // Aggregation...
        public ICollection<Course> Courses { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
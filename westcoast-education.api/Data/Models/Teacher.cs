namespace westcoast_education.api.Data.Models
{
    public class Teacher : Person
    {
        // Navigerings egenskaper
        // Att en lärare kan undervisa flera kurser...
        public ICollection<Course> Courses { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
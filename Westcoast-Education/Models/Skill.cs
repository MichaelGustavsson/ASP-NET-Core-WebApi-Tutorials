namespace wescoast_education.api.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string SkillName { get; set; }

        // Navigation...
        // Ett kunskapsområde kan vara kopplat till flera lärare...
        public ICollection<Teacher> Teachers { get; set; }
    }
}
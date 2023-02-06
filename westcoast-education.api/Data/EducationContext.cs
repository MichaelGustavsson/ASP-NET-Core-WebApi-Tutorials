using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data.Models;

namespace westcoast_education.api.Data
{
    public class EducationContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public EducationContext(DbContextOptions options) : base(options)
        {
        }
    }
}
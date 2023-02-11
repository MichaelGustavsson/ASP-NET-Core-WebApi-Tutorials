using System.ComponentModel.DataAnnotations;

namespace wescoast_education.api.ViewModels.Courses
{
    public class CoursePostViewModel
    {
        [Required(ErrorMessage = "Kurstitel måste anges")]
        [StringLength(128, MinimumLength = 6)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Kursnummer måste anges")]
        public int CourseNumber { get; set; }
        [Required(ErrorMessage = "Kursveckor måste anges")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Start datum för kurs måste anges")]
        public DateTime StartDate { get; set; }
    }
}
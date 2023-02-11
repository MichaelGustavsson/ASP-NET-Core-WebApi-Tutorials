namespace wescoast_education.api.ViewModels.Teachers
{
    public class TeacherPostViewModel : PersonViewModel
    {
        public IList<string> Skills { get; set; } = new List<string>();
    }
}
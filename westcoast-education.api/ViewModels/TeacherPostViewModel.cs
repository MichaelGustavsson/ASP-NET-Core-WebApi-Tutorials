namespace westcoast_education.api.ViewModels
{
    public class TeacherPostViewModel : PersonPostViewModel
    {
        public IList<SkillViewModel> Skills { get; set; }
    }
}
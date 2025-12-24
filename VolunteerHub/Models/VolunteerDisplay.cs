namespace VolunteerHub.Models
{
    public class VolunteerDisplay
    {
        public Volunteer Volunteer { get; set; }
        public string DisplayName => $"{Volunteer.FirstName} {Volunteer.LastName}";

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class ProjectDisplay
    {
        public Project Project { get; set; }
        public string DisplayName => Project.ProjectName;

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
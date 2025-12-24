using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;

namespace VolunteerHub.Views
{
    public partial class DashboardPage : ContentPage
    {
        private readonly AppDbContext _dbContext;

        public DashboardPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadStatistics();
        }

        private async Task LoadStatistics()
        {
            try
            {
                // Total and Active Volunteers
                var totalVolunteers = await _dbContext.Volunteers.CountAsync();
                var activeVolunteers = await _dbContext.Volunteers.CountAsync(v => v.Status == "Active");
                TotalVolunteersLabel.Text = totalVolunteers.ToString();
                ActiveVolunteersLabel.Text = activeVolunteers.ToString();

                // Total and Active Projects
                var totalProjects = await _dbContext.Projects.CountAsync();
                var activeProjects = await _dbContext.Projects.CountAsync(p => p.Status == "Active");
                var completedProjects = await _dbContext.Projects.CountAsync(p => p.Status == "Completed");
                var planningProjects = await _dbContext.Projects.CountAsync(p => p.Status == "Planning");

                TotalProjectsLabel.Text = totalProjects.ToString();
                ActiveProjectsLabel.Text = activeProjects.ToString();
                CompletedProjectsLabel.Text = completedProjects.ToString();
                PlanningProjectsLabel.Text = planningProjects.ToString();

                // Assignments and Hours
                var totalAssignments = await _dbContext.VolunteerAssignments.CountAsync();
                var totalHours = await _dbContext.VolunteerAssignments.SumAsync(a => a.HoursContributed);

                TotalAssignmentsLabel.Text = totalAssignments.ToString();
                TotalHoursLabel.Text = totalHours.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load statistics: {ex.Message}", "OK");
            }
        }

        private async void OnManageVolunteersClicked(object sender, EventArgs e)
        {
            // Navigate to Volunteers page
            await Navigation.PushAsync(new VolunteersPage(_dbContext));
        }

        private async void OnManageProjectsClicked(object sender, EventArgs e)
        {
            // Navigate to Projects page
            await Navigation.PushAsync(new ProjectsPage(_dbContext));
        }

        private async void OnManageAssignmentsClicked(object sender, EventArgs e)
        {
            // Navigate to Assignments page
            await Navigation.PushAsync(new AssignmentsPage(_dbContext));
        }

        private async void OnExportReportsClicked(object sender, EventArgs e)
        {
            // Navigate to Export page
            await Navigation.PushAsync(new ExportPage(_dbContext));
        }

        private async void OnHelpAboutClicked(object sender, EventArgs e)
        {
            // Navigate to About page
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
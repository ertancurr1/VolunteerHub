using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;
using VolunteerHub.Services;

namespace VolunteerHub.Views
{
    public partial class ExportPage : ContentPage
    {
        private readonly AppDbContext _dbContext;

        public ExportPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private async void OnExportVolunteersCsvClicked(object sender, EventArgs e)
        {
            try
            {
                var volunteers = await _dbContext.Volunteers.ToListAsync();
                var csv = ExportService.ExportVolunteersToCsv(volunteers);
                await SaveFile("volunteers.csv", csv);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnExportVolunteersJsonClicked(object sender, EventArgs e)
        {
            try
            {
                var volunteers = await _dbContext.Volunteers.ToListAsync();
                var json = ExportService.ExportToJson(volunteers);
                await SaveFile("volunteers.json", json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnExportProjectsCsvClicked(object sender, EventArgs e)
        {
            try
            {
                var projects = await _dbContext.Projects.ToListAsync();
                var csv = ExportService.ExportProjectsToCsv(projects);
                await SaveFile("projects.csv", csv);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnExportProjectsJsonClicked(object sender, EventArgs e)
        {
            try
            {
                var projects = await _dbContext.Projects.ToListAsync();
                var json = ExportService.ExportToJson(projects);
                await SaveFile("projects.json", json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnExportAssignmentsCsvClicked(object sender, EventArgs e)
        {
            try
            {
                var assignments = await _dbContext.VolunteerAssignments
                    .Include(a => a.Volunteer)
                    .Include(a => a.Project)
                    .ToListAsync();
                var csv = ExportService.ExportAssignmentsToCsv(assignments);
                await SaveFile("assignments.csv", csv);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnExportAssignmentsJsonClicked(object sender, EventArgs e)
        {
            try
            {
                var assignments = await _dbContext.VolunteerAssignments
                    .Include(a => a.Volunteer)
                    .Include(a => a.Project)
                    .ToListAsync();
                var json = ExportService.ExportToJson(assignments);
                await SaveFile("assignments.json", json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void OnGenerateFullReportClicked(object sender, EventArgs e)
        {
            try
            {
                var volunteers = await _dbContext.Volunteers.CountAsync();
                var activeVolunteers = await _dbContext.Volunteers.CountAsync(v => v.Status == "Active");
                var projects = await _dbContext.Projects.CountAsync();
                var activeProjects = await _dbContext.Projects.CountAsync(p => p.Status == "Active");
                var assignments = await _dbContext.VolunteerAssignments.CountAsync();
                var totalHours = await _dbContext.VolunteerAssignments.SumAsync(a => a.HoursContributed);

                string report = $"VolunteerHub Summary Report\n" +
                              $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}\n\n" +
                              $"VOLUNTEERS:\n" +
                              $"  Total: {volunteers}\n" +
                              $"  Active: {activeVolunteers}\n\n" +
                              $"PROJECTS:\n" +
                              $"  Total: {projects}\n" +
                              $"  Active: {activeProjects}\n\n" +
                              $"ASSIGNMENTS:\n" +
                              $"  Total: {assignments}\n" +
                              $"  Total Hours Contributed: {totalHours}\n";

                await SaveFile("summary_report.txt", report);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Report generation failed: {ex.Message}", "OK");
            }
        }

        private async Task SaveFile(string filename, string content)
        {
            try
            {
                // Save to Downloads folder
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string filePath = Path.Combine(downloadsPath, filename);

                await File.WriteAllTextAsync(filePath, content);

                await DisplayAlert("Success", $"File saved to:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save file: {ex.Message}", "OK");
            }
        }
    }
}
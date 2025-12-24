using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class ProjectsPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private List<Project> _allProjects;

        public ProjectsPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProjects();
        }

        private async Task LoadProjects()
        {
            try
            {
                // Load all projects from database
                _allProjects = await _dbContext.Projects
                    .OrderByDescending(p => p.StartDate)
                    .ToListAsync();

                // Display in CollectionView
                ProjectsCollectionView.ItemsSource = _allProjects;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load projects: {ex.Message}", "OK");
            }
        }

        private async void OnSearchPressed(object sender, EventArgs e)
        {
            await PerformSearch();
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            await PerformSearch();
        }

        private async Task PerformSearch()
        {
            try
            {
                var searchText = SearchBar.Text?.ToLower() ?? "";

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // Show all projects if search is empty
                    ProjectsCollectionView.ItemsSource = _allProjects;
                }
                else
                {
                    // Filter projects by name, description, or status
                    var filteredProjects = _allProjects.Where(p =>
                        p.ProjectName.ToLower().Contains(searchText) ||
                        (p.Description != null && p.Description.ToLower().Contains(searchText)) ||
                        p.Status.ToLower().Contains(searchText)
                    ).ToList();

                    ProjectsCollectionView.ItemsSource = filteredProjects;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
            }
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            // Navigate to add project page
            await Navigation.PushAsync(new ProjectDetailPage(_dbContext, null));
        }

        private async void OnEditProjectClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var project = button?.CommandParameter as Project;

                if (project != null)
                {
                    // Navigate to edit project page
                    await Navigation.PushAsync(new ProjectDetailPage(_dbContext, project));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to edit project: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteProjectClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var project = button?.CommandParameter as Project;

                if (project != null)
                {
                    // Confirm deletion
                    bool confirm = await DisplayAlert(
                        "Confirm Delete",
                        $"Are you sure you want to delete '{project.ProjectName}'?",
                        "Yes",
                        "No");

                    if (confirm)
                    {
                        // Delete from database
                        _dbContext.Projects.Remove(project);
                        await _dbContext.SaveChangesAsync();

                        await DisplayAlert("Success", "Project deleted successfully!", "OK");

                        // Reload the list
                        await LoadProjects();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete project: {ex.Message}", "OK");
            }
        }

        private async void OnViewDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var project = button?.CommandParameter as Project;

                if (project != null)
                {
                    // Show detailed information
                    string endDateStr = project.EndDate.HasValue
                        ? project.EndDate.Value.ToString("yyyy-MM-dd")
                        : "Ongoing";

                    string details = $"Project: {project.ProjectName}\n" +
                                   $"Description: {project.Description}\n" +
                                   $"Start Date: {project.StartDate:yyyy-MM-dd}\n" +
                                   $"End Date: {endDateStr}\n" +
                                   $"Status: {project.Status}\n" +
                                   $"Required Volunteers: {project.RequiredVolunteers}";

                    await DisplayAlert("Project Details", details, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to show details: {ex.Message}", "OK");
            }
        }
    }
}
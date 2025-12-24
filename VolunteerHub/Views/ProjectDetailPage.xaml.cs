using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class ProjectDetailPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private readonly Project _project;
        private readonly bool _isEditMode;

        public ProjectDetailPage(AppDbContext dbContext, Project project)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _project = project;
            _isEditMode = project != null;

            LoadProjectData();
        }

        private void LoadProjectData()
        {
            if (_isEditMode)
            {
                // Edit mode - populate fields with existing data
                HeaderLabel.Text = "Edit Project";
                ProjectNameEntry.Text = _project.ProjectName;
                DescriptionEditor.Text = _project.Description;
                StartDatePicker.Date = _project.StartDate;

                if (_project.EndDate.HasValue)
                {
                    EndDatePicker.Date = _project.EndDate.Value;
                    NoEndDateCheckBox.IsChecked = false;
                }
                else
                {
                    NoEndDateCheckBox.IsChecked = true;
                    EndDatePicker.IsEnabled = false;
                }

                StatusPicker.SelectedItem = _project.Status;
                RequiredVolunteersEntry.Text = _project.RequiredVolunteers.ToString();
            }
            else
            {
                // Add mode - set defaults
                HeaderLabel.Text = "Add New Project";
                StartDatePicker.Date = DateTime.Now;
                EndDatePicker.Date = DateTime.Now.AddMonths(3); // Default 3 months
                StatusPicker.SelectedIndex = 0; // Default to "Planning"
                RequiredVolunteersEntry.Text = "5"; // Default value
            }
        }

        private void OnNoEndDateChanged(object sender, CheckedChangedEventArgs e)
        {
            // Enable/disable EndDatePicker based on checkbox
            EndDatePicker.IsEnabled = !e.Value;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(ProjectNameEntry.Text))
                {
                    await DisplayAlert("Validation Error", "Project name is required", "OK");
                    return;
                }

                if (StatusPicker.SelectedItem == null)
                {
                    await DisplayAlert("Validation Error", "Status is required", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(RequiredVolunteersEntry.Text) ||
                    !int.TryParse(RequiredVolunteersEntry.Text, out int requiredVolunteers) ||
                    requiredVolunteers < 0)
                {
                    await DisplayAlert("Validation Error", "Please enter a valid number for required volunteers", "OK");
                    return;
                }

                // Validate dates
                DateTime? endDate = NoEndDateCheckBox.IsChecked ? null : EndDatePicker.Date;
                if (endDate.HasValue && endDate.Value < StartDatePicker.Date)
                {
                    await DisplayAlert("Validation Error", "End date cannot be before start date", "OK");
                    return;
                }

                if (_isEditMode)
                {
                    // Update existing project
                    _project.ProjectName = ProjectNameEntry.Text.Trim();
                    _project.Description = DescriptionEditor.Text?.Trim();
                    _project.StartDate = StartDatePicker.Date;
                    _project.EndDate = endDate;
                    _project.Status = StatusPicker.SelectedItem.ToString();
                    _project.RequiredVolunteers = requiredVolunteers;
                    _project.LastModified = DateTime.Now;

                    await DisplayAlert("Success", "Project updated successfully!", "OK");
                }
                else
                {
                    // Create new project
                    var newProject = new Project
                    {
                        ProjectName = ProjectNameEntry.Text.Trim(),
                        Description = DescriptionEditor.Text?.Trim(),
                        StartDate = StartDatePicker.Date,
                        EndDate = endDate,
                        Status = StatusPicker.SelectedItem.ToString(),
                        RequiredVolunteers = requiredVolunteers,
                        CreatedDate = DateTime.Now,
                        LastModified = DateTime.Now
                    };

                    _dbContext.Projects.Add(newProject);
                    await DisplayAlert("Success", "Project added successfully!", "OK");
                }

                // Save to database
                await _dbContext.SaveChangesAsync();

                // Go back to previous page
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save project: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Go back without saving
            await Navigation.PopAsync();
        }
    }
}
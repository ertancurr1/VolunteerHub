using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class AssignmentDetailPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private readonly VolunteerAssignment _assignment;
        private readonly bool _isEditMode;
        private List<Volunteer> _volunteers;
        private List<Project> _projects;

        public AssignmentDetailPage(AppDbContext dbContext, VolunteerAssignment assignment)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _assignment = assignment;
            _isEditMode = assignment != null;

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Load volunteers and projects for dropdowns
                _volunteers = await _dbContext.Volunteers
                    .Where(v => v.Status == "Active")
                    .OrderBy(v => v.FirstName)
                    .ToListAsync();

                _projects = await _dbContext.Projects
                    .Where(p => p.Status == "Active" || p.Status == "Planning")
                    .OrderBy(p => p.ProjectName)
                    .ToListAsync();

                // Create display wrappers for volunteers
                var volunteerDisplayList = _volunteers.Select(v => new VolunteerDisplay
                {
                    Volunteer = v
                }).ToList();

                // Create display wrappers for projects
                var projectDisplayList = _projects.Select(p => new ProjectDisplay
                {
                    Project = p
                }).ToList();

                // Populate pickers
                VolunteerPicker.ItemsSource = volunteerDisplayList;
                ProjectPicker.ItemsSource = projectDisplayList;

                if (_isEditMode)
                {
                    // Edit mode - populate fields with existing data
                    HeaderLabel.Text = "Edit Assignment";

                    // Find and select the volunteer
                    var volunteerDisplay = volunteerDisplayList
                        .FirstOrDefault(vd => vd.Volunteer.VolunteerId == _assignment.VolunteerId);
                    if (volunteerDisplay != null)
                    {
                        VolunteerPicker.SelectedItem = volunteerDisplay;
                    }

                    // Find and select the project
                    var projectDisplay = projectDisplayList
                        .FirstOrDefault(pd => pd.Project.ProjectId == _assignment.ProjectId);
                    if (projectDisplay != null)
                    {
                        ProjectPicker.SelectedItem = projectDisplay;
                    }

                    AssignmentDatePicker.Date = _assignment.AssignmentDate;
                    HoursEntry.Text = _assignment.HoursContributed.ToString();
                    NotesEditor.Text = _assignment.Notes;
                }
                else
                {
                    // Add mode - set defaults
                    HeaderLabel.Text = "New Assignment";
                    AssignmentDatePicker.Date = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate selections
                if (VolunteerPicker.SelectedItem == null)
                {
                    await DisplayAlert("Validation Error", "Please select a volunteer", "OK");
                    return;
                }

                if (ProjectPicker.SelectedItem == null)
                {
                    await DisplayAlert("Validation Error", "Please select a project", "OK");
                    return;
                }

                var selectedVolunteerDisplay = VolunteerPicker.SelectedItem as VolunteerDisplay;
                var selectedProjectDisplay = ProjectPicker.SelectedItem as ProjectDisplay;

                if (selectedVolunteerDisplay?.Volunteer == null || selectedProjectDisplay?.Project == null)
                {
                    await DisplayAlert("Error", "Failed to get selected volunteer or project", "OK");
                    return;
                }

                var selectedVolunteer = selectedVolunteerDisplay.Volunteer;
                var selectedProject = selectedProjectDisplay.Project;

                // Validate hours
                if (!int.TryParse(HoursEntry.Text, out int hours) || hours < 0)
                {
                    await DisplayAlert("Validation Error", "Please enter valid hours (0 or greater)", "OK");
                    return;
                }

                if (_isEditMode)
                {
                    // Update existing assignment
                    _assignment.VolunteerId = selectedVolunteer.VolunteerId;
                    _assignment.ProjectId = selectedProject.ProjectId;
                    _assignment.AssignmentDate = AssignmentDatePicker.Date;
                    _assignment.HoursContributed = hours;
                    _assignment.Notes = NotesEditor.Text?.Trim();

                    // Save to database FIRST
                    await _dbContext.SaveChangesAsync();

                    // Show success AFTER saving
                    await DisplayAlert("Success", "Assignment updated successfully!", "OK");
                }
                else
                {
                    // Check if assignment already exists
                    var existingAssignment = await _dbContext.VolunteerAssignments
                        .FirstOrDefaultAsync(va =>
                            va.VolunteerId == selectedVolunteer.VolunteerId &&
                            va.ProjectId == selectedProject.ProjectId);

                    if (existingAssignment != null)
                    {
                        await DisplayAlert("Duplicate",
                            "This volunteer is already assigned to this project. Use Edit to update it.",
                            "OK");
                        return;
                    }

                    // Create new assignment
                    var newAssignment = new VolunteerAssignment
                    {
                        VolunteerId = selectedVolunteer.VolunteerId,
                        ProjectId = selectedProject.ProjectId,
                        AssignmentDate = AssignmentDatePicker.Date,
                        HoursContributed = hours,
                        Notes = string.IsNullOrWhiteSpace(NotesEditor.Text) ? null : NotesEditor.Text
                    };

                    _dbContext.VolunteerAssignments.Add(newAssignment);

                    // Save to database FIRST
                    await _dbContext.SaveChangesAsync();

                    // Show success AFTER saving
                    await DisplayAlert("Success", "Assignment created successfully!", "OK");
                }

                // Go back to previous page
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Show detailed error
                await DisplayAlert("Error", $"Failed to save assignment: {ex.Message}\n\nInner: {ex.InnerException?.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Go back without saving
            await Navigation.PopAsync();
        }
    }
}
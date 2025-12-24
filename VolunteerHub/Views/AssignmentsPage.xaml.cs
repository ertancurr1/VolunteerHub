using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class AssignmentsPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private List<VolunteerAssignment> _allAssignments;

        public AssignmentsPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAssignments();
        }

        private async Task LoadAssignments()
        {
            try
            {
                // Load all assignments with related volunteer and project data
                _allAssignments = await _dbContext.VolunteerAssignments
                    .Include(va => va.Volunteer)
                    .Include(va => va.Project)
                    .OrderByDescending(va => va.AssignmentDate)
                    .ToListAsync();

                // Display in CollectionView
                AssignmentsCollectionView.ItemsSource = _allAssignments;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load assignments: {ex.Message}", "OK");
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
                    // Show all assignments if search is empty
                    AssignmentsCollectionView.ItemsSource = _allAssignments;
                }
                else
                {
                    // Filter assignments by volunteer name or project name
                    var filteredAssignments = _allAssignments.Where(a =>
                        a.Volunteer.FirstName.ToLower().Contains(searchText) ||
                        a.Volunteer.LastName.ToLower().Contains(searchText) ||
                        a.Project.ProjectName.ToLower().Contains(searchText)
                    ).ToList();

                    AssignmentsCollectionView.ItemsSource = filteredAssignments;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
            }
        }

        private async void OnAddAssignmentClicked(object sender, EventArgs e)
        {
            // Navigate to add assignment page
            await Navigation.PushAsync(new AssignmentDetailPage(_dbContext, null));
        }

        private async void OnEditAssignmentClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var assignment = button?.CommandParameter as VolunteerAssignment;

                if (assignment != null)
                {
                    // Navigate to edit assignment page
                    await Navigation.PushAsync(new AssignmentDetailPage(_dbContext, assignment));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to edit assignment: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteAssignmentClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var assignment = button?.CommandParameter as VolunteerAssignment;

                if (assignment != null)
                {
                    // Confirm deletion
                    bool confirm = await DisplayAlert(
                        "Confirm Delete",
                        $"Remove {assignment.Volunteer.FirstName} {assignment.Volunteer.LastName} from {assignment.Project.ProjectName}?",
                        "Yes",
                        "No");

                    if (confirm)
                    {
                        // Delete from database
                        _dbContext.VolunteerAssignments.Remove(assignment);
                        await _dbContext.SaveChangesAsync();

                        await DisplayAlert("Success", "Assignment deleted successfully!", "OK");

                        // Reload the list
                        await LoadAssignments();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete assignment: {ex.Message}", "OK");
            }
        }

        private async void OnAddHoursClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var assignment = button?.CommandParameter as VolunteerAssignment;

                if (assignment != null)
                {
                    // Prompt for hours to add
                    string result = await DisplayPromptAsync(
                        "Add Hours",
                        $"Current hours: {assignment.HoursContributed}\nEnter hours to add:",
                        placeholder: "0",
                        keyboard: Keyboard.Numeric);

                    if (!string.IsNullOrWhiteSpace(result) && int.TryParse(result, out int hoursToAdd) && hoursToAdd > 0)
                    {
                        assignment.HoursContributed += hoursToAdd;
                        await _dbContext.SaveChangesAsync();

                        await DisplayAlert("Success", $"Added {hoursToAdd} hours!", "OK");

                        // Reload the list
                        await LoadAssignments();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add hours: {ex.Message}", "OK");
            }
        }
    }
}
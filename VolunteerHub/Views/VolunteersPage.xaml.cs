using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class VolunteersPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private List<Volunteer> _allVolunteers;

        public VolunteersPage(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadVolunteers();
        }

        private async Task LoadVolunteers()
        {
            try
            {
                // Load all volunteers from database
                _allVolunteers = await _dbContext.Volunteers
                    .OrderBy(v => v.FirstName)
                    .ToListAsync();

                // Display in CollectionView
                VolunteersCollectionView.ItemsSource = _allVolunteers;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load volunteers: {ex.Message}", "OK");
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
                    // Show all volunteers if search is empty
                    VolunteersCollectionView.ItemsSource = _allVolunteers;
                }
                else
                {
                    // Filter volunteers by name, email, or skills
                    var filteredVolunteers = _allVolunteers.Where(v =>
                        v.FirstName.ToLower().Contains(searchText) ||
                        v.LastName.ToLower().Contains(searchText) ||
                        v.Email.ToLower().Contains(searchText) ||
                        (v.Skills != null && v.Skills.ToLower().Contains(searchText))
                    ).ToList();

                    VolunteersCollectionView.ItemsSource = filteredVolunteers;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
            }
        }

        private async void OnAddVolunteerClicked(object sender, EventArgs e)
        {
            // Navigate to add volunteer page
            await Navigation.PushAsync(new VolunteerDetailPage(_dbContext, null));
        }

        private async void OnEditVolunteerClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var volunteer = button?.CommandParameter as Volunteer;

                if (volunteer != null)
                {
                    // Navigate to edit volunteer page
                    await Navigation.PushAsync(new VolunteerDetailPage(_dbContext, volunteer));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to edit volunteer: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteVolunteerClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var volunteer = button?.CommandParameter as Volunteer;

                if (volunteer != null)
                {
                    // Confirm deletion
                    bool confirm = await DisplayAlert(
                        "Confirm Delete",
                        $"Are you sure you want to delete {volunteer.FirstName} {volunteer.LastName}?",
                        "Yes",
                        "No");

                    if (confirm)
                    {
                        // Delete from database
                        _dbContext.Volunteers.Remove(volunteer);
                        await _dbContext.SaveChangesAsync();

                        await DisplayAlert("Success", "Volunteer deleted successfully!", "OK");

                        // Reload the list
                        await LoadVolunteers();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete volunteer: {ex.Message}", "OK");
            }
        }

        private async void OnViewDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var volunteer = button?.CommandParameter as Volunteer;

                if (volunteer != null)
                {
                    // Show detailed information
                    string details = $"Name: {volunteer.FirstName} {volunteer.LastName}\n" +
                                   $"Email: {volunteer.Email}\n" +
                                   $"Phone: {volunteer.Phone}\n" +
                                   $"Skills: {volunteer.Skills}\n" +
                                   $"Availability: {volunteer.Availability}\n" +
                                   $"Join Date: {volunteer.JoinDate:yyyy-MM-dd}\n" +
                                   $"Status: {volunteer.Status}";

                    await DisplayAlert("Volunteer Details", details, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to show details: {ex.Message}", "OK");
            }
        }
    }
}
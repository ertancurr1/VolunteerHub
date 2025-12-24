using VolunteerHub.Data;
using VolunteerHub.Models;

namespace VolunteerHub.Views
{
    public partial class VolunteerDetailPage : ContentPage
    {
        private readonly AppDbContext _dbContext;
        private readonly Volunteer _volunteer;
        private readonly bool _isEditMode;

        public VolunteerDetailPage(AppDbContext dbContext, Volunteer volunteer)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _volunteer = volunteer;
            _isEditMode = volunteer != null;

            LoadVolunteerData();
        }

        private void LoadVolunteerData()
        {
            if (_isEditMode)
            {
                // Edit mode - populate fields with existing data
                HeaderLabel.Text = "Edit Volunteer";
                FirstNameEntry.Text = _volunteer.FirstName;
                LastNameEntry.Text = _volunteer.LastName;
                EmailEntry.Text = _volunteer.Email;
                PhoneEntry.Text = _volunteer.Phone;
                SkillsEntry.Text = _volunteer.Skills;
                AvailabilityEntry.Text = _volunteer.Availability;
                JoinDatePicker.Date = _volunteer.JoinDate;
                StatusPicker.SelectedItem = _volunteer.Status;
            }
            else
            {
                // Add mode - set defaults
                HeaderLabel.Text = "Add New Volunteer";
                JoinDatePicker.Date = DateTime.Now;
                StatusPicker.SelectedIndex = 0; // Default to "Active"
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
                {
                    await DisplayAlert("Validation Error", "First name is required", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
                {
                    await DisplayAlert("Validation Error", "Last name is required", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmailEntry.Text))
                {
                    await DisplayAlert("Validation Error", "Email is required", "OK");
                    return;
                }

                // Basic email validation
                if (!EmailEntry.Text.Contains("@"))
                {
                    await DisplayAlert("Validation Error", "Please enter a valid email address", "OK");
                    return;
                }

                if (StatusPicker.SelectedItem == null)
                {
                    await DisplayAlert("Validation Error", "Status is required", "OK");
                    return;
                }

                if (_isEditMode)
                {
                    // Update existing volunteer
                    _volunteer.FirstName = FirstNameEntry.Text.Trim();
                    _volunteer.LastName = LastNameEntry.Text.Trim();
                    _volunteer.Email = EmailEntry.Text.Trim();
                    _volunteer.Phone = PhoneEntry.Text?.Trim();
                    _volunteer.Skills = SkillsEntry.Text?.Trim();
                    _volunteer.Availability = AvailabilityEntry.Text?.Trim();
                    _volunteer.JoinDate = JoinDatePicker.Date;
                    _volunteer.Status = StatusPicker.SelectedItem.ToString();
                    _volunteer.LastModified = DateTime.Now;

                    await DisplayAlert("Success", "Volunteer updated successfully!", "OK");
                }
                else
                {
                    // Create new volunteer
                    var newVolunteer = new Volunteer
                    {
                        FirstName = FirstNameEntry.Text.Trim(),
                        LastName = LastNameEntry.Text.Trim(),
                        Email = EmailEntry.Text.Trim(),
                        Phone = PhoneEntry.Text?.Trim(),
                        Skills = SkillsEntry.Text?.Trim(),
                        Availability = AvailabilityEntry.Text?.Trim(),
                        JoinDate = JoinDatePicker.Date,
                        Status = StatusPicker.SelectedItem.ToString(),
                        CreatedDate = DateTime.Now,
                        LastModified = DateTime.Now
                    };

                    _dbContext.Volunteers.Add(newVolunteer);
                    await DisplayAlert("Success", "Volunteer added successfully!", "OK");
                }

                // Save to database
                await _dbContext.SaveChangesAsync();

                // Go back to previous page
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save volunteer: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Go back without saving
            await Navigation.PopAsync();
        }
    }
}
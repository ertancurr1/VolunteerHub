using Microsoft.EntityFrameworkCore;
using VolunteerHub.Data;

namespace VolunteerHub
{
    public partial class App : Application
    {
        private readonly AppDbContext _dbContext;

        public App(AppDbContext dbContext)
        {
            InitializeComponent();

            _dbContext = dbContext;

            // Ensure database is created
            InitializeDatabase();

            MainPage = new AppShell();
        }

        private async void InitializeDatabase()
        {
            try
            {
                // Uncomment the line below in case of database reset needed
                // await _dbContext.Database.EnsureDeletedAsync();

                // Create database and apply migrations
                await _dbContext.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                // Log or handle error
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}
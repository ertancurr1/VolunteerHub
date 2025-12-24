using Microsoft.Extensions.Logging;
using VolunteerHub.Data;
using Microsoft.EntityFrameworkCore;

namespace VolunteerHub
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Database
            builder.Services.AddDbContext<AppDbContext>();

            // Register Pages
            builder.Services.AddTransient<Views.DashboardPage>();
            builder.Services.AddTransient<Views.VolunteersPage>();
            builder.Services.AddTransient<Views.ProjectsPage>();
            builder.Services.AddTransient<Views.AssignmentsPage>();
            builder.Services.AddTransient<Views.ExportPage>();
            builder.Services.AddTransient<Views.AboutPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

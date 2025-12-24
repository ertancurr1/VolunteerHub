using System.Globalization;

namespace VolunteerHub.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "active" => Color.FromArgb("#27AE60"),      // Professional Green
                    "inactive" => Color.FromArgb("#95A5A6"),    // Gray
                    "planning" => Color.FromArgb("#F39C12"),    // Amber
                    "completed" => Color.FromArgb("#27AE60"),   // Professional Green
                    "cancelled" => Color.FromArgb("#E74C3C"),   // Red
                    _ => Color.FromArgb("#95A5A6")              // Default Gray
                };
            }
            return Color.FromArgb("#95A5A6");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
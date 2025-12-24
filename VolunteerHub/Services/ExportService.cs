using System.Text;
using System.Text.Json;
using VolunteerHub.Models;

namespace VolunteerHub.Services
{
    public static class ExportService
    {
        public static string ExportVolunteersToCsv(List<Volunteer> volunteers)
        {
            var csv = new StringBuilder();
            csv.AppendLine("ID,First Name,Last Name,Email,Phone,Skills,Availability,Join Date,Status");

            foreach (var v in volunteers)
            {
                csv.AppendLine($"{v.VolunteerId},{v.FirstName},{v.LastName},{v.Email},{v.Phone},{v.Skills},{v.Availability},{v.JoinDate:yyyy-MM-dd},{v.Status}");
            }

            return csv.ToString();
        }

        public static string ExportProjectsToCsv(List<Project> projects)
        {
            var csv = new StringBuilder();
            csv.AppendLine("ID,Project Name,Description,Start Date,End Date,Status,Required Volunteers");

            foreach (var p in projects)
            {
                var endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString("yyyy-MM-dd") : "Ongoing";
                var desc = p.Description?.Replace(",", ";") ?? "";
                csv.AppendLine($"{p.ProjectId},{p.ProjectName},{desc},{p.StartDate:yyyy-MM-dd},{endDate},{p.Status},{p.RequiredVolunteers}");
            }

            return csv.ToString();
        }

        public static string ExportAssignmentsToCsv(List<VolunteerAssignment> assignments)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Assignment ID,Volunteer Name,Project Name,Assignment Date,Hours Contributed,Notes");

            foreach (var a in assignments)
            {
                var notes = a.Notes?.Replace(",", ";") ?? "";
                csv.AppendLine($"{a.AssignmentId},{a.Volunteer.FirstName} {a.Volunteer.LastName},{a.Project.ProjectName},{a.AssignmentDate:yyyy-MM-dd},{a.HoursContributed},{notes}");
            }

            return csv.ToString();
        }

        public static string ExportToJson<T>(List<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(data, options);
        }
    }
}
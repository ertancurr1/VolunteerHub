using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerHub.Models
{
    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int VolunteerId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        public int HoursContributed { get; set; } = 0;

        [StringLength(200)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("VolunteerId")]
        public Volunteer Volunteer { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
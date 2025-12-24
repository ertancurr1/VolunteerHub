using System.ComponentModel.DataAnnotations;

namespace VolunteerHub.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Planning";

        public int RequiredVolunteers { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<VolunteerAssignment> Assignments { get; set; }
    }
}
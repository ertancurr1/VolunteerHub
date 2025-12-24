using System.ComponentModel.DataAnnotations;

namespace VolunteerHub.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Skills { get; set; }

        [StringLength(100)]
        public string Availability { get; set; }

        public DateTime JoinDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<VolunteerAssignment> Assignments { get; set; }
    }
}
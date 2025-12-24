using Microsoft.EntityFrameworkCore;
using VolunteerHub.Models;

namespace VolunteerHub.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "volunteerhub.db");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Volunteer)
                .WithMany(v => v.Assignments)
                .HasForeignKey(va => va.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(va => va.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed some initial data
            modelBuilder.Entity<Volunteer>().HasData(
                new Volunteer
                {
                    VolunteerId = 1,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@email.com",
                    Phone = "555-0101",
                    Skills = "Project Management, Leadership, Fundraising",
                    Availability = "Weekdays",
                    JoinDate = new DateTime(2023, 6, 15),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 2,
                    FirstName = "Michael",
                    LastName = "Chen",
                    Email = "michael.chen@email.com",
                    Phone = "555-0102",
                    Skills = "Marketing, Social Media, Content Creation",
                    Availability = "Evenings",
                    JoinDate = new DateTime(2023, 8, 20),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 3,
                    FirstName = "Emily",
                    LastName = "Rodriguez",
                    Email = "emily.rodriguez@email.com",
                    Phone = "555-0103",
                    Skills = "Teaching, Mentoring, Curriculum Development",
                    Availability = "Weekends",
                    JoinDate = new DateTime(2024, 1, 10),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 4,
                    FirstName = "David",
                    LastName = "Thompson",
                    Email = "david.thompson@email.com",
                    Phone = "555-0104",
                    Skills = "Event Planning, Logistics, Coordination",
                    Availability = "Weekends",
                    JoinDate = new DateTime(2024, 3, 5),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 5,
                    FirstName = "Lisa",
                    LastName = "Anderson",
                    Email = "lisa.anderson@email.com",
                    Phone = "555-0105",
                    Skills = "Healthcare, Nursing, First Aid",
                    Availability = "Weekdays",
                    JoinDate = new DateTime(2024, 5, 12),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 6,
                    FirstName = "James",
                    LastName = "Williams",
                    Email = "james.williams@email.com",
                    Phone = "555-0106",
                    Skills = "Construction, Carpentry, Repairs",
                    Availability = "Weekends",
                    JoinDate = new DateTime(2023, 4, 18),
                    Status = "Inactive",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 7,
                    FirstName = "Maria",
                    LastName = "Garcia",
                    Email = "maria.garcia@email.com",
                    Phone = "555-0107",
                    Skills = "Translation, Spanish, Community Outreach",
                    Availability = "Evenings",
                    JoinDate = new DateTime(2024, 7, 22),
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                new Volunteer
                {
                    VolunteerId = 8,
                    FirstName = "Robert",
                    LastName = "Brown",
                    Email = "robert.brown@email.com",
                    Phone = "555-0108",
                    Skills = "Finance, Accounting, Budgeting",
                    Availability = "Weekdays",
                    JoinDate = new DateTime(2023, 11, 8),
                    Status = "Inactive",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                }
            );

            modelBuilder.Entity<Project>().HasData(
                // Completed Project
                new Project
                {
                    ProjectId = 1,
                    ProjectName = "Winter Clothing Drive 2024",
                    Description = "Collection and distribution of winter clothing to homeless shelters across the city",
                    StartDate = new DateTime(2024, 10, 1),
                    EndDate = new DateTime(2024, 12, 15),
                    Status = "Completed",
                    RequiredVolunteers = 8,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Active Project
                new Project
                {
                    ProjectId = 2,
                    ProjectName = "Community Food Bank Operations",
                    Description = "Daily operations including sorting, packing, and distributing food to families in need",
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2025, 3, 31),
                    Status = "Active",
                    RequiredVolunteers = 12,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Active Project
                new Project
                {
                    ProjectId = 3,
                    ProjectName = "Youth Mentorship Program",
                    Description = "One-on-one mentoring for at-risk youth focusing on education and career development",
                    StartDate = new DateTime(2024, 11, 15),
                    EndDate = new DateTime(2025, 6, 30),
                    Status = "Active",
                    RequiredVolunteers = 15,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Planning Project
                new Project
                {
                    ProjectId = 4,
                    ProjectName = "Spring Community Garden Initiative",
                    Description = "Establishing community gardens in underserved neighborhoods to promote healthy eating",
                    StartDate = new DateTime(2025, 3, 1),
                    EndDate = new DateTime(2025, 9, 30),
                    Status = "Planning",
                    RequiredVolunteers = 20,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Cancelled Project
                new Project
                {
                    ProjectId = 5,
                    ProjectName = "Summer Sports Camp",
                    Description = "Free sports camp for children - cancelled due to facility unavailability",
                    StartDate = new DateTime(2024, 6, 15),
                    EndDate = new DateTime(2024, 8, 15),
                    Status = "Cancelled",
                    RequiredVolunteers = 10,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Completed Project
                new Project
                {
                    ProjectId = 6,
                    ProjectName = "Thanksgiving Meal Service 2024",
                    Description = "Preparation and serving of Thanksgiving meals to community members",
                    StartDate = new DateTime(2024, 11, 20),
                    EndDate = new DateTime(2024, 11, 28),
                    Status = "Completed",
                    RequiredVolunteers = 25,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                },
                // Planning Project
                new Project
                {
                    ProjectId = 7,
                    ProjectName = "Digital Literacy Program",
                    Description = "Teaching basic computer and internet skills to seniors",
                    StartDate = new DateTime(2025, 2, 1),
                    Status = "Planning",
                    RequiredVolunteers = 8,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                }
            );

            // Seed Assignments
            modelBuilder.Entity<VolunteerAssignment>().HasData(
                new VolunteerAssignment
                {
                    AssignmentId = 1,
                    VolunteerId = 1,
                    ProjectId = 2,
                    AssignmentDate = new DateTime(2024, 9, 15),
                    HoursContributed = 45,
                    Notes = "Team leader for food distribution"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 2,
                    VolunteerId = 2,
                    ProjectId = 2,
                    AssignmentDate = new DateTime(2024, 9, 20),
                    HoursContributed = 32,
                    Notes = "Handles social media and outreach"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 3,
                    VolunteerId = 3,
                    ProjectId = 3,
                    AssignmentDate = new DateTime(2024, 11, 20),
                    HoursContributed = 28,
                    Notes = "Mentoring two high school students"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 4,
                    VolunteerId = 4,
                    ProjectId = 1,
                    AssignmentDate = new DateTime(2024, 10, 5),
                    HoursContributed = 52,
                    Notes = "Coordinated collection events"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 5,
                    VolunteerId = 5,
                    ProjectId = 6,
                    AssignmentDate = new DateTime(2024, 11, 22),
                    HoursContributed = 18,
                    Notes = "Health screening and first aid services"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 6,
                    VolunteerId = 7,
                    ProjectId = 2,
                    AssignmentDate = new DateTime(2024, 10, 10),
                    HoursContributed = 25,
                    Notes = "Translation services for Spanish-speaking families"
                },
                new VolunteerAssignment
                {
                    AssignmentId = 7,
                    VolunteerId = 1,
                    ProjectId = 6,
                    AssignmentDate = new DateTime(2024, 11, 21),
                    HoursContributed = 15,
                    Notes = "Logistics coordination"
                }
            );
        }
    }
}
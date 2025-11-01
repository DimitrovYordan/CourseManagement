using Microsoft.EntityFrameworkCore;

using CourseManagement.Entities;
using CourseManagement.Models;
using CourseManagement.Enums;

namespace CourseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.EnrolledAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "C# Fundamentals",
                    Description = "Learn the basics of C# programming language.",
                    Capacity = 20,
                    SeatsAvailable = 20,
                    Type = CourseType.Online
                },
                new Course
                {
                    Id = 2,
                    Title = "ASP.NET Core Web API",
                    Description = "Build RESTful APIs with .NET 8 and Entity Framework.",
                    Capacity = 25,
                    SeatsAvailable = 25,
                    Type = CourseType.Classroom
                },
                new Course
                {
                    Id = 3,
                    Title = "Angular for Beginners",
                    Description = "Introductory course to Angular and TypeScript.",
                    Capacity = 30,
                    SeatsAvailable = 30,
                    Type = CourseType.Online
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FullName = "Ivan Petrov",
                    Email = "ivan.petrov@example.com"
                },
                new Student
                {
                    Id = 2,
                    FullName = "Maria Dimitrova",
                    Email = "maria.dimitrova@example.com"
                },
                new Student
                {
                    Id = 3,
                    FullName = "Georgi Stoyanov",
                    Email = "georgi.stoyanov@example.com"
                }
            );

            modelBuilder.Entity<StudentCourse>().HasData(
                new StudentCourse
                {
                    StudentId = 1,
                    CourseId = 1
                },
                new StudentCourse
                {
                    StudentId = 2,
                    CourseId = 2
                },
                new StudentCourse
                {
                    StudentId = 3,
                    CourseId = 3
                },
                new StudentCourse
                {
                    StudentId = 1,
                    CourseId = 2
                }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    Id = 1,
                    StudentId = 1,
                    CourseId = 1,
                    Progress = 60
                },
                new Enrollment
                {
                    Id = 2,
                    StudentId = 2,
                    CourseId = 2,
                    Progress = 80
                },
                new Enrollment
                {
                    Id = 3,
                    StudentId = 3,
                    CourseId = 3,
                    Progress = 40
                }
            );
        }
    }
}

using CourseManagement.Enums;
using CourseManagement.Models;

namespace CourseManagement.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Capacity { get; set; }

        public int EnrolledCount { get; set; }

        public int SeatsAvailable { get; set; }

        public CourseType Type { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        public bool IsAvailable => EnrolledCount < Capacity;
    }
}

using CourseManagement.Enums;

namespace CourseManagement.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Capacity { get; set; }

        public CourseType Type { get; set; }
    }
}

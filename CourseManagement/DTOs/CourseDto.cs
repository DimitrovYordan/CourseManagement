using CourseManagement.Enums;

namespace CourseManagement.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Capacity { get; set; }

        public int SeatsAvailable { get; set; }

        public CourseType Type { get; set; }
    }
}

using CourseManagement.Enums;

namespace CourseManagement.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Enrolled;

        public int Progress { get; set; } = 0;

        public DateTime EnrolledAt { get; set; }
    }
}

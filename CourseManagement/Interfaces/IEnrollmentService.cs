using CourseManagement.DTOs;

namespace CourseManagement.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDto> EnrollAsync(CreateEnrollmentDto dto);
        
        Task<EnrollmentDto?> UpdateProgressAsync(int id, int progress);

        Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId);

        Task<string> EnrollStudentAsync(EnrollDto dto);

        Task<string> CompleteCourseAsync(EnrollDto dto);
    }
}

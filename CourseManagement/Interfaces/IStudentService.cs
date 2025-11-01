using CourseManagement.DTOs;

namespace CourseManagement.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();

        Task<StudentDto?> GetStudentByIdAsync(int id);

        Task<StudentDto> CreateStudentAsync(CreateStudentDto dto);

        Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto dto);

        Task DeleteStudentAsync(int id);
    }
}

using CourseManagement.DTOs;

namespace CourseManagement.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();

        Task<CourseDto?> GetCourseByIdAsync(int id);

        Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

        Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto dto);

        Task DeleteCourseAsync(int id);
    }
}

using Microsoft.AspNetCore.Mvc;

using CourseManagement.DTOs;
using CourseManagement.Interfaces;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllCoursesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseAsync(int id)
        {
            var course = await _service.GetCourseByIdAsync(id);

            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync(CreateCourseDto dto)
        {
            return Ok(await _service.CreateCourseAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseAsync(int id, UpdateCourseDto dto)
        {
            var result = await _service.UpdateCourseAsync(id, dto);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            await _service.DeleteCourseAsync(id);

            return Ok("Course deleted successfully.");
        }
    }
}

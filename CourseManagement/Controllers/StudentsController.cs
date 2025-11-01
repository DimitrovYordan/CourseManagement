using Microsoft.AspNetCore.Mvc;

using CourseManagement.DTOs;
using CourseManagement.Interfaces;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            return Ok(await _service.GetAllStudentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentAsync(int id)
        {
            var student = await _service.GetStudentByIdAsync(id);

            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentAsync(CreateStudentDto dto)
        {
            return Ok(await _service.CreateStudentAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var result = await _service.UpdateStudentAsync(id, dto);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAsync(int id)
        {
            await _service.DeleteStudentAsync(id);

            return Ok("Student deleted successfully.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

using CourseManagement.DTOs;
using CourseManagement.Interfaces;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        public EnrollmentsController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(CreateEnrollmentDto dto)
        {
            var result = await _service.EnrollAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}/progress")]
        public async Task<IActionResult> UpdateProgress(int id, UpdateProgressDto dto)
        {
            var result = await _service.UpdateProgressAsync(id, dto.Progress);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            return Ok(await _service.GetByStudentAsync(studentId));
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollDto dto)
        {
            var result = await _service.EnrollStudentAsync(dto);

            return Ok(result);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteCourse([FromBody] EnrollDto dto)
        {
            var result = await _service.CompleteCourseAsync(dto);

            return Ok(result);
        }
    }
}

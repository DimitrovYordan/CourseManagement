using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.DTOs;
using CourseManagement.Entities;
using CourseManagement.Interfaces;

using static CourseManagement.Middlewares.CourseException;

namespace CourseManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _db;

        public StudentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _db.Students
                .Select(student => new StudentDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email
                })
                .ToListAsync();

            return students;
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await _db.Students.FindAsync(id);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            return new StudentDto 
            { 
                Id = student.Id, 
                FullName = student.FullName, 
                Email = student.Email 
            };
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto dto)
        {
            var student = new Student { FullName = dto.FullName, Email = dto.Email };

            _db.Students.Add(student);
            await _db.SaveChangesAsync();

            return new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email
            };
        }

        public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = await _db.Students.FindAsync(id)
                ?? throw new NotFoundException("Student not found.");

            if (dto.FullName != null)
            {
                student.FullName = dto.FullName;
            }

            if (dto.Email != null)
            {
                student.Email = dto.Email;
            }

            await _db.SaveChangesAsync();

            return new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email
            };
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _db.Students.FindAsync(id)
                ?? throw new NotFoundException("Student not found.");

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
    }
}

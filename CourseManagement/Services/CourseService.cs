using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.DTOs;
using CourseManagement.Entities;
using CourseManagement.Interfaces;

using static CourseManagement.Middlewares.CourseException;

namespace CourseManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _db;

        public CourseService(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            return await _db.Courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Capacity = c.Capacity,
                SeatsAvailable = c.SeatsAvailable
            }).ToListAsync();
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _db.Courses.FindAsync(id)
                ?? throw new NotFoundException("Course not found.");

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Capacity = course.Capacity,
                SeatsAvailable = course.SeatsAvailable
            };
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Capacity = dto.Capacity,
                SeatsAvailable = dto.Capacity,
                Type = dto.Type
            };

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Capacity = course.Capacity,
                SeatsAvailable = course.SeatsAvailable,
                Type = course.Type
            };
        }

        public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto dto)
        {
            var course = await _db.Courses.FindAsync(id)
                ?? throw new NotFoundException("Course not found.");

            if (dto.Title != null)
            {
                course.Title = dto.Title;
            }

            if (dto.Description != null)
            {
                course.Description = dto.Description;
            }

            if (dto.Capacity.HasValue)
            {
                course.SeatsAvailable += dto.Capacity.Value - course.Capacity;
                course.Capacity = dto.Capacity.Value;
            }

            await _db.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Capacity = course.Capacity,
                SeatsAvailable = course.SeatsAvailable
            };
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _db.Courses.FindAsync(id)
                ?? throw new NotFoundException("Course not found.");

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
        }
    }
}

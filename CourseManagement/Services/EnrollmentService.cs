using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.DTOs;
using CourseManagement.Enums;
using CourseManagement.Interfaces;
using CourseManagement.Models;

using static CourseManagement.Middlewares.CourseException;

namespace CourseManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly ProgressStrategyFactory _strategyFactory;

        public EnrollmentService(ApplicationDbContext db, ProgressStrategyFactory strategyFactory)
        {
            _db = db;
            _strategyFactory = strategyFactory;
        }

        public async Task<EnrollmentDto> EnrollAsync(CreateEnrollmentDto dto)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            var course = await _db.Courses
                .Include(c => c.StudentCourses)
                .FirstOrDefaultAsync(c => c.Id == dto.CourseId);

            var student = await _db.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == dto.StudentId);

            if (course == null)
            {
                throw new NotFoundException("Course not found.");
            }

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            if (!course.IsAvailable)
            {
                throw new ConflictException("Course is full.");
            }

            bool alreadyEnrolled = await _db.StudentCourses
                .AnyAsync(sc => sc.CourseId == dto.CourseId && sc.StudentId == dto.StudentId);

            if (alreadyEnrolled)
            {
                throw new ConflictException("Student already enrolled in this course.");
            }

            var studentCourse = new StudentCourse
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Progress = EnrollmentStatus.Enrolled
            };

            course.EnrolledCount += 1;

            _db.StudentCourses.Add(studentCourse);
            _db.Courses.Update(course);

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return new EnrollmentDto
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Progress = 0,
                Status = EnrollmentStatus.Enrolled.ToString()
            };
        }

        public async Task<EnrollmentDto?> UpdateProgressAsync(int id, int progress)
        {
            var enrollment = await _db.Enrollments
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (enrollment == null)
            {
                return null;
            }

            enrollment.Progress = progress;

            if (progress >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }
            else if (progress > 0)
            {
                enrollment.Status = EnrollmentStatus.InProgress;
            }

            await _db.SaveChangesAsync();

            return new EnrollmentDto
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Status = enrollment.Status.ToString(),
                Progress = enrollment.Progress
            };
        }

        public async Task<IEnumerable<EnrollmentDto>> GetByStudentAsync(int studentId)
        {
            return await _db.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Status = e.Status.ToString(),
                    Progress = e.Progress
                }).ToListAsync();
        }

        public async Task<string> EnrollStudentAsync(EnrollDto dto)
        {
            var student = await _db.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == dto.StudentId)
                ?? throw new NotFoundException("Student not found.");

            var course = await _db.Courses
                .Include(c => c.StudentCourses)
                .FirstOrDefaultAsync(c => c.Id == dto.CourseId)
                ?? throw new NotFoundException("Course not found.");

            if (!course.IsAvailable)
            {
                throw new ConflictException("Course is full.");
            }

            bool alreadyEnrolled = student.StudentCourses.Any(sc => sc.CourseId == dto.CourseId);
            if (alreadyEnrolled)
            {
                throw new ConflictException("Student already enrolled in this course.");
            }

            var enrollment = new StudentCourse
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Progress = EnrollmentStatus.Enrolled
            };

            course.EnrolledCount++;

            _db.StudentCourses.Add(enrollment);
            await _db.SaveChangesAsync();

            return $"Student '{student.FullName}' successfully enrolled in '{course.Title}'.";
        }

        public async Task<string> CompleteCourseAsync(EnrollDto dto)
        {
            var enrollment = await _db.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .FirstOrDefaultAsync(sc => sc.StudentId == dto.StudentId && sc.CourseId == dto.CourseId)
                ?? throw new NotFoundException("Enrollment not found.");

            if (enrollment.Progress == EnrollmentStatus.Completed)
            {
                throw new ConflictException("Course already completed.");
            }

            var strategy = _strategyFactory.GetStrategy(enrollment.Course.Type);
            var simulatedProgress = strategy.CalculateProgress(0, 100);

            if (simulatedProgress >= 100)
            {
                enrollment.Progress = EnrollmentStatus.Completed;
                enrollment.Course.EnrolledCount = Math.Max(0, enrollment.Course.EnrolledCount - 1);

                await _db.SaveChangesAsync();

                return $"Student '{enrollment.Student.FullName}' completed '{enrollment.Course.Title}'.";
            }

            throw new BadRequestException("Progress calculation failed.");
        }
    }
}

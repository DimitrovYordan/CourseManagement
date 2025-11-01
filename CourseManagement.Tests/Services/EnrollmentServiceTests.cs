using CourseManagement.Data;
using CourseManagement.DTOs;
using CourseManagement.Entities;
using CourseManagement.Enums;
using CourseManagement.Interfaces;
using CourseManagement.Models;
using CourseManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using static CourseManagement.Middlewares.CourseException;

namespace CourseManagement.Tests.Services
{
    public class EnrollmentServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ApplicationDbContext(options);
        }

        private EnrollmentService GetService(ApplicationDbContext context)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(OnlineProgressStrategy)))
                .Returns(new FakeProgressStrategy());

            var strategyFactory = new ProgressStrategyFactory(mockServiceProvider.Object);
            return new EnrollmentService(context, strategyFactory);
        }

        [Fact]
        public async Task EnrollAsync_ShouldEnrollStudentSuccessfully()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            var course = new Course { Id = 1, Title = "C# Basics", Capacity = 10, EnrolledCount = 0 };
            var student = new Student { Id = 1, FullName = "John Doe", Email = "johndoe@example.com" };
            context.Courses.Add(course);
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var dto = new CreateEnrollmentDto { CourseId = 1, StudentId = 1 };

            // Act
            var result = await service.EnrollAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CourseId);
            Assert.Equal(1, result.StudentId);
            Assert.Equal(EnrollmentStatus.Enrolled.ToString(), result.Status);
            Assert.Equal(1, course.EnrolledCount);
        }

        [Fact]
        public async Task EnrollAsync_ShouldThrow_WhenCourseNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            var student = new Student { Id = 1, FullName = "Jane Doe", Email = "janedoe@example.com" };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var dto = new CreateEnrollmentDto { CourseId = 99, StudentId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.EnrollAsync(dto));
        }

        [Fact]
        public async Task EnrollStudentAsync_ShouldReturnSuccessMessage()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            var course = new Course { Id = 1, Title = "Angular", Capacity = 10 };
            var student = new Student { Id = 1, FullName = "Maria Petrova", Email = "mariapetrova@example.com" };
            context.Courses.Add(course);
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var dto = new EnrollDto { CourseId = 1, StudentId = 1 };

            // Act
            var message = await service.EnrollStudentAsync(dto);

            // Assert
            Assert.NotNull(message);
            Assert.Contains("enrolled", message, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(1, course.EnrolledCount);
        }

        [Fact]
        public async Task UpdateProgressAsync_ShouldMarkAsCompleted_WhenProgressIs100()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            var student = new Student
            {
                Id = 1,
                FullName = "John Doe",
                Email = "john@example.com"
            };

            var course = new Course
            {
                Id = 1,
                Title = "C# Fundamentals",
                Capacity = 10,
                EnrolledCount = 0
            };

            context.Students.Add(student);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var enrollment = new Enrollment
            {
                Id = 1,
                StudentId = student.Id,
                Student = student,
                CourseId = course.Id,
                Course = course,
                Progress = 0,
                Status = EnrollmentStatus.Enrolled
            };

            context.Enrollments.Add(enrollment);
            await context.SaveChangesAsync();

            // Act
            var result = await service.UpdateProgressAsync(enrollment.Id, 100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result!.Progress);
            Assert.Equal(EnrollmentStatus.Completed.ToString(), result.Status);
        }

        [Fact]
        public async Task GetByStudentAsync_ShouldReturnEnrollments()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            context.Enrollments.AddRange(
                new Enrollment { Id = 1, StudentId = 5, CourseId = 3, Progress = 80, Status = EnrollmentStatus.InProgress },
                new Enrollment { Id = 2, StudentId = 5, CourseId = 4, Progress = 100, Status = EnrollmentStatus.Completed }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetByStudentAsync(5);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Status == EnrollmentStatus.Completed.ToString());
        }

        [Fact]
        public async Task CompleteCourseAsync_ShouldMarkCourseAsCompleted()
        {
            // Arrange
            var context = GetDbContext();
            var service = GetService(context);

            var course = new Course { Id = 1, Title = "ASP.NET Core", Type = CourseType.Online, Capacity = 5, EnrolledCount = 1 };
            var student = new Student { Id = 1, FullName = "Ivan Petrov", Email = "ivan.petrov@example.com" };
            var enrollment = new StudentCourse
            {
                StudentId = 1,
                CourseId = 1,
                Progress = EnrollmentStatus.Enrolled
            };

            context.Courses.Add(course);
            context.Students.Add(student);
            context.StudentCourses.Add(enrollment);
            await context.SaveChangesAsync();

            var dto = new EnrollDto { StudentId = 1, CourseId = 1 };

            // Act
            var result = await service.CompleteCourseAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("completed", result, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(EnrollmentStatus.Completed, enrollment.Progress);
        }

        // === FAKE STRATEGY ===
        private class FakeProgressStrategy : IProgressStrategy
        {
            public int CalculateProgress(int currentProgress, int increment) => 100;
        }
    }
}

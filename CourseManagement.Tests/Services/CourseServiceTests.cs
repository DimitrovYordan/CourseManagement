using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.Entities;
using CourseManagement.Services;
using CourseManagement.DTOs;
using CourseManagement.Enums;

using Xunit;

namespace CourseManagement.Tests.Services
{
    public class CourseServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddCourse_ShouldAddCourseToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CourseService(context);

            var courseDto = new CreateCourseDto
            {
                Title = "C# Fundamentals",
                Description = "Intro to C#",
                Capacity = 10,
                Type = CourseType.Online
            };

            // Act
            var createdCourse = await service.CreateCourseAsync(courseDto);

            // Assert
            var courseInDb = await context.Courses.FirstOrDefaultAsync();

            Assert.NotNull(createdCourse);
            Assert.NotNull(courseInDb);
            Assert.Equal(courseDto.Title, courseInDb.Title);
            Assert.Equal(courseDto.Capacity, courseInDb.Capacity);
        }

        [Fact]
        public async Task GetAllCourses_ShouldReturnAllCourses()
        {
            // Arrange
            var context = GetDbContext();
            context.Courses.Add(new Course { Title = "C# Basics", Capacity = 5 });
            context.Courses.Add(new Course { Title = "Angular Intro", Capacity = 8 });
            await context.SaveChangesAsync();

            // Act
            var service = new CourseService(context);
            var result = await service.GetAllCoursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCourseById_ShouldReturnCorrectCourse()
        {
            // Arrange
            var context = GetDbContext();
            var course = new Course { Title = "SQL 101", Capacity = 15 };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            // Act
            var service = new CourseService(context);
            var result = await service.GetCourseByIdAsync(course.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course.Id, result.Id);
        }
    }
}

using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.DTOs;
using CourseManagement.Entities;
using CourseManagement.Services;

namespace CourseManagement.Tests.Services
{
    public class StudentServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddStudent_ShouldAddStudentToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var service = new StudentService(context);

            var student = new CreateStudentDto 
            { 
                FullName = "John Doe", 
                Email = "john@example.com" 
            };

            // Act
            await service.CreateStudentAsync(student);
            var result = await context.Students.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.FullName);
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnAllStudents()
        {
            // Arrange
            var context = GetDbContext();
            context.Students.Add(new Student { FullName = "Alice", Email = "a@a.com" });
            context.Students.Add(new Student { FullName = "Bob", Email = "b@b.com" });
            await context.SaveChangesAsync();

            // Act
            var service = new StudentService(context);
            var result = await service.GetAllStudentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetStudentById_ShouldReturnCorrectStudent()
        {
            // Arrange
            var context = GetDbContext();
            var student = new Student { FullName = "Charlie", Email = "c@c.com" };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // Act
            var service = new StudentService(context);
            var result = await service.GetStudentByIdAsync(student.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Charlie", result.FullName);
        }
    }
}

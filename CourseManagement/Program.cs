using Microsoft.EntityFrameworkCore;

using CourseManagement.Data;
using CourseManagement.Interfaces;
using CourseManagement.Middlewares;
using CourseManagement.Services;

namespace CourseManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // DB Context
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Services
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            // Strategy pattern injection
            builder.Services.AddScoped<OnlineProgressStrategy>();
            builder.Services.AddScoped<ClassroomProgressStrategy>();
            builder.Services.AddScoped<ProgressStrategyFactory>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware for global exception handling
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

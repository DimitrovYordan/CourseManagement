using CourseManagement.Enums;
using CourseManagement.Interfaces;

namespace CourseManagement.Services
{
    public class ProgressStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProgressStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProgressStrategy GetStrategy(CourseType courseType)
        {
            return courseType switch
            {
                CourseType.Online => (IProgressStrategy)_serviceProvider.GetRequiredService(typeof(OnlineProgressStrategy)),
                CourseType.Classroom => (IProgressStrategy)_serviceProvider.GetRequiredService(typeof(ClassroomProgressStrategy)),
                _ => throw new ArgumentOutOfRangeException(nameof(courseType), "Unsupported course type")
            };
        }
    }
}

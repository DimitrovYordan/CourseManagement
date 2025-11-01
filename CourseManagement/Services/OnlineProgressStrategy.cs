using CourseManagement.Interfaces;

namespace CourseManagement.Services
{
    public class OnlineProgressStrategy : IProgressStrategy
    {
        public int CalculateProgress(int currentProgress, int increment)
        {
            return Math.Min(100, currentProgress + increment);
        }
    }
}

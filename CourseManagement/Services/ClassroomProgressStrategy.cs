using CourseManagement.Interfaces;

namespace CourseManagement.Services
{
    public class ClassroomProgressStrategy : IProgressStrategy
    {
        public int CalculateProgress(int currentProgress, int increment)
        {
            int next = currentProgress + (int)Math.Ceiling(increment * 0.8);

            return Math.Min(100, next);
        }
    }
}

namespace CourseManagement.Interfaces
{
    public interface IProgressStrategy
    {
        int CalculateProgress(int currentProgress, int increment);
    }
}

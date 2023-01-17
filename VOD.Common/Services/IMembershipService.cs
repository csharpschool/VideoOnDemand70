namespace VOD.Common.Services
{
    public interface IMembershipService
    {
        Task<CourseDTO> GetCourseAsync(int? id);
        Task<List<CourseDTO>> GetCoursesAsync();
        Task<VideoDTO> GetVideoAsync(int? id);
    }
}
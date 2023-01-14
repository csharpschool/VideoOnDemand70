using System.ComponentModel.DataAnnotations;

namespace VOD.Application.Common.DTOs;

public class VideoDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public string Thumbnail { get; set; }
    public string Url { get; set; }

    public int SectionId { get; set; }
    public int CourseId { get; set; }
    public string Course { get; set; }
    public string Section { get; set; }

}

namespace VOD.Application.Common.DTOs;
public class CourseDTO
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string MarqueeImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Free { get; set; }

    public int InstructorId { get; set; }
    public InstructorDTO Instructor { get; set; } = new();
    public List<SectionDTO> Sections { get; set; } = new();
}


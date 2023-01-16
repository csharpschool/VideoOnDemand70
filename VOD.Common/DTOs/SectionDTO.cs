namespace VOD.Common.DTOs;
public class SectionDTO
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int CourseId { get; set; }
    public string Course { get; set; }
    public List<VideoDTO> Videos { get; set; }
}

public class SectionCreateDTO
{
    public string Title { get; set; }

    public int CourseId { get; set; }
}

public class SectionEditDTO : SectionCreateDTO
{
    public int Id { get; set; }
}

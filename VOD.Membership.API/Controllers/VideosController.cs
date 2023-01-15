namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly IDbService _db;

    public VideosController(IDbService db) => _db = db;

    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            List<VideoDTO>? videos = await _db.GetAsync<Video, VideoDTO>();

            return Results.Ok(videos);
        }
        catch
        {
        }

        return Results.NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IResult> Get(int id)
    {
        try
        {
            var video = await _db.SingleAsync<Video, VideoDTO>(c => c.Id.Equals(id));
            if(video is null) return Results.NotFound();
            var section = await _db.SingleAsync<Section, SectionDTO>(c => c.Id.Equals(video.SectionId));
            if (section is not null)
            {
                video.Section = section.Title;
                var course = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(section.CourseId));
                if (section is not null) 
                {
                    video.Course = course.Title;
                    video.CourseId = course.Id;
                }
            }

            return Results.Ok(video);
        }
        catch
        {
        }
        return Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] VideoCreateDTO dto)
    {
        try
        {
            if (dto == null) return Results.BadRequest();

            var video = await _db.AddAsync<Video, VideoCreateDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

            return Results.Created(_db.GetURI<Video>(video), video);
        }
        catch
        {
        }

        return Results.BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] VideoEditDTO dto)
    {
        try
        {
            if (dto == null) return Results.BadRequest("No entity provided");
            if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

            var exists = await _db.AnyAsync<Section>(i => i.Id.Equals(dto.SectionId));
            if (!exists) return Results.NotFound("Could not find related entity");

            exists = await _db.AnyAsync<Video>(c => c.Id.Equals(id));
            if (!exists) return Results.NotFound("Could not find entity");

            _db.Update<Video, VideoEditDTO>(dto.Id, dto);

            var success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

            return Results.NoContent();
        }
        catch
        {
        }

        return Results.BadRequest("Unable to update the entity");

    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        try
        {
            var success = await _db.DeleteAsync<Video>(id);

            if (!success) return Results.NotFound();

            success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

            return Results.NoContent();
        }
        catch
        {
        }

        return Results.BadRequest();
    }
}

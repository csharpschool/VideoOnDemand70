namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionsController : ControllerBase
{
    private readonly IDbService _db;

    public SectionsController(IDbService db) => _db = db;

    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            _db.Include<Video>();
            List<SectionDTO>? sections = await _db.GetAsync<Section, SectionDTO>();

            return Results.Ok(sections);
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
            _db.Include<Video>();
            var section = await _db.SingleAsync<Section, SectionDTO>(c => c.Id.Equals(id));
            if(section is null) return Results.NotFound();

            var course = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(section.CourseId));
            if (section is not null) section.Course = course.Title;

            return Results.Ok(section);
        }
        catch
        {
        }
        return Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] SectionCreateDTO dto)
    {
        try
        {
            if (dto == null) return Results.BadRequest();

            var section = await _db.AddAsync<Section, SectionCreateDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();

            var sectionDTO = await _db.SingleAsync<Section, SectionDTO>(c => c.Id.Equals(section.Id));
            if (sectionDTO is null) return Results.NotFound();

            var courseDTO = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(section.CourseId));
            if (courseDTO is not null) sectionDTO.Course = courseDTO.Title;

            return Results.Created(_db.GetURI<Section>(section), sectionDTO);
        }
        catch
        {
        }

        return Results.BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] SectionEditDTO dto)
    {
        try
        {
            if (dto == null) return Results.BadRequest("No entity provided");
            if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

            var exists = await _db.AnyAsync<Section>(c => c.Id.Equals(id));
            if (!exists) return Results.NotFound("Could not find entity");

            _db.Update<Section, SectionEditDTO>(dto.Id, dto);

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
            var success = await _db.DeleteAsync<Section>(id);

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

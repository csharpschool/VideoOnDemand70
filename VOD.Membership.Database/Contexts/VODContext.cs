using VOD.Membership.Database.Entities;

namespace VOD.Membership.Database.Contexts;

public class VODContext : DbContext
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Video> Videos => Set<Video>();

    public VODContext(DbContextOptions<VODContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }


}

namespace VOD.Users.Database.Contexts;

public class VODUserContext : IdentityDbContext<VODUser>
{
    public VODUserContext(DbContextOptions<VODUserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}


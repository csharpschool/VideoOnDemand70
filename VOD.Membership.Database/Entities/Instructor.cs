namespace VOD.Membership.Database.Entities;
public class Instructor : IEntity
{
    public int Id { get; set; }
    [MaxLength(80), Required]
    public string Name { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; }
    [MaxLength(1024)]
    public string Avatar { get; set; }

    public virtual ICollection<Course>? Courses { get; set; }
}

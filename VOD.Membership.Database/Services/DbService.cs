namespace VOD.Membership.Database.Services;

public class DbService : IDbService
{
    private readonly VODContext _db;
    private readonly IMapper _mapper;

    public DbService(VODContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<TDto>> GetAsync<TEntity, TDto>()
    where TEntity : class, IEntity
    where TDto : class
    {
        var entities = await _db.Set<TEntity>().ToListAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    public async Task<List<TDto>> GetAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
    {
        var entities = await _db.Set<TEntity>().Where(expression).ToListAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    private async Task<TEntity?> SingleAsync<TEntity>(
    Expression<Func<TEntity, bool>> expression)
    where TEntity : class, IEntity =>
    await _db.Set<TEntity>().SingleOrDefaultAsync(expression);


    public async Task<TDto> SingleAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
    {
        var entity = await SingleAsync(expression);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
    where TEntity : class, IEntity
    where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _db.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public async Task<bool> SaveChangesAsync() =>
        await _db.SaveChangesAsync() >= 0;
    public string GetURI<TEntity>(TEntity entity) where TEntity : class, IEntity
        => $"/{typeof(TEntity).Name.ToLower()}s/{entity.Id}";

    public void Update<TEntity, TDto>(int id, TDto dto)
    where TEntity : class, IEntity
    where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);
        entity.Id = id;
        _db.Set<TEntity>().Update(entity);
    }

    public async Task<bool> AnyAsync<TEntity>(
    Expression<Func<TEntity, bool>> expression)
    where TEntity : class, IEntity =>
        await _db.Set<TEntity>().AnyAsync(expression);

    public async Task<bool> DeleteAsync<TEntity>(int id)
    where TEntity : class, IEntity
    {
        try
        {
            var entity = await SingleAsync<TEntity>(e => e.Id.Equals(id));
            if (entity is null) return false;
            _db.Remove(entity);
        }
        catch (Exception ex)
        {
            throw;
        }

        return true;
    }

    public void Include<TEntity>() where TEntity : class, IEntity
    {
        var propertyNames = _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

        if (propertyNames is null) return;

        foreach (var name in propertyNames)
            _db.Set<TEntity>().Include(name).Load();
    }
}

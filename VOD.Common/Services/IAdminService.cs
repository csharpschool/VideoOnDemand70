namespace VOD.Common.Services;

public interface IAdminService
{
    Task CreateAsync<TDto>(TDto dto);
    Task DeleteAsync<TDto>(int id);
    Task EditAsync<TDto>(TDto dto, int id);
    Task<List<TDto>> GetAsync<TDto>();
    Task<TDto?> GetAsync<TDto>(int id);
}
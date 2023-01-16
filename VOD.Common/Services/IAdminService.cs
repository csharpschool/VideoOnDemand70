namespace VOD.Common.Services;

public interface IAdminService
{
    Task<List<TDto>> Get<TDto>();
}
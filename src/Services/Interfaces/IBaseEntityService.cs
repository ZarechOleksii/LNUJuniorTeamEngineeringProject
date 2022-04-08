using Models;

namespace Services.Interfaces
{
    public interface IBaseEntityService
    {
        public Task<List<BaseEntity>> GetBaseEntitiesAsync();
    }
}

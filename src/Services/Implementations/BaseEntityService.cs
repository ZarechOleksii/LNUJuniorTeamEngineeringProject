using Data;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class BaseEntityService : IBaseEntityService
    {
        private readonly IRepository<BaseEntity> _repository;

        public BaseEntityService(IRepository<BaseEntity> rep)
        {
            _repository = rep;
        }

        public async Task<List<BaseEntity>> GetBaseEntitiesAsync()
        {
            return await _repository.FetchAllNoTracking();
        }
    }
}

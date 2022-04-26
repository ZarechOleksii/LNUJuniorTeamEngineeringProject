using Models.Entities;

namespace Services.Interfaces
{
    public interface IRatingService
    {
        public Task<bool> AddRateAsync(MovieRate comment);
    }
}

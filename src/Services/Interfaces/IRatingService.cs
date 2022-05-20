using Models.Entities;

namespace Services.Interfaces
{
    public interface IRatingService
    {
        public Task<bool> AddRateAsync(MovieRate comment);
        public Task<double> GetRateAsync(Guid movieId);
    }
}

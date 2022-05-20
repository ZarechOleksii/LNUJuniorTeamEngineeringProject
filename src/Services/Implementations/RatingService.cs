using Data.RatingRepository;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IRatingRepository repository, ILogger<RatingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> AddRateAsync(MovieRate movieRate)
        {
            try
            {
                var previousRate = (await _repository.FetchAllNoTracking())
                    .FirstOrDefault(x => x.MovieId == movieRate.MovieId && x.UserId == movieRate.UserId);

                if (previousRate != null)
                {
                    await _repository.DeleteAsync(previousRate);
                }

                return await _repository.AddAsync(movieRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in RatingService method AddRateAsync");
                return false;
            }
        }

        public async Task<double> GetRateAsync(Guid movieId)
        {
            try
            {
                var rateValue = await _repository.GetMovieRateAsync(movieId);
                return Math.Round(rateValue, 2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in RatingService method GetRateAsync");
                return 0.0;
            }
        }
    }
}

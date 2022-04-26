﻿using Data;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRepository<MovieRate> _repository;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IRepository<MovieRate> repository, ILogger<RatingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> AddRateAsync(MovieRate movieRate)
        {
            try
            {
                return await _repository.AddAsync(movieRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in RatingService method AddRateAsync");
                return false;
            }
        }
    }
}

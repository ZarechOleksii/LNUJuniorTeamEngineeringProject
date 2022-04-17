using System;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Xunit;

namespace UnitTests.DataTests
{
    public class BaseRepositoryTest
    {
        private readonly BaseRepository<BaseEntity> _rep;
        private readonly ApplicationContext _context;

        public BaseRepositoryTest()
        {
            var dbName = $"OnlyMovies_{DateTime.Now.ToFileTimeUtc()}";
            DbContextOptions<ApplicationContext> dbContextOptions
                = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            _context = new ApplicationContext(dbContextOptions);
            PopulateData(_context);
            _rep = new BaseRepository<BaseEntity>(_context);
        }

        [Fact]
        public async Task Add_WhenNotPresent_Success()
        {
            // arrange
            var testObject = new BaseEntity() { };

            // act
            var before = (await _rep.FetchAllNoTracking()).Count;
            var result = await _rep.AddAsync(testObject);
            var after = (await _rep.FetchAllNoTracking()).Count;

            // assert
            Assert.True(result);
            Assert.Equal(4, before);
            Assert.Equal(5, after);
        }

        [Fact]
        public async Task Add_WhenPresent_Throws()
        {
            // arrange
            var testObject = new BaseEntity()
            {
                Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a")
            };

            // assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(async ()
                => await _rep.AddAsync(testObject));
        }

        [Fact]
        public async Task Add_WhenTracked_Throws()
        {
            // arrange
            var testObject = await _rep.GetAsync(Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a"));

            // assert
            await Assert
                .ThrowsAsync<ArgumentException>(async ()
                => await _rep.AddAsync(testObject));
        }

        [Fact]
        public async Task Add_WhenRequiredFieldInsertNull_Throws()
        {
            // arrange
            BaseRepository<Movie> rep = new (_context);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var testObject = new Movie()
            {
                Id = Guid.NewGuid(),
                Name = null,
                Description = null,
                Url = null
            };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // assert
            await Assert
                .ThrowsAsync<DbUpdateException>(async ()
                => await rep.AddAsync(testObject));
        }

        [Fact]
        public async Task Remove_WhenPresent_Success()
        {
            // arrange
            var testObject = await _rep.GetAsync(Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a"));

            // act
            var before = (await _rep.FetchAllNoTracking()).Count;
            var result = await _rep.DeleteAsync(testObject);
            var after = (await _rep.FetchAllNoTracking()).Count;

            // assert
            Assert.True(result);
            Assert.Equal(4, before);
            Assert.Equal(3, after);
        }

        [Fact]
        public async Task Remove_WhenNotPresent_Throws()
        {
            // arrange
            var testObject = new BaseEntity() { Id = Guid.NewGuid() };

            // assert
            await Assert
                .ThrowsAsync<DbUpdateConcurrencyException>(async ()
                => await _rep.DeleteAsync(testObject));
        }

        [Fact]
        public async Task Remove_WhenNotTracked_Throws()
        {
            // arrange
            var testObject = new BaseEntity() { Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a") };

            // assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(async ()
                => await _rep.DeleteAsync(testObject));
        }

        [Fact]
        public async Task Get_WhenPresent_Success()
        {
            // act
            var result = await _rep.GetAsync(Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a"));

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_WhenNotPresent_IsNull()
        {
            // act
            var result = await _rep.GetAsync(Guid.NewGuid());

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_WhenTracked_Success()
        {
            // arrange
            BaseRepository<Movie> rep = new (_context);
            var id = Guid.NewGuid();

            var testObject = new Movie()
            {
                Id = id,
                Name = string.Empty,
                Description = string.Empty,
                Url = string.Empty
            };

            // act
            await _rep.AddAsync(testObject);
            testObject.Name = "Test";
            await _rep.SaveChangesAsync();
            var edited = await rep.GetAsync(id);

            // assert
            Assert.NotNull(edited);
            Assert.Equal("Test", edited.Name);
        }

        [Fact]
        public async Task Update_WhenChangingKey_Throws()
        {
            // arrange
            var toChange = await _rep.GetAsync(Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a"));

            // act
            var newId = Guid.NewGuid();
            toChange.Id = newId;

            // assert
            await Assert
                .ThrowsAsync<InvalidOperationException>(async ()
                => await _rep.SaveChangesAsync());
        }

        [Fact]
        public async Task Update_WhenUntracked_NotChanged()
        {
            // arrange
            var toChange = new BaseEntity()
            {
                Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a")
            };

            // act
            var newId = Guid.NewGuid();
            toChange.Id = newId;
            await _rep.SaveChangesAsync();
            var notFound = await _rep.GetAsync(newId);

            // assert
            Assert.Null(notFound);
        }

        private static void PopulateData(ApplicationContext context)
        {
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("adc71289-a52b-4d06-9ffe-ed8d36604f13")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("a81e2adf-d628-45a2-ba9d-e30e1a337432")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("1fda063f-c6a1-4022-b2e3-6e4d43db4d33")
            });
            context.SaveChanges();
        }
    }
}

using Data;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;

namespace IntegrationTests
{
    public class BaseControllerTest
    {
        protected readonly HttpClient client;
        public BaseControllerTest()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        Console.WriteLine(services);
                        var descriptor = services.SingleOrDefault(
                                        d => d.ServiceType ==
                                            typeof(DbContextOptions<ApplicationContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<ApplicationContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<ApplicationContext>();

                            db.Database.EnsureCreated();
                            DbHelper.InitializeDbForTests(db);
                        }
                    });
                });
            client = application.CreateClient();
        }
    }
}
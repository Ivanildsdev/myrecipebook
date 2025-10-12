using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        override protected void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                     {
                          // Additional configuration for test environment can be added here
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<MyRecipeBookDbContext>));
                         if (descriptor != null) services.Remove(descriptor);

                         var provider = services
                             .AddEntityFrameworkInMemoryDatabase()
                             .BuildServiceProvider();

                            services.AddDbContext<MyRecipeBookDbContext>(options => 
                            {
                                options.UseInMemoryDatabase("InMemoryDbForTesting");
                                options.UseInternalServiceProvider(provider);
                            });
                     });
        }
    }
}

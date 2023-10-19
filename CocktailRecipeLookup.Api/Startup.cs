using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailRecipeLookup.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // This is the default port for Create React App
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            // ... other configurations
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ... other middleware
            app.UseCors();
            // ... other middleware
        }
    }
}

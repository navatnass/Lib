using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using LibraryReservationSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<LibraryContext>(options =>
                options.UseInMemoryDatabase("LibraryDB"));

            var app = builder.Build();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
                SeedDatabase(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers to endpoints
            app.MapControllers();

            // Run the app
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Set the application to listen on specific URLs
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000;https://localhost:5001"); // Specify your ports here
                });
        private static void SeedDatabase(LibraryContext context)
        {
            if (!context.Books.Any())  // Check if the database is already seeded
            {
                context.Books.AddRange(new List<Book>
                {
                    new Book { Name = "Book 1", Year = "2020", Type = "Book", PictureUrl = "url1" },
                    new Book { Name = "Book 2", Year = "2019", Type = "Audiobook", PictureUrl = "url2" }
                });
                context.SaveChanges();
            }
        }
    }
}


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// SEED DATA

// DATA 
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Register DbContext with dependency injection
        services.AddDbContext<LibraryContext>(options =>
            options.UseInMemoryDatabase("LibraryDB"));
    }
}

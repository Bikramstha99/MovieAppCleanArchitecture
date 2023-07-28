using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAppApplication.Interface.IRepository;
using MovieAppInfrastructure.Implementation.NewFolder;
using MovieAppInfrastructure.Implementation.Repository;
using MovieAppInfrastructure.Persistance;
using MovieAppInfrastructure.Persistance.Seed;

namespace MovieAppInfrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLinkLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MvcConnectionString");

            services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<MovieDbContext>();


           

            services.AddScoped<IDbInitializer, DbInitializer>();

            //if (builder.Configuration.GetValue<bool>("UseSP")) // To either use EF or SP 
            //{
            //    // builder.Services.AddScoped<ICommentRepository, SPCommentRepository>();
            //    // builder.Services.AddScoped<IRatingRepository, SPRatingRepository>();
            //    //services.AddScoped<IMovieRepository, SPMovieRepository>();
            //}
            //else
            //{
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            //}

            return services; // Add this line to fix the error
        }
    }
}

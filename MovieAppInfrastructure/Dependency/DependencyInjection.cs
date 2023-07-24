using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Persistance;
using MovieAppInfrastructure.Implementation.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLinkLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MvcConnectionString");

            services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(connectionString));



            // services.AddScoped<IDbInitializer, DbInitializer>();

            //if (builder.Configuration.GetValue<bool>("UseSP")) // To either use EF or SP 
            //{
            //    // builder.Services.AddScoped<ICommentRepository, SPCommentRepository>();
            //    // builder.Services.AddScoped<IRatingRepository, SPRatingRepository>();
            //    //services.AddScoped<IMovieRepository, SPMovieRepository>();
            //}
            //else
            //{
            // builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            // builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            //}

            return services; // Add this line to fix the error
        }
    }
}

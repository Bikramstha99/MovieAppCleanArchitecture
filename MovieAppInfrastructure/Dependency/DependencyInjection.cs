using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppInfrastructure.Implementation.NewFolder;
using MovieAppInfrastructure.Implementation.Repository;
using MovieAppInfrastructure.Implementation.Services;
using MovieAppInfrastructure.Persistance;
using MovieAppInfrastructure.Persistance.Seed;
using System.Text;

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
            services.AddScoped<IUnitOfWOrk, UnitOfWork>();


            //if (builder.Configuration.GetValue<bool>("UseSP")) // To either use EF or SP 
            //{
            //    // builder.Services.AddScoped<ICommentRepository, SPCommentRepository>();
            //    // builder.Services.AddScoped<IRatingRepository, SPRatingRepository>();
            //    //services.AddScoped<IMovieRepository, SPMovieRepository>();
            //}
            //else
            //{
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            //}

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:validAudience"],
            ValidIssuer = configuration["JWT:validIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secret"]))
        };

        });

            return services; 
        }
    }
}

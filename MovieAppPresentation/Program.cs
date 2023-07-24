using Microsoft.EntityFrameworkCore;
using MovieAppApplication.Persistance;
using MovieAppInfrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MvcConnectionString"),
        b => b.MigrationsAssembly("MovieAppPresentation")));
/*services.AddServices(configuration)*/
;
services.AddDataLinkLayerServices(configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

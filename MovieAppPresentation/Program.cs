using MovieAppInfrastructure.DependencyInjection;
using MovieAppInfrastructure.Persistance.Seed;

var builder = WebApplication.CreateBuilder(args);



var services = builder.Services;
var configuration = builder.Configuration;

services.AddDataLinkLayerServices(configuration);
//services.AddControllersWithViews();
services.AddRazorPages();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=Index}/{id?}");


SeedDatabase();
app.Run();
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initalizer();
    }
}

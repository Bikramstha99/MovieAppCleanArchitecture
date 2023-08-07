using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieAppInfrastructure.Persistance.Enum;

namespace MovieAppInfrastructure.Persistance.Seed
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MovieDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(MovieDbContext dBContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dBContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initalizer()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }

            if (!_roleManager.RoleExistsAsync(UserRole.Admin.ToString()).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRole.Admin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRole.User.ToString())).GetAwaiter().GetResult();
            }

            var x = new IdentityUser();
            var user = new IdentityUser
            {
                PhoneNumber = "9808139665",
                PhoneNumberConfirmed = true,
                Email = "stha.bikram999@gmail.com",
                UserName = "stha.bikram999@gmail.com",
                NormalizedEmail = "STHA.BIKRAM999@GMAIL.COM",
                NormalizedUserName = "STHA.BIKRAM999@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            var userManager = _userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();
            if (userManager.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                await _userManager.SetTwoFactorEnabledAsync(user, true); // Enable two-factor authentication
            }
            //var result = _dbContext.Users.FirstOrDefault(a => a.Email == "stha.bikram999@gmail.com");
            //_userManager.AddToRoleAsync(user, UserRole.Admin.ToString()).GetAwaiter().GetResult();
            await _dbContext.SaveChangesAsync();
        }


    }
}

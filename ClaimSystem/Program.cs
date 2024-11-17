using ClaimSystem.Models;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClaimSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ClaimVerificationService>();

            // Add Identity with in-memory database
            builder.Services.AddDbContext<ClaimDbContext>(options =>
                options.UseInMemoryDatabase("claimsDb"));

            // Add Identity services with user and role management
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ClaimDbContext>()
                .AddDefaultTokenProviders();

            // Add authentication and authorization services
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Map routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=HR}/{action=HRDashboard}/{id?}");
            });

            // Seed data for testing
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedData(userManager, roleManager);
            }

            // Run the application
            app.Run();
        }

        // Method to seed dummy accounts and roles for testing
        public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Roles to be seeded
            var roles = new[] { "Lecturer", "Academic Manager", "Programme Coordinator","HR" };

            // Seed roles if they do not exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed a Lecturer user
            if (await userManager.FindByNameAsync("testlecturer") == null)
            {
                var lecturer = new IdentityUser { UserName = "testlecturer", Email = "lecturer@test.com" };
                await userManager.CreateAsync(lecturer, "Password123!");
                await userManager.AddToRoleAsync(lecturer, "Lecturer");
            }

            // Seed an Academic Manager user
            if (await userManager.FindByNameAsync("testacademicmanager") == null)
            {
                var academicManager = new IdentityUser { UserName = "testacademicmanager", Email = "academicmanager@test.com" };
                await userManager.CreateAsync(academicManager, "Password123!");
                await userManager.AddToRoleAsync(academicManager, "Academic Manager");
            }

            // Seed a Programme Coordinator user
            if (await userManager.FindByNameAsync("testprogrammecoordinator") == null)
            {
                var programmeCoordinator = new IdentityUser { UserName = "testprogrammecoordinator", Email = "programmecoordinator@test.com" };
                await userManager.CreateAsync(programmeCoordinator, "Password123!");
                await userManager.AddToRoleAsync(programmeCoordinator, "Programme Coordinator");
            }

            //SEED A HR user 

            if (await userManager.FindByNameAsync("testHR") == null)
            {
                var HR = new IdentityUser { UserName = "testHR", Email = "HR@test.com" };
                await userManager.CreateAsync(HR, "Password123!");
                await userManager.AddToRoleAsync(HR, "HR");
            }
        }
    }
}

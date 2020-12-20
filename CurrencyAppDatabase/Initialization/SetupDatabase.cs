using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.CurrencyApp;
using CurrencyAppDatabase.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyAppDatabase.Initialization
{
    public static class SetupDatabase
    {
        private static RoleManager<AppRole> _roleManager;
        private static UserManager<AppUser> _userManager;

        public static async Task InitializeDatabase(IServiceProvider serviceProvider, string []adminParams, string[] roles)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // CreateAdmin(Name, Password, Email, RoleName)
            await CreateAdmin(adminParams[0], adminParams[1], adminParams[2], roles[0]);
            // Array of roles without "Admin" Role
            await CreateUserRoles(roles.Skip(1).ToArray());

        }

        // Creating if not exist an Admin role and Admin user
        public static async Task CreateAdmin(string adminName, string adminPass, string adminEmail, string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
                await _roleManager.CreateAsync(new AppRole(roleName));

            var adminUser = await _userManager.FindByNameAsync(adminName);

            if (adminUser != null)
                return;
            else
            {
                //Checking if new super Admin exist
                foreach (AppUser oldAdmin in (await _userManager.GetUsersInRoleAsync(roleName)))
                    await _userManager.DeleteAsync(oldAdmin);

                var newAdmin = new AppUser
                {
                    UserName = adminName,
                    Email = adminEmail,
                    CreatedAt = DateTime.Now
                };

                var createAdmin = await _userManager.CreateAsync(newAdmin, adminPass);

                if (createAdmin.Succeeded)
                    await _userManager.AddToRoleAsync(newAdmin, roleName);
            }

        }
        // Setting up default User role
        public static async Task CreateUserRoles(string[] roles)
        {
            foreach (string roleName in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                    await _roleManager.CreateAsync(new AppRole(roleName));
            }
            await CreateSampleUser(roles[0]);
        }

        // For test purposes
        private static async Task CreateSampleUser(string roleName,string userName="BasicUser", string password="User123", string userEmail="user123@simple.com")
        {
            var basicUser = await _userManager.FindByNameAsync(userName);

            if (basicUser != null)
                return;
            else
            {
                var newUser = new AppUser
                {
                    UserName = userName,
                    Email = userEmail,
                    CreatedAt = DateTime.Now
                };

                var createAdmin = await _userManager.CreateAsync(newUser, password);

                if (createAdmin.Succeeded)
                    await _userManager.AddToRoleAsync(newUser, roleName);
            }
        }
    }
}

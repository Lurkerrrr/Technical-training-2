using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Enums;
using DotHotel.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace DotHotel.Core.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AttachRoleToUser(ApplicationUser user, UserRoles role)
        {
            await CreateRoleIfNotExist(role);

            if (await _userManager.IsInRoleAsync(user, role.ToString()))
            {
                return;
            }

            await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task CreateRoleIfNotExist(UserRoles role)
        {
            if (await _roleManager.FindByNameAsync(role.ToString()) is null)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = role.ToString() });
            }
        }
    }
}

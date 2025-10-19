using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Enums;

namespace DotHotel.Core.ServiceContracts
{
    public interface IUserRoleService
    {
        Task AttachRoleToUser(ApplicationUser user, UserRoles role);
        Task CreateRoleIfNotExist(UserRoles role);
    }
}

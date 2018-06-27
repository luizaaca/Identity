using Core.Model.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> AddUser(ApplicationUser user);
        Task<IdentityResult> UpdateUser(ApplicationUser user);
        Task<IdentityResult> DeleteUser(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUser(int userId);
        Task<ApplicationUser> GetUser(string userName);
    }
}

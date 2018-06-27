using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Identity;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Core.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public async Task<IdentityResult> AddUser(ApplicationUser user)
        {
            if (await connection.InsertAsync(user) > 0) return IdentityResult.Success;
            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            user.Active = false;
            if(await connection.UpdateAsync(user)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
        }

        public async Task<ApplicationUser> GetUser(int userId)
        {
            return await connection.GetAsync<ApplicationUser>(userId);
        }
        public async Task<ApplicationUser> GetUser(string userName)
        {
            var sql = @"select * from Users where UserName = @Username AND Active = 1";
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserName = userName });
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await connection.GetAllAsync<ApplicationUser>();
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
            if(await connection.UpdateAsync(user)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
        }
    }
}

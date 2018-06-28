using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            var sql = @"select * from Users where Email = @Email AND Active = 1";
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { Email = email });
        }

        public async Task<IEnumerable<ApplicationRole>> GetUserRoles(int userId)
        {
            return await Task.Run(() =>
            {
                var roleDictionary = new Dictionary<int, ApplicationRole>();

                var sql = @"select A.*, C.Id as ActionId, C.* from Roles A
                            inner join RolesActions B on A.Id = B.RoleId AND B.Active = 1
                            inner join Actions C on C.Id = B.ActionId AND C.Active = 1
                            inner join UserRoles D on D.UserId = @UserId AND D.Active = 1
                            where A.Active = 1";

                return connection.Query<ApplicationRole, Model.Identity.Action, ApplicationRole>(sql,
                    (role, action) =>
                    {
                        if (!roleDictionary.TryGetValue(role.Id, out ApplicationRole roleEntry))
                        {
                            roleEntry = role;
                            roleEntry.RoleActions = new List<Model.Identity.Action>();
                            roleDictionary.Add(roleEntry.Id, roleEntry);
                        }

                        roleEntry.RoleActions.Add(action);

                        return roleEntry;
                    },
                    new { UserId = userId },
                    splitOn: "ActionId")
                .Distinct()
                .ToList();
            });
        }

        public async Task<IdentityResult> AddUserRole(int userId, int roleId)
        {
            var sql = @"insert into UserRoles values (@UserId, @RoleId, 1)";

            if (await connection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId }) > 0) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not bind user {userId} to the role {roleId}." });
        }
    }
}

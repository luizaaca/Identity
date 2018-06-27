using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model.Identity;
using Core.Repositories;
using Identity.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    //[Authorize]
    public class UserManagementController : Controller
    {
        //[RequiresPermission("UserManagement.List")]
        public IActionResult Index()
        {
            var repo = new UserRepository();
            var user = new ApplicationUser();
            user.UserName = "Joao";
            user.Email = "joao@joao.com";
            user.PasswordHash = "123";
            user.Active = true;

            repo.AddUser(user).Wait();

            var user2 = repo.GetUser(user.Id).Result;

            user2.Email = "joao1@joao.com";
            repo.UpdateUser(user2).Wait();

            repo.DeleteUser(user).Wait();

            var list = repo.GetAllUsers().Result;

            return View();
        }
    }
}
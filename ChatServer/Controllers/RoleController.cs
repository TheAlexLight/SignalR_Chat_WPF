using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Controllers
{
    public class RoleController
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityError>> Create(string name)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (!string.IsNullOrEmpty(name))
            {
               IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));

                errors = result.Errors.ToList();
            }
            else
            {
                IdentityError error = new IdentityError();
                error.Description = "Name cannot be null or empty";
                errors.Add(error);
            }

            return errors;
        }

        public async Task<IdentityResult> Delete(string name)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(name);

            IdentityResult result = new IdentityResult();

            if (role != null)
            {
                result = await _roleManager.DeleteAsync(role);
            }

            return result;
        }

        public async Task Assign(string username, List<string> addedRoles)
        {
          User user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var removedRoles = userRoles.Except(addedRoles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
            }
        }

        //public async Task Edit(string id)
        //{
        //    User user = await _userManager.FindByIdAsync(id);

        //    if (user != null)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        var allRoles = _roleManager.Roles.ToList();

        //        RoleModel roleModel = new RoleModel()
        //        {
        //            UserId = user.Id,
        //            UserEmail = user.Email,
        //            UserRoles = userRoles,
        //            AllRoles = allRoles
        //        };
        //    }

        //    IdentityRole role = await _roleManager.FindByIdAsync(id);
        //}
    }
}

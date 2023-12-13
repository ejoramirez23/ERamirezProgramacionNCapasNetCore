
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ML;
using System.ComponentModel.DataAnnotations;


namespace PLIdentity.Controllers
{
    public class RolController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        public RolController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Roles = roleManager.Roles.ToList();
            return View(Roles);
        }

        [HttpGet]
        public IActionResult Asignar(Guid IdRole, string Name)
        {
            ML.Result result = BL.IdentityUser.GetAll();
            ML.UserIdentity userIdentity = new ML.UserIdentity();
            if (result.Correct)
            {
                userIdentity.IdentityUsers = result.Objects;
            }
            userIdentity.Rol = new ML.Rol();
            userIdentity.Rol.Name = Name;
            userIdentity.Rol.RoleId = IdRole;

            return View(userIdentity);
        }

        [HttpPost]
        public IActionResult Asignar(ML.UserIdentity user)
        {
            ML.Result result = BL.IdentityUser.Asignar(user);
            if (result.Correct)
            {
                ViewBag.Message = "Rol asigando correctamente";

            }
            else
            {
                ViewBag.Message = result.Message;
            }

            return PartialView("Modal");
        }

        [HttpGet]
        public IActionResult Form(Guid IdRole, string Name)
        {
            IdentityRole role = new IdentityRole();
            if (Name != null)
            {
                role.Name = Name;
                role.Id = IdRole.ToString();
                return View(role);
            }
            else
            {
                return View(role);
            }

        }



        [HttpPost]
        public async Task<IActionResult> Form([Required] IdentityRole rol)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByIdAsync(rol.Id.ToString());
               
                if (role == null)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(rol.Name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                    else
                    {

                    }
                }
                else 
                {
                    role.Id = await roleManager.GetRoleIdAsync(rol);
                    role.Name = await roleManager.GetRoleNameAsync(rol);

                    IdentityResult result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                }
            }
            return View(rol);
        }

       

        public async Task<IActionResult> Delete(Guid IdRole)
        {
            IdentityRole role = await roleManager.FindByIdAsync(IdRole.ToString());
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("GetAll");
                //else
                //    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("GetAll", roleManager.Roles);
        }
    }
}

using LogicLevel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.IdentityClass;
using ProjectDataStructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsindianCommunity.CustomClasses;

namespace UsindianCommunity.Controllers
{

    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        private readonly AppDbContext AppDb;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<AdministrationController> log, AppDbContext appDb)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = log;
            this.AppDb = appDb;
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserClaim(string userId)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "True"))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaim(UserClaimsViewModel model)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            result = await userManager.AddClaimsAsync(user,
                model.Cliams.Select(c => new Claim(c.ClaimType, c.IsSelected ? "True" : "False")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId, AreaName = Area });

        }

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        [HttpGet]
        public async Task<IActionResult> ManageUserRole(string id)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            ViewBag.UserId = id;
            var User = await userManager.FindByIdAsync(id);
            if (User == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            var Model = new List<UserRolesViewModel>();
            foreach (var role in roleManager.Roles.ToList())
            {

                var UserRolesViewModel = new UserRolesViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(User, UserRolesViewModel.RoleName))
                {
                    UserRolesViewModel.IsSelected = true;
                }
                else
                {
                    UserRolesViewModel.IsSelected = false;

                }
                Model.Add(UserRolesViewModel);
            }
            return View(Model);
        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRole(List<UserRolesViewModel> model, string id)
        {
            try
            {
                string Area = HttpContext.Request.Query["AreaName"];
                ViewBag.Area = HttpContext.Request.Query["AreaName"];
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return View("NotFound");

                }
                var roles = await userManager.GetRolesAsync(user);

                var result = await userManager.RemoveFromRolesAsync(user, roles.ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(model);
                }

                result = await userManager.AddToRolesAsync(user,
                    model.Where(x => x.IsSelected).Select(y => y.RoleName));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }
           

            return RedirectToAction("EditUser", new { Id = id, AreaName = Area });
            }
            catch(Exception e)
            {
                
                logger.LogError($"exception Occured: {e}");
                ViewBag.ErrorTitle = $"Error In Services";
                ViewBag.ErrorMessage = $"Read Error Message  And Provide Fix <br />{ e.Message}";
                return View("../Error/Error");            

             }



        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            //string Id= HttpContext.Request.Query["Id"];
            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} cannot be found";
                return View("NotFound");
            }
            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Type + ":" + c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                //user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = area;
            IQueryable<ApplicationUser> query = AppDb.applicationUsers.Where(u => u.Id == id).Include(e => e.IndiaUserAddress);
            ApplicationUser user = query.FirstOrDefault();
            //var user = await userManager.FindByIdAsync(id);
            IndiaUserAddress indiaUserAddress = user.IndiaUserAddress;
            if (user == null && indiaUserAddress == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                if (indiaUserAddress != null)
                {
                    var deleteaddress = AppDb.Remove(entity: indiaUserAddress);
                }
                try
                {
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded || indiaUserAddress != null)
                    {
                        return RedirectToAction("ListUsers", new { Areaselector = area });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListUsers");
                }
                catch(Exception)
                {
                    ViewBag.ErrorTitle = $"{user.Email}  is in use";
                    ViewBag.ErrorMessage = $"{user.Email} User cannot be deleted as  This User Associated With Role Or some claim Remove That Role And Claim And Try After That";
                    return View("../Error/Error");

                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> AccountInformation()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            string Area = HttpContext.Request.Query["AreaName"];
            return View(usr);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role With Id ={id} can not found";
                return View("Not Found");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageRole", "Administration");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ManageRole");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"exception Occured: {ex}");
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("../Error/Error");
                }
            }

        }

        /// <summary>
        /// Done 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsers(string searchString, int? pageNumber, string Areaselector)
        {
            string area = HttpContext.Request.Query["AreaName"];
            string Area = TempData["Area"].ToString() ?? area;
            if (Area == null)
            {
                Area = Areaselector;
            }

            ViewBag.Area = Area;
            ViewBag.UserName = searchString;
            var users = userManager.Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => (s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString) || s.Email.Contains(searchString)));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            int pagesize = 3;
            var Model = await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pagesize);
            return View(Model);


        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            TempData["Area"] = Area;
            return View();
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel, string name)
        {
            // ViewBag.Area = createRoleViewModel.RoleName;
            string Area = HttpContext.Request.Query["AreaName"];

            ViewBag.Area = Area;

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = createRoleViewModel.RoleName };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    TempData["Area"] = Area;
                    return RedirectToAction("ManageRole", "Administration");
                }
                foreach (IdentityError identityError in result.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }
            return View();
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult ManageRole()
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = TempData["Area"] ?? Area;
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            string Area = HttpContext.Request.Query["AreaName"];
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            // Retrieve all the Users
            foreach (ApplicationUser user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            string Area = HttpContext.Request.Query["AreaName"];
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ManageRole", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserInRole(string id)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            string Area = HttpContext.Request.Query["AreaName"];
            string Id = id;
            ViewBag.roleId = Id;
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserInRole(List<UserRoleViewModel> model,string RoleId)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            string Area = HttpContext.Request.Query["AreaName"];
            string roleId = RoleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                IdentityResult result = null;
                var user = await userManager.FindByIdAsync(model[i].UserId);
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        //TempData["Area"] = Area;
                        return RedirectToAction("EditRole", new { Id = roleId, AreaName = Area });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId , AreaName = Area });
        }
    }
}

using LogicLevel.DefinationRepository;
using MailServices.InterfaceRepository;
using MailServices.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ProjectDataStructure.IdentityClass;
using ProjectDataStructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UsindianCommunity.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        public readonly RoleManager<IdentityRole> roleManager;
        private readonly IAccountManager accountManager;
        private readonly IMailService mailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
         ILogger<AccountController> ilogger, RoleManager<IdentityRole> roleManager, IAccountManager accountManager, IMailService mailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = ilogger;
            this.roleManager = roleManager;
            this.accountManager = accountManager;
            this.mailService = mailService;
        }
        //Done Methods
        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            var user = await userManager.GetUserAsync(User);
            var userHasPassword = await userManager.HasPasswordAsync(user);
            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var result = await userManager.AddPasswordAsync(user, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await signInManager.RefreshSignInAsync(user);
                ViewBag.Section = 2;
                ViewBag.Message = "Password Added Successfull";
                return View("Confirmation");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ChangePasswordAsync()
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            var user = await userManager.GetUserAsync(User);
            var userHasPassword = await userManager.HasPasswordAsync(user);
            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword",new { AreaName = ViewBag.Area});
            }
            return View();            
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);
                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ViewBag.Section = 2;
                    ViewBag.Message = "Password Changed Successfull";
                    return View();
                }
                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);
                ViewBag.Section = 2;
                ViewBag.Message = "Password Changed Successfull";
                return View("Confirmation");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email,string AreaName)
        {
            string Area = AreaName;
            ViewBag.Area = Area;
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        ViewBag.Section = 1;
                        ViewBag.Message = "Password Changed Successfull";
                        return View("Confirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                ViewBag.Section = 1;
                ViewBag.Message = "Password Changed Successfull";
                return View("Confirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token,AreaName=Area }, Request.Scheme);
                    // Log the password reset link
                    logger.Log(LogLevel.Warning, passwordResetLink);
                    MailRequest mailRequest = new MailRequest()
                    {
                        ToEmail = user.Email,
                        Subject = "Password Reset Confirmation",
                        Body = @"<html><body><p>This Mail Contain Link To Change Password Confirmation For Your Account</p><br /><pre><a href='" + passwordResetLink + "'>Click This Link To Change Password</a></pre></body></html>",

                    };
                    var EmailResult = await mailService.SendEmailAsync(mailRequest);
                    if (EmailResult == 1)
                    {
                        ViewBag.Section = 0;
                        ViewBag.Message = "If you have an account with us, we have sent an email with the instructions to reset your password.";
                        return View("Confirmation");
                    }
                    else
                    {
                        TempData["Area"] = Area;
                        ViewBag.Title = "Unsuccess";
                        ViewBag.ErrorTitle = "Email Sending Unsuccessful";
                        ViewBag.ErrorMessage = "Theres is Some Problem With Services Please Try After Some Time ";
                        return View("../Error/Error");
                    }
                        // Send the user to Forgot Password Confirmation view
                    
                }
                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                ViewBag.Section = 0;
                ViewBag.Message = "If you have an account with us, we have sent an email with the instructions to reset your password.";
                return View("Confirmation");
            }
            return View(model);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl, string AreaName)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl });
            var properties = signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string ReturnUrl = null, string remoteError = null, string area = null)
        {
            try
            {
                string Area = (ReturnUrl.Split("/"))[1];
                string returnUrl = ReturnUrl ?? Url.Content("~/");

                ViewBag.Area = Area;
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };
                if (remoteError != null)
                {
                    ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                    return View("Login", loginViewModel);
                }
                // Get the login information about the user from the external login provider
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ModelState.AddModelError(string.Empty, "Error loading external login information.");
                    return View("Login", loginViewModel);
                }

                // If the user already has a login (i.e if there is a record in AspNetUserLogins
                // table) then sign-in the user with this external login provider
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                ApplicationUser user = null;
                if (email != null)
                {
                    // Find the user
                    user = await userManager.FindByEmailAsync(email);
                    // If email is not confirmed, display login view with validation error
                    if (user != null && !user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                        return View("Login", loginViewModel);
                    }
                }
                var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                   info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                // If there is no record in AspNetUserLogins table, the user may not have
                // a local account
                else
                {
                    // Get the email claim value
                    if (email != null)
                    {
                        // Create a new user without password if we do not have a user already
                        if (user == null)
                        {
                            user = new ApplicationUser
                            {
                                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                            };
                            await userManager.CreateAsync(user);
                            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token, AreaName = Area }, Request.Scheme);
                            MailRequest mailRequest = new MailRequest()
                            {
                                ToEmail = info.Principal.FindFirstValue(ClaimTypes.Email),
                                Subject = "Email Confirmation",
                                Body = @"<html><body><h1>Thank You For Registration</h1><p>This Mail Contain Link To Email Confirmation For Your REgistartion</p><br /><pre><a href='" + confirmationLink + "'>Click TO Confirm Email</a></pre></body></html>",

                            };
                            var EmailResult = await mailService.SendEmailAsync(mailRequest);
                            if (EmailResult == 1)
                            {
                                IdentityRole identityRole;
                                if (mailRequest.ToEmail.ToUpper() == "NIKIRAVI2016@GMAIL.COM" || mailRequest.ToEmail.ToUpper() == "RAVINIKI2016@YAHOO.COM")
                                {
                                    var initialRole = roleManager.Roles.Where(s => s.Name == "Admin");
                                    identityRole = initialRole.FirstOrDefault();
                                }
                                else
                                {
                                    var initialRole = roleManager.Roles.Where(s => s.Name == "User");
                                    identityRole = initialRole.FirstOrDefault();
                                }
                                var role = new AspNetRoles()
                                {
                                    RoleId = identityRole.Id,
                                    USerId = user.Id
                                };
                                accountManager.SaveUserRole(role);
                                // If the user is signed in and in the Admin role, then it is
                                // the Admin user that is creating a new user. So redirect the
                                // Admin user to ListRoles action
                                TempData["Area"] = Area;
                                ViewBag.Title = "Successful";
                                ViewBag.ErrorTitle = "Registration successful";
                                ViewBag.ErrorMessage = "Before you Login, please confirm your " + "email, by clicking on the confirmation link we have emailed you";
                                return View("../Error/Error");
                            }
                            else
                            {
                                await userManager.DeleteAsync(user);
                                TempData["Area"] = Area;
                                ViewBag.Title = "UnSuccessful";
                                ViewBag.ErrorTitle = "Registration Unsuccessful";
                                ViewBag.ErrorMessage = "Theres is Some Problem With Services Please Try After Some Time ";
                                return View("../Error/Error");
                            }
                        }

                        // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                        await userManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect(returnUrl);
                    }
                    // If we cannot find the user email we cannot continue
                    ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                    ViewBag.ErrorMessage = "Please contact support on nikiravi2016@gmail.com";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                //logger.LogError($"exception Occured: {ex}");
                ViewBag.ErrorTitle = $"Error  Deatil<br>";
                ViewBag.ErrorMessage = "Read Full Error And Try To Fix<br>" + $"{ex.Message} ";
                return View("../Error/Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area ?? TempData["Area"] ?? returnUrl.Split("/")[1].ToString(); ;
            ViewBag.returnUrl = returnUrl;
            string returnstring = string.Format("/{0}", Area);
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl ?? returnstring,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home", new { area = ViewBag.Area });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnurl)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = Area;
            loginViewModel.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null && !user.EmailConfirmed &&
                            (await userManager.CheckPasswordAsync(user, loginViewModel.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(loginViewModel);
                }
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe,true);
                if (result.Succeeded && returnurl != null)
                {
                    return LocalRedirect(returnurl);
                }
                else if (result.Succeeded && returnurl == null)
                {
                    return RedirectToAction("index", "home", new { area = Area });
                }
                if (result.IsLockedOut)
                {
                    return View("AccountLocked",new { AreaName=Area});
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            string area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = area;
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home", new { area = area });
        }
        [HttpGet]
        public IActionResult Register()
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            RegisterViewModel registerViewModel = new RegisterViewModel();
            var roles = roleManager.Roles;
            registerViewModel.roles = roleManager.Roles;
            ViewBag.Roles = ToSelectList(roles, "Id", "Name");
            if ((signInManager.IsSignedIn(User) && User.IsInRole("Admin")))
            {
                return View(registerViewModel);
            }
            else if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home", new { area = Area });
            }
            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerviewModel)
        {
            string Area = HttpContext.Request.Query["AreaName"];
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = registerviewModel.FirstName,
                    LastName = registerviewModel.LastName,
                    Email = registerviewModel.Email,
                    UserName = registerviewModel.Email,
                    PhoneNumber = registerviewModel.Phone
                };
                var result = await userManager.CreateAsync(user, registerviewModel.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token, AreaName=Area }, Request.Scheme);
                    logger.Log(LogLevel.Warning, confirmationLink);
                    MailRequest mailRequest = new MailRequest()
                    {
                        ToEmail = user.Email,
                        Subject = "Email Confirmation",
                        Body = @"<html><body><h1>Thank You For Registration</h1><p>This Mail Contain Link To Email Confirmation For Your REgistartion</p><br /><pre><a href='" + confirmationLink + "'>Click TO Confirm Email</a></pre></body></html>",

                    };
                    var EmailResult = await mailService.SendEmailAsync(mailRequest);
                    if (EmailResult == 1)
                    {
                        if (registerviewModel.roles != null)
                        {
                            var role = new AspNetRoles()
                            {
                                RoleId = Request.Form["roles"].ToString(),
                                USerId = user.Id
                            };
                            accountManager.SaveUserRole(role);
                        }
                        else
                        {
                            IdentityRole identityRole;
                            if (user.Email.ToUpper() == "NIKIRAVI2016@GMAIL.COM" || user.Email.ToUpper() == "RAVINIKI2016@YAHOO.COM")
                            {
                                var initialRole = roleManager.Roles.Where(s => s.Name == "Admin");
                                identityRole = initialRole.FirstOrDefault();
                            }
                            else
                            {
                                var initialRole = roleManager.Roles.Where(s => s.Name == "User");
                                identityRole = initialRole.FirstOrDefault();
                            }
                            var role = new AspNetRoles()
                            {
                                RoleId = identityRole.Id,
                                USerId = user.Id
                            };
                            accountManager.SaveUserRole(role);
                        }
                        // If the user is signed in and in the Admin role, then it is
                        // the Admin user that is creating a new user. So redirect the
                        // Admin user to ListRoles action
                        if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        {
                            TempData["Area"] = Area;
                            return RedirectToAction("ListUsers", "Administration");
                        }
                        TempData["Area"] = Area;
                        ViewBag.Title = "Successful";
                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you Login, please confirm your " + "email, by clicking on the confirmation link we have emailed you";
                        return View("../Error/Error");
                    }
                    else
                    {
                        await userManager.DeleteAsync(user);
                        TempData["Area"] = Area;
                        ViewBag.Title = "UnSuccessful";
                        ViewBag.ErrorTitle = "Registration Unsuccessful";
                        ViewBag.ErrorMessage = "Theres is Some Problem With Services Please Try After Some Time ";
                        return View("../Error/Error");
                    }                                 

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registerviewModel);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }
        [HttpGet]
        public IActionResult AccessDenied(string AreaName)
        {
            string returnurl = HttpContext.Request.Query["ReturnUrl"].ToString();
            string Area = (returnurl.Substring(returnurl.LastIndexOf("?") + 1)).Substring((returnurl.Substring(returnurl.LastIndexOf("?") + 1)).LastIndexOf("=") + 1);
            ViewBag.Area = Area;
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token,string AreaName)
        {
            ViewBag.Area = AreaName;
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("index","Home",new {area=AreaName});
            }
            ViewBag.Title = "Not Verified";
            ViewBag.ErrorMessage = "Email Is Not Verified Use Another Email Address";
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("../Eror/Error");
        }
        [NonAction]
        private SelectList ToSelectList(IQueryable<IdentityRole> roles, string v1, string v2)
        {
            List<IdentityRole> list = new List<IdentityRole>();
            foreach (IdentityRole role in roles)
            {
                list.Add(new IdentityRole()
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }
            return new SelectList(list, v1, v2);
        }
    }
}

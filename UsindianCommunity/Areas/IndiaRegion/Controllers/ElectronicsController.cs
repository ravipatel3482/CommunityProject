using AutoMapper;
using Dapper;
using LogicLevel;
using LogicLevel.DefinationRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.Electronics;
using ProjectDataStructure.Enum;
using ProjectDataStructure.IdentityClass;
using ProjectDataStructure.IndiaViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UsindianCommunity.Areas.IndiaRegion.Controllers
{
    [Area("indiaRegion")]
    public class ElectronicsController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountManager accountManager;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IIndiaRepository indiaRepository;
        public readonly AppDbContext appDbContext;
        private readonly IElectronics electronics;
        private readonly IWebHostEnvironment HostingEnvironment;
        private readonly ILogger<ElectronicsController> logger;
        private readonly RoleManager<IdentityRole> rolemanager;
        public ElectronicsController(UserManager<ApplicationUser> userManager,
                                      SignInManager<ApplicationUser> signInManager,
                                      IAccountManager accountManager, RoleManager<IdentityRole> roManager,
                                      IMapper mapper, ILogger<ElectronicsController> Logger,
                                      IIndiaRepository indiaRepository, AppDbContext appDbContext,
                                      IElectronics electronics, IWebHostEnvironment hostingEnvironment)
        {
            this.signInManager = signInManager;
            this.accountManager = accountManager;
            this.mapper = mapper;
            this.userManager = userManager;
            this.indiaRepository = indiaRepository;
            this.appDbContext = appDbContext;
            this.electronics = electronics;
            this.HostingEnvironment = hostingEnvironment;
            this.logger = Logger;
            this.rolemanager = roManager;
        }
        [HttpGet]
        public async Task<IActionResult> servieProvider()
        {
            ViewBag.Area = HttpContext.Request.Query["AreaName"];
            try
            {
                
                IEnumerable<ServiceProviderDisplayViewModel> serviceProviderDisplayViewModel = await electronics.GetAllServicesProvider();
                return View(serviceProviderDisplayViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError($"exception Occured: {ex}");
                ViewBag.ErrorTitle = $"Error  Deatil";
                ViewBag.ErrorMessage = "Read Full Error And Try To Fix " + $"{ex.Message} ";
                return View("../Error/Error");
            }

        }
        [HttpGet]
        public IActionResult Product()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProviderDeatilForms()
        {
            ServiceproviderViewModel serviceprovider = new ServiceproviderViewModel();
            List<ServicesTypesViewModel> services = new List<ServicesTypesViewModel>();
            foreach (var ServiceType in appDbContext.SerciesTypes)
            {
                ServicesTypesViewModel ServiceTypeViewModel = new ServicesTypesViewModel()
                {
                    isSelected = false,
                    ServicesTypeId = ServiceType.ServicesTypeId,
                    ServiceType = ServiceType.ServiceType,
                };
                services.Add(ServiceTypeViewModel);
            }
            serviceprovider.ServicesTypes = services;
            var City = appDbContext.IndiaCities;
            ApplicationUser applicationLevelUser = await userManager.GetUserAsync(User);
            IQueryable<ApplicationUser> query = appDbContext.applicationUsers.Where(u => u.Id == applicationLevelUser.Id).Include(e => e.IndiaUserAddress);
            ApplicationUser applicationUser = query.FirstOrDefault();
           
            if (applicationUser != null)
            {
                    if(applicationUser.IndiaUserAddress != null)
                    {
                    IQueryable<IndiaUserAddress> AddressQuery = appDbContext.IndianUserAddresses.Where(A => A.AddressId == applicationUser.IndiaUserAddress.AddressId).Include(c => c.City);
                    applicationUser.IndiaUserAddress = AddressQuery.FirstOrDefault();
                    ViewBag.AddressFlag = "false";
                        serviceprovider = new ServiceproviderViewModel()
                        {
                            AddressId = applicationUser.IndiaUserAddress.AddressId,
                            ApplicationUserId = applicationUser.Id,
                            FirstName = (applicationUser.FirstName != null) ? applicationUser.FirstName : string.Empty,
                            Email = applicationUser.Email,
                            BusinessAddress = applicationUser.IndiaUserAddress.Address,
                            ZipCode = applicationUser.IndiaUserAddress.Zipcode,
                            stateID = (int)applicationUser.IndiaUserAddress.City.indiastate,
                            City = applicationUser.IndiaUserAddress.City.CityName,
                            phoneNumber = applicationUser.PhoneNumber,
                            ServicesTypes = services,
                            
                        };
                    }
                    else
                    {
                        serviceprovider = new ServiceproviderViewModel()
                        {
                            ApplicationUserId = applicationUser.Id,
                            FirstName = (applicationUser.FirstName != null) ? applicationUser.FirstName : string.Empty,
                            Email = applicationUser.Email,
                            ServicesTypes = services,
                            phoneNumber = applicationUser.PhoneNumber ?? string.Empty,                         

                        };
                    }
                    return View(serviceprovider);
            }
            else
            {
                ViewBag.Title = "NotFound";
                ViewBag.ErrorTitle = "User Not Found";
                ViewBag.ErrorMessage = "Do Registaration First And Then Try ";
                return View("../Error/Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ProviderDeatilForms(ServiceproviderViewModel serviceproviderViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    serviceproviderViewModel.stateID = (int)serviceproviderViewModel.IndiaState;
                    // If the Photo property on the incoming model object is not null, then the user
                    // has selected an image to upload.
                    if (serviceproviderViewModel.BusinessPhoto != null)
                    {
                        // The image must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the inject
                        // HostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, "images");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + serviceproviderViewModel.BusinessPhoto.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        serviceproviderViewModel.BusinessPhoto.CopyTo(new FileStream(filePath, FileMode.Create));
                        serviceproviderViewModel.photopath = uniqueFileName;
                    }
                    int result =await electronics.SaveServiceProvider(serviceproviderViewModel);
                    if (result > 0)
                    {
                        IdentityRole identityRole = new IdentityRole();
                        var initialRole = rolemanager.Roles.Where(s => s.Name == "Services Provider");
                        identityRole = initialRole.FirstOrDefault();
                        var role = new AspNetRoles()
                        {
                            RoleId = identityRole.Id,
                            USerId = serviceproviderViewModel.ApplicationUserId
                        };
                        accountManager.SaveUserRole(role);
                        ApplicationUser applicationLevelUser = await userManager.GetUserAsync(User);
                        await signInManager.SignOutAsync();
                        await signInManager.SignInAsync(applicationLevelUser, false);
                        return RedirectToAction("ConfirmationForProvider", new { Id = result, Description = serviceproviderViewModel.Service });
                     }
                    else
                    {
                        ModelState.AddModelError("Error", "Please Fix The error");
                        return View("../Error/Error");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"exception Occured: {ex}");
                    ViewBag.ErrorTitle = $"Error  Deatil";
                    ViewBag.ErrorMessage = "Read Full Error And Try To FIx " + $"{ex.Message} ";
                    return View("../Error/Error");
                }
            }
            List<ServicesTypesViewModel> services = new List<ServicesTypesViewModel>();
            foreach (var ServiceType in appDbContext.SerciesTypes)
            {
                ServicesTypesViewModel ServiceTypeViewModel = new ServicesTypesViewModel()
                {
                    isSelected = false,
                    ServicesTypeId = ServiceType.ServicesTypeId,
                    ServiceType = ServiceType.ServiceType,
                };
                services.Add(ServiceTypeViewModel);
            }
            serviceproviderViewModel.ServicesTypes = services;
            return View(serviceproviderViewModel);
        }
        [HttpGet]
        public async  Task<IActionResult> ServiceOrder()
        {
            IEnumerable<ServicesOrderViewModel> servicesOrderViewModels = await electronics.GetAllServiceInquiry();
            foreach(ServicesOrderViewModel s in servicesOrderViewModels)
            {
                s.status =(Servicesstatus)s.Servicesstatus;
            }
            return View(servicesOrderViewModels);
        }
        //[AcceptVerbs("Get", "Post")]
        [HttpPost]
        public async  Task<IActionResult> EditServiceStatus(ServicesOrderViewModel services ,int Id)
        {
           
            try
            {
                
                electronics.UpdateServcieStatus(Id, (int)services.Servicesstatus);
                
            }
            catch (Exception ex)
            {
                logger.LogError($"exception Occured: {ex}");
                ViewBag.ErrorTitle = $"Error  Deatil";
                ViewBag.ErrorMessage = "Read Full Error And Try To Fix " + $"{ex.Message} ";
                return View("../Error/Error");
            }
            return RedirectToAction("ServiceOrder", "Electronics");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ServiceInquiry()
        {
            var City = appDbContext.IndiaCities.Take(7);
            ViewBag.City = ToSelectList(City, "CityId", "CityName");
            ApplicationUser applicationLevelUser = await userManager.GetUserAsync(User);

            if (applicationLevelUser != null)
            {
                IQueryable<ApplicationUser> query = appDbContext.applicationUsers.Where(u => u.Id == applicationLevelUser.Id).Include(e => e.IndiaUserAddress);
                ApplicationUser applicationUser = query.FirstOrDefault();
                ServicesOrderViewModel servicesOrder = new ServicesOrderViewModel();                
                ViewBag.Bool = true;
                if (applicationUser.IndiaUserAddress != null)
                {
                    ViewBag.AddressFlag = true;
                    IQueryable<IndiaUserAddress> AddressQuery = appDbContext.IndianUserAddresses.Where(A => A.AddressId == applicationUser.IndiaUserAddress.AddressId).Include(c => c.City);
                    applicationUser.IndiaUserAddress = AddressQuery.FirstOrDefault();
                    IndiaUserAddress indiaUserAddress = applicationUser.IndiaUserAddress;
                    servicesOrder.AddressId = indiaUserAddress.AddressId;
                    servicesOrder.ServicesAddress = indiaUserAddress.Address;
                    servicesOrder.Zipcode = indiaUserAddress.Zipcode;
                    servicesOrder.City = indiaUserAddress.City;
                    servicesOrder.CityId = servicesOrder.City.CityId;

                }
                else
                {
                    ViewBag.AddressFlag = false;
                }
                servicesOrder.Name =applicationUser.FirstName ??  string.Empty;
                servicesOrder.Email = applicationUser.Email;
                servicesOrder.PhoneNo = applicationUser.PhoneNumber ?? string.Empty;
                servicesOrder.ApplicationUserId = applicationUser.Id;
                return View(servicesOrder);
            }
            else
            {
                ViewBag.Title = "NotFound";
                ViewBag.ErrorTitle = "User Not Found";
                ViewBag.ErrorMessage = "Do Registaration First And Then Try ";
                return View("../Error/Error");
            }          
        }
        [HttpPost]
        public async Task<IActionResult> ServiceInquiry(ServicesOrderViewModel servicesOrder)
        {
            if (ModelState.IsValid)
            {
                        servicesOrder.Servicesstatus = (int)Servicesstatus.Open;
                        try
                        {
                            int result = await electronics.Saveserviceorder(servicesOrder);
                            if (result>0)
                            {
                            return RedirectToAction("Confirmation", new { Id = result, Description = servicesOrder.ServicesDescription });                       
                            }
                            else
                            {
                                ModelState.AddModelError("Error", "Please Fix The error");
                                return View("../Error/Error");
                            }                   
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"exception Occured: {ex}");
                            ViewBag.ErrorTitle = $"Error  Deatil";
                            ViewBag.ErrorMessage = "Read Full Error And Try To FIx " + $"{ex.Message} ";
                            return View("../Error/Error");
                        }
            }

            return View(servicesOrder);
        }
        [HttpGet]
        public IActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmationForProvider(int Id, string Description)
        {


            ServicesProvider ServicesOrder = new ServicesProvider()
            {
                ProviderId = Id,
                ServiceYouProvide = Description
            };

            return View(ServicesOrder);
        }

        [HttpGet]
        public IActionResult Confirmation(int Id, string Description)
        {


            ServicesOrder ServicesOrder = new ServicesOrder()
            {
                ServicesInquiryID = Id,
                ServicesDescription = Description,
                Servicesstatus = Servicesstatus.Open
            };

            return View(ServicesOrder);
        }
        [NonAction]
        private SelectList ToSelectList(IQueryable<City> cities, string v1, string v2)
        {
            List<City> list = new List<City>();
            foreach (City city in cities)
            {
                list.Add(new City()
                {
                    CityId = city.CityId,
                    CityName = city.CityName
                });
            }
            return new SelectList(list, v1, v2);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            string loginurl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, "Account/Login");
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use, click the <a href='" + loginurl + "'> here </a> to login.");
            }
        }


    }
}

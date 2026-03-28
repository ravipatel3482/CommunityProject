using LogicLevel;
using MailServices.implementationRepository;
using MailServices.InterfaceRepository;
using MailServices.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using ProjectDataStructure.IdentityClass;
using System;
using UsindianCommunity.Security;




var builder = WebApplication.CreateBuilder(args);

// enable NLog
builder.Host.UseNLog();

IConfiguration Configuration = builder.Configuration;


builder.Services.AddLogicLevelLibrary();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 3;
    option.SignIn.RequireConfirmedEmail = true;
    option.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
    option.Lockout.MaxFailedAccessAttempts = 5;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration["DBConnection"]));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy",
        policy => policy.RequireClaim("Delete-Post", "true").RequireClaim("Create-Post", "true"));
    options.AddPolicy("RolePolicy",
        policy => policy.RequireRole("Admin"));

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EditRolePolicy", policy =>
        policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
});

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "561545338674-4dml8sn8hldhkr5rcfmn62chtadi9udg.apps.googleusercontent.com";
    options.ClientSecret = "w6bPXqHg03Ro9slQoHb28xV6";
}).AddFacebook(options =>
{
    options.AppId = "710579106562095";
    options.AppSecret = "c32cadb0049826c6252d3d2e04973190";
});

builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
        o.TokenLifespan = TimeSpan.FromHours(5));
builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
      o.TokenLifespan = TimeSpan.FromDays(1));
builder.Services.AddSingleton<DataProtectionPurposeStrings>();
builder.Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

// --- Build app and configure middleware (moved from Startup.Configure) ---
var app = builder.Build();

var env = app.Environment;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
}


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "indiaRegion",
    areaName: "indiaRegion",
    pattern: "indiaRegion/{controller=Home}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name: "UsRegion",
    areaName: "UsRegion",
    pattern: "UsRegion/{controller=Home}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name: "CanadaRegion",
    areaName: "CanadaRegion",
    pattern: "CanadaRegion/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
using code.Auth;
using code.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.s

builder.Services.AddRazorPages();   
builder.Services.AddControllersWithViews();

// set up authentication
var authConfig = builder.Configuration.GetSection("AuthenticationOptions");
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = authConfig["BrowserCookieName"];
        options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(authConfig["CookieExpirationTime"]));
        options.SlidingExpiration = true;
        options.LoginPath = authConfig["DefaultLoginPath"];
        options.AccessDeniedPath = authConfig["DefaultAccessDeniedPath"];
    });

// set up authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Ban", policy =>
        policy.Requirements.Add(new AuthRequirement(0)));
    options.AddPolicy("UserManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(1)));
    options.AddPolicy("BlackboardManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(2)));
    options.AddPolicy("TrainManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(4)));
    options.AddPolicy("TrainNotesManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(5)));
    options.AddPolicy("BlackboardPostingPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(6)));
});

// add other services
builder.Services.AddSingleton<IAuthorizationHandler, AuthRequirementHandler>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

UserValidationService.ConfigureServise(builder.Configuration);
builder.Services.AddSingleton<UserValidationService>();

LoggerService.configureService(builder.Configuration);
builder.Services.AddSingleton<LoggerService>();
DbConnectionService.configureService(builder.Configuration);
builder.Services.AddSingleton<DbConnectionService>();
builder.Services.AddTransient<SQLService>();
builder.Services.AddTransient<BlackBoardService>();


builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<AccountManagerService>();
builder.Services.AddTransient<BlackBoardService>();
builder.Services.AddTransient<TrainManagerService>();
builder.Services.AddTransient<WagonManagerService>();
builder.Services.AddTransient<TemplateManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapDefaultControllerRoute();

app.Run();

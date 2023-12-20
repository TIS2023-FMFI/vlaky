using code.Auth;
using code.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.s

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "TrainCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Login/";
        options.AccessDeniedPath = "/Forbidden/";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(1)));
    options.AddPolicy("BlackboardManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(2)));
    options.AddPolicy("TemplateCreationPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(3)));
    options.AddPolicy("TrainManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(4)));
    options.AddPolicy("TrainNotesManagementPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(5)));
    options.AddPolicy("BlackboardPostingPolicy", policy =>
        policy.Requirements.Add(new AuthRequirement(6)));
});

builder.Services.AddSingleton<IAuthorizationHandler, AuthRequirementHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

DbConnectionService.configureService(builder.Configuration);
builder.Services.AddSingleton<DbConnectionService>();
builder.Services.AddTransient<SQLService>();

builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddTransient<AccountManagerService>();

builder.Services.AddServerSideBlazor();

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

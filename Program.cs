using code.Services;
using code.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.s
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddTransient<AccountManagerService>();
builder.Services.AddTransient<BlackBoardService>();

builder.Services.AddServerSideBlazor();

DbConnectionService.configureService(builder.Configuration);
builder.Services.AddSingleton<DbConnectionService>();
builder.Services.AddTransient<SQLService>();

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

app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();

app.Run();

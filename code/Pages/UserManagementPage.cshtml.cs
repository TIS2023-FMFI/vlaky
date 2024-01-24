using code.Models;
using code.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using code.Auth;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace code.Pages;

[Authorize(Policy = "Ban")]
[Authorize(Policy = "UserManagementPolicy")]

public class ManageModel : PageModel
{
    private readonly ILogger<ManageModel> _logger;
    
    public bool allow;

    public ManageModel(ILogger<ManageModel> logger)
    {
        _logger = logger;
        allow = false;
    }
    

    public Account? Acc{get;private set;}
    public async Task OnGet()
    {

        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            await HttpContext.ChallengeAsync();
        }

    }


}


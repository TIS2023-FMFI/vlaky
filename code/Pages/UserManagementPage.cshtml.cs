using code.Models;
using code.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using code.Auth;
using System.Data;

namespace code.Pages;

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
        var a = HttpContext.User.Claims.First(x => x.Type=="Privileges");
        var b = AuthRequirementHandler.isBitSet(a.Value,1);//index od nuly
        if(b){
            allow = true;
        }

    }


}


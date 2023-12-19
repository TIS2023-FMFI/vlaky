using code.Models;
using code.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace code.Pages;

public class ManageModel : PageModel
{
    private readonly ILogger<ManageModel> _logger;
    public Account Curr{get;set;}

    public ManageModel(ILogger<ManageModel> logger, Account C)
    {
        _logger = logger;
        Curr = C;
    }
    

    public Account? Acc{get;private set;}
    public void OnGet()
    {

        Curr.Id = 1;
        Curr.Privileges = 1;
        //Response.Cookies.Append("priv", "1");
    }

}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using code.Services;
using code.Models;
using System.Linq;

namespace code.Pages
{
    public class TemplatesModel : PageModel
    {
        private readonly TemplateManagerService _TemplateManagerService;
        private readonly ILogger<TemplatesModel> _logger;

        public List<TrainTemplate> Templates { get; set; }

        public TemplatesModel(ILogger<TemplatesModel> logger, TemplateManagerService templateManagerService)
        {
            _logger = logger;
            _TemplateManagerService = templateManagerService;
            Templates = new List<TrainTemplate>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            var templateId = HttpContext.Request.Query["templateId"].ToString();
            if (!string.IsNullOrEmpty(templateId))
            {
                await _TemplateManagerService.RemoveTemplateById(Convert.ToInt32(templateId));
            }

            Templates = await _TemplateManagerService.GetTemplates();

            return Page();
        }
    }
}

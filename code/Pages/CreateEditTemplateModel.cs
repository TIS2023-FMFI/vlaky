using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using code.Services;
using code.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace code.Pages
{
    public class CreateEditTemplateModel : PageModel
    {
        private readonly TemplateManagerService _templateManagerService;
        private readonly ILogger<CreateEditTemplateModel> _logger;

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditTemplateModel(TemplateManagerService templateManagerService, ILogger<CreateEditTemplateModel> logger)
        {
            _templateManagerService = templateManagerService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int? templateId)
        {
            ErrorMessage = null;

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            if (templateId.HasValue)
            {
                var template = await _templateManagerService.GetTemplateById(templateId.Value);
                if (template != null)
                {
                    Name = template.Name;
                    Destination = template.Destination;
                }
                else
                {
                    return RedirectToPage("/Templates");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Destination))
            {
                ErrorMessage = "Vyplňte všetky polia.";
                return Page();
            }

            var templateId = HttpContext.Request.Query["templateId"].ToString();
            var newTemplate = new TrainTemplate
            {
                Name = Name,
                Destination = Destination
            };

            if (!string.IsNullOrEmpty(templateId))
            {
                newTemplate.Id = Convert.ToInt32(templateId);
                await _templateManagerService.UpdateTemplate(newTemplate);
            }
            else
            {
                await _templateManagerService.AddTemplate(newTemplate);
            }

            return RedirectToPage("/Templates");
        }
    }
}

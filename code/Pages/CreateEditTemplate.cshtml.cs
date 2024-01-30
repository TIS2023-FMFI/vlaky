using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using code.Services;
using code.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace code.Pages
{
    [Authorize(Policy = "TrainManagementPolicy")]
    public class CreateEditTemplateModel : PageModel
    {
        private readonly TemplateManagerService _templateManagerService;
        private readonly ILogger<CreateEditTemplateModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditTemplateModel(TemplateManagerService templateManagerService, 
            ILogger<CreateEditTemplateModel> logger,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _templateManagerService = templateManagerService;
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
        }

        public async void OnGetAsync(int? templateId)
        {
            if (await _userValidationService.IsUserInvalid(HttpContext)) {return;}

            ErrorMessage = null;

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
                    RedirectToPage("/Templates");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _userValidationService.IsUserInvalid(HttpContext))
            {
                return Redirect("/Login");
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Destination))
            {
                ErrorMessage = "Vyplňte všetky polia.";
                return Page();
            }

            var allTemplates = await _templateManagerService.GetTemplates();

            if (allTemplates == null) 
            {
                allTemplates = new List<TrainTemplate>();
            }


            foreach (var tt in allTemplates)
            {
                if (tt.Name == Name)
                {
                    ErrorMessage = "Šablóna s týmto názvom už existuje.";
                    return Page();
                }
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

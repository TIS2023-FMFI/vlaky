using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using code.Services;
using code.Models;

namespace code.Pages
{
    public class DaySchedule
    {
        public DateTime Date { get; set; }
        public List<Train> Trains { get; set; }
    }

    public class ScheduleModel : PageModel
    {
        private readonly TrainManagerService _TrainManagerService;
        private readonly ILogger<ScheduleModel> _logger;

        public List<DaySchedule> Schedule { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ScheduleModel(TrainManagerService trainManagerService, ILogger<ScheduleModel> logger)
        {
            _TrainManagerService = trainManagerService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(DateTime? startDate, DateTime? endDate)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Schedule = new List<DaySchedule>();
            StartDate = startDate ?? DateTime.Today;
            EndDate = endDate ?? StartDate.AddDays(7);

            var allTrainsByDate = await _TrainManagerService.GetTrainsByDate(StartDate, EndDate);

            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var daySchedule = new DaySchedule
                {
                    Date = date,
                    Trains = allTrainsByDate
                        .Where(t => t.Date.Date == date.Date)
                        .OrderBy(t => t.Id)
                        .ToList()
                };

                Schedule.Add(daySchedule);
            }

            return Page();
        }

        public string GetTrainStatusImage(int status)
        {
            switch (status)
            {
                case 0: return "~/images/train_naplanovany.svg";
                case 1: return "~/images/train_vpriprave.svg";
                case 2: return "~/images/train_pripraveny.svg";
                case 3: return "~/images/train_expedovany.svg";
                case 4: return "~/images/train_zruseny.svg";
                default: return "~/images/train_default.svg";
            }
        }

        public string EditDateName(string name)
        {
            switch (name.ToLower())
            {
                case "pondelok": return "Pondelok";
                case "utorok": return "Utorok";
                case "streda": return "Streda";
                case "štvrtok": return "Štvrtok";
                case "piatok": return "Piatok";
                case "sobota": return "Sobota";
                case "nedeľa": return "Nedeľa";
                default: return "";
            }
        }
    }
}

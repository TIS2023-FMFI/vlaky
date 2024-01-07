using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using code.Models;
using code.Services;

namespace code.Pages
{
    public class HistoryModel : PageModel
    {
        private readonly TrainManagerService _trainManagerService;
        private readonly ILogger<HistoryModel> _logger;

        public List<DaySchedule> Schedule { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TrainsInState3Count { get; set; }
        public int TrainsInState4Count { get; set; }
        public int TotalTrainsCount { get; set; }

        public HistoryModel(TrainManagerService trainManagerService, ILogger<HistoryModel> logger)
        {
            _trainManagerService = trainManagerService;
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

            var allTrainsByDate = await _trainManagerService.GetTrainsByDate(StartDate, EndDate);
            TrainsInState3Count = allTrainsByDate.Count(t => t.Status == 3);
            TrainsInState4Count = allTrainsByDate.Count(t => t.Status == 4);
            TotalTrainsCount = TrainsInState3Count + TrainsInState4Count;

            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var daySchedule = new DaySchedule
                {
                    Date = date,
                    Trains = allTrainsByDate
                        .Where(t => t.Date.Date == date.Date && (t.Status == 3 || t.Status == 4))
                        .OrderBy(t => t.Id)
                        .ToList()
                };

                Schedule.Add(daySchedule);
            }

            return Page();
        }

        public string GetTrainStatusImage(int status)
        {
            switch(status)
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

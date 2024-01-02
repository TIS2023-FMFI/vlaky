using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;


namespace code.Pages;

public class ScheduleModel : PageModel
{
    public List<DaySchedule> Schedule { get; set; }

    public void OnGet()
    {
        // Populate the Schedule property by fetching data from the database
    }
}

public class DaySchedule
{
    public DateTime Date { get; set; }
    public List<TrainSchedule> Trains { get; set; }
}

public class TrainSchedule
{
    public string Name { get; set; }
    public string Availability { get; set; }
    // Add other properties as needed
}

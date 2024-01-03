using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using code.Services;
using System.Linq;

namespace code.Pages;

public class NNote
{
    public int Id { get; set; }
    public int Priority { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public string User { get; set; }
}

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BlackBoardService _blackBoardService;

    public List<NNote> Notes { get; set; }

    public IndexModel(ILogger<IndexModel> logger, 
        BlackBoardService blackBoardService)
    {
        _logger = logger;
        _blackBoardService = blackBoardService;
        Notes = new List<NNote>();
    }



    public async Task OnGetAsync()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            Response.Redirect("/Login");
            return;
        }



        var noteId = HttpContext.Request.Query["noteId"].ToString();
        if (!string.IsNullOrEmpty(noteId)) {
            await _blackBoardService.RemoveNoteById(Convert.ToInt32(noteId));
        }

        

        var blackBoardNotes = await _blackBoardService.GetNotes();
        Notes = blackBoardNotes
            .Select(bbn => new NNote
            {   
                Id = bbn.Id,
                Priority = bbn.Priority,
                Title = bbn.Title,
                Content = bbn.Text,
                Date = bbn.Date.ToString("dd/MM/yy"),
                User = bbn.UserId.ToString()
            })
            .OrderBy(note => note.Priority)
            .ToList();
    }
}

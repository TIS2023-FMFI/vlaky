using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using code.Services;
using code.Models;
using System.Linq;

namespace code.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BlackBoardService _blackBoardService;

        public List<BlackBoardNote> Notes { get; set; }

        public IndexModel(ILogger<IndexModel> logger,
            BlackBoardService blackBoardService)
        {
            _logger = logger;
            _blackBoardService = blackBoardService;
            Notes = new List<BlackBoardNote>();
        }

        public async Task OnGetAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Login");
                return;
            }

            var noteId = HttpContext.Request.Query["noteId"].ToString();
            if (!string.IsNullOrEmpty(noteId))
            {
                await _blackBoardService.RemoveNoteById(Convert.ToInt32(noteId));
            }

            var blackBoardNotes = await _blackBoardService.GetNotes();
            Notes = blackBoardNotes
                .Select(note => new BlackBoardNote
                {
                    Id = note.Id,
                    Priority = note.Priority,
                    Title = note.Title,
                    Text = note.Text,
                    Date = note.Date,
                    UserId = note.UserId
                })
                .OrderBy(note => note.Priority)
                .ToList();
        }
    }
}

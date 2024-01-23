using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using code.Services;
using code.Models;
using System.Linq;
using System.Security.Claims;

namespace code.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlackBoardService _blackBoardService;
        private readonly AccountManagerService _accountManagerService;
        private readonly ILogger<IndexModel> _logger;
        private readonly LoggerService _loggerService;

        public List<BlackBoardNote> Notes { get; set; }

        public string UserName { get; private set; }

        public IndexModel(ILogger<IndexModel> logger,
            BlackBoardService blackBoardService,
            AccountManagerService accountManagerService, LoggerService loggerService)
        {
            _logger = logger;
            _accountManagerService = accountManagerService;
            _blackBoardService = blackBoardService;
            _loggerService = loggerService;
            Notes = new List<BlackBoardNote>();
        }

        public async Task OnGetAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Login");
                return;
            }
            else
            {
                UserName = User.FindFirst(ClaimTypes.Name)?.Value;
            }

            var noteId = HttpContext.Request.Query["noteId"].ToString();
            if (!string.IsNullOrEmpty(noteId))
            {
                var note = await _blackBoardService.GetNoteById(Convert.ToInt32(noteId));
                await _blackBoardService.RemoveNoteById(Convert.ToInt32(noteId));
                _loggerService.writeCommDelete(HttpContext, note);
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

        public async Task<string> GetAccountName(int userId)
        {
            var accountName = await _accountManagerService.GetUserNameById(userId);
            if (accountName!= null)
            {
                return accountName;
            }
            return "vymazaný používateľ";
        }
    }
}

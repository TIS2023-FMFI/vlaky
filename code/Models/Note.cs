using code.Services;
using Npgsql;

namespace code.Models
{
    public abstract class Note
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public abstract Task UpdateText(string new_text, SQLService s);
        public abstract Task DeleteSelf(SQLService s);
    }
}
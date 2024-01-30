using Npgsql;
using code.Models;
using code.Services;

public class BlackBoardService
{
    private SQLService s;

    public BlackBoardService(SQLService ns)
    {
        s = ns;
    }

    public async Task AddNote(BlackBoardNote n)
    {
        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("user_id", n.UserId),
            new NpgsqlParameter("text", n.Text),
            new NpgsqlParameter("date", n.Date),
            new NpgsqlParameter("priority", n.Priority),
            new NpgsqlParameter("title", n.Title)
        };
        
        try
        {
            (await s.sqlCommand(
                "INSERT INTO board_comments (user_id, text, date, priority, title) VALUES (@user_id, @text, @date, @priority, @title)",
                parameters
            )).Close();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<BlackBoardNote>> GetNotes()
    {
        var notes = new List<BlackBoardNote>();
        var parameters = new List<NpgsqlParameter>();
        using (MyReader myreader = await s.sqlCommand("SELECT * FROM board_comments", parameters))
        {
            NpgsqlDataReader reader = myreader.Reader;
            while (reader.Read())
            {
                var a = new BlackBoardNote
                {
                    Id = (int)reader["id"],
                    Title = (string)reader["title"],
                    UserId = (int)reader["user_id"],
                    Text = (string)reader["text"],
                    Date = (DateTime)reader["date"],
                    Priority = (int)reader["priority"]
                };
                notes.Add(a);
            }
        }
        return notes;
    }

    public async Task<BlackBoardNote> GetNoteById(int id)
    {
        var parameters = new List<NpgsqlParameter> { new NpgsqlParameter("id", id) };
        using (
            MyReader myreader = await s.sqlCommand(
                "SELECT * FROM board_comments WHERE id = @id",
                parameters
            )
        )
        {
            NpgsqlDataReader reader = myreader.Reader;
            if (reader.Read())
            {
                var a = new BlackBoardNote
                {
                    Id = (int)reader["id"],
                    Title = (string)reader["title"],
                    UserId = (int)reader["user_id"],
                    Text = (string)reader["text"],
                    Date = (DateTime)reader["date"],
                    Priority = (int)reader["priority"]
                };
                return a;
            }
            return null;
        }
    }

    public async Task RemoveNoteById(int noteId)
    {
        var parameters = new List<NpgsqlParameter> { new NpgsqlParameter("id", noteId) };

        (await s.sqlCommand("DELETE FROM board_comments WHERE id = @id", parameters)).Close();
    }

    public async Task EditNote(BlackBoardNote updatedNote)
    {
        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("id", updatedNote.Id),
            new NpgsqlParameter("title", updatedNote.Title),
            new NpgsqlParameter("user_id", updatedNote.UserId),
            new NpgsqlParameter("date", updatedNote.Date),
            new NpgsqlParameter("text", updatedNote.Text),
            new NpgsqlParameter("priority", updatedNote.Priority)
        };

        (await s.sqlCommand(
            "UPDATE board_comments SET title = @title, user_id = @user_id, date = @date, text = @text, priority = @priority WHERE id = @id",
            parameters
        )).Close();
    }
}

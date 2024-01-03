using code.Services;
using Npgsql;

namespace code.Models
{
    public class BlackBoardNote : Note
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Title { get; set; }

        public BlackBoardNote() { }

        public override async Task UpdateText(string new_text, SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("text", new_text),
                new NpgsqlParameter("date", DateTime.Now),
                new NpgsqlParameter("user_id", UserId),
                new NpgsqlParameter("id", Id),
            };

            await s.sqlCommand("UPDATE board_comments SET text = @text, date = @date WHERE user_id = @user_id AND id = @id", parameters);
        }

        public override async Task DeleteSelf(SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("user_id", UserId),
                new NpgsqlParameter("id", Id)
            };
            await s.sqlCommand("DELETE FROM board_comments WHERE user_id = @user_id AND id = @id", parameters);
        }

        public async Task SetPriority(int newPriority, SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("priority", newPriority),
                new NpgsqlParameter("user_id", UserId),
                new NpgsqlParameter("id", Id),
            };
            Priority = newPriority;
            await s.sqlCommand("UPDATE board_comments SET priority = @priority WHERE user_id = @user_id AND id = @id", parameters);
        }
    }
}
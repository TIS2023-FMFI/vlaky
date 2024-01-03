using code.Services;
using Npgsql;

namespace code.Models
{
    public class TrainNote : Note
    {
        public int TrainId { get; set; }

        public TrainNote() { }

        public override async Task UpdateText(string new_text, SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("text", new_text),
                new NpgsqlParameter("user_id", UserId),
                new NpgsqlParameter("train_id", TrainId)
            };
            await s.sqlCommand("UPDATE train_comments SET text = @text WHERE user_id = @user_id AND train_id = @train_id", parameters);
        }

        public override async Task DeleteSelf(SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("user_id", UserId),
                new NpgsqlParameter("train_id", TrainId)
            };
            await s.sqlCommand("DELETE FROM train_comments WHERE user_id = @user_id AND train_id = @train_id", parameters);
        }
    }
}
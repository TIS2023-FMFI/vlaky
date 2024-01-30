using code.Services;
using Npgsql;

namespace code.Models
{
    public class WagonNote:Note{
        public int WagonId{get;set;}
        public string UserName{get;set;}
        public WagonNote(){}
        public override async Task DeleteSelf(SQLService s){
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("user_id",UserId), 
                new NpgsqlParameter("wagon_id",WagonId)
            };
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM wagon_comments WHERE user_id = ($1) AND wagon_id = ($2)", parameters);
            reader.Close();
        }
        public override async Task UpdateText(string new_text,SQLService s)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("text",new_text),
                new NpgsqlParameter("user_id",UserId),
                new NpgsqlParameter("wagon_id",WagonId),
            };
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE wagon_comments SET text = ($1) WHERE user_id = ($2) AND wagon_id = ($3)", parameters);
            reader.Close();
        }
    }
}
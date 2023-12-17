using code.Models;
using code.Services;
using Npgsql;

namespace code.Services{
    public class BlackBoardService{
            public BlackBoardService(){}
            SQLService s;
            public void AddNote(BlackBoardNote n,SQLService ns){
                s = ns;
                IEnumerable<NpgsqlParameter>parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",n.Id));
                parameters.Append(new NpgsqlParameter("user_id",n.UserId));
                parameters.Append(new NpgsqlParameter("text",n.Text));
                parameters.Append(new NpgsqlParameter("date",n.Date));
                parameters.Append(new NpgsqlParameter("priority",n.Priority));
                NpgsqlDataReader reader = s.sqlCommand("INSERT INTO board_comments (id,user_id,text,date,priority) VALUES (($1),($2),($3),($4),($5))", parameters);
                reader.Close();
            }
            public void RemoveNote(BlackBoardNote n){
                n.DeleteSelf(s);
            }
            public List<BlackBoardNote> GetNotes(){
                List<BlackBoardNote>Notes = new List<BlackBoardNote>();
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM board_notes", parameters);

                while(reader.Read()){
                    var a = new BlackBoardNote();
                    a.Id = (int)reader[0];
                    a.UserId = (int)reader[1];
                    a.Text = (string)reader[2];
                    a.Date = (DateTime)reader[3];
                    a.Priority = (int)reader[4];
                    Notes.Add(a);
                }
                reader.Close();
                return Notes;
            }

        }
}
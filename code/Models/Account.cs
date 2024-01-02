using code.Services;
using Npgsql;

namespace code.Models
{
    public class Account{
        public Account(){}
        public Account(int id,string name,string mail, int privileges,string pass){
            Id = id;
            Name = name;
            Mail = mail;
            Privileges = privileges;
            Pass = pass;
        }
        public int Id{get;set;}
        public string Name{get;set;}
        public string Mail{get;set;}
        public int Privileges{get;set;}
        public string Pass{get;set;}
        public async void DeleteSelf(SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("p1",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM users WHERE id = (@p1)", parameters);
            reader.Close();
        }
        public async void UpdatePassWord(string new_passs,SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("p1",new_passs),new NpgsqlParameter("p2",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE users SET pass = sha256((@p1)::bytea) WHERE id = (@p2)", parameters);
            reader.Close();
        }
        public async void UpdateMail(string new_mail,SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("p1",new_mail),new NpgsqlParameter("p2",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE users SET mail = (@p1) WHERE id = (@p2)", parameters);
            reader.Close();
        }
        public async void UpdatePrivileges(int privs,SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("p1",privs),new NpgsqlParameter("p2",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE users SET privileges = (@p1) WHERE id = (@p2)", parameters);
            reader.Close();
        }

    }
}
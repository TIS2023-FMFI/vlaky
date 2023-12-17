using Npgsql;
using code.Models;
using code.Services;

namespace code.Services{
    public class AccountManager{
            List<Account>Accounts = new List<Account>();
            SQLService s;
            public AccountManager(SQLService ns){
                //nacitame Accounts do listu
                s = ns;
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM users", parameters);

                while(reader.Read()){
                    var a = new Account
                    {
                        Id = (int)reader[0],
                        Name = (string)reader[1],
                        Mail = (string)reader[2],
                        Privileges = (int)reader[3],
                        Pass = (string)reader[4]
                    };
                    Accounts.Add(a);
                }
                reader.Close();
            }
            public void AddAccount(Account n){
                Accounts.Add(n);
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",n.Id));
                parameters.Append(new NpgsqlParameter("name",n.Name));
                parameters.Append(new NpgsqlParameter("mail",n.Mail));
                parameters.Append(new NpgsqlParameter("privileges",n.Privileges));
                parameters.Append(new NpgsqlParameter("pass",n.Pass));
                NpgsqlDataReader reader = s.sqlCommand("INSERT INTO users (id, name, mail, privileges, pass) VALUES (($1),($2),($3),($4),($5))", parameters);
                reader.Close();
            }
            public void RemoveAccount(int id){
                foreach(var u in Accounts){
                    if(u.Id==id){
                        Accounts.Remove(u);
                    }
                }
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",id));
                NpgsqlDataReader reader = s.sqlCommand("DELETE FROM users WHERE id = ($1)", parameters);
                reader.Close();
            }

            public List<Account> GetAccounts(){
                return Accounts;
            }

        }
}
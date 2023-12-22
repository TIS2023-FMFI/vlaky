using Npgsql;
using code.Models;
using code.Services;
using System.Text;
using System.Security.Cryptography;

namespace code.Services{
    public class AccountManagerService{
            SQLService s;
            public AccountManagerService(SQLService ns){
                s = ns;
            }
            public async void AddAccount(Account n){

                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("p1", n.Name),
                    new NpgsqlParameter("p2", n.Mail),
                    new NpgsqlParameter("p3", n.Privileges),
                    new NpgsqlParameter("p4", n.Pass)
                };

                NpgsqlDataReader reader = await s.sqlCommand("INSERT INTO users (name, mail, privileges, pass) VALUES ((@p1),(@p2),(@p3),sha256((@p4)::bytea))", parameters);
                reader.Close();
            }
            public async void RemoveAccount(int id){
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("p1", id)
                };
                NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM users WHERE id = (@p1)", parameters);
                reader.Close();
            }

            public async Task<List<Account>> GetAccounts(){
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
                NpgsqlDataReader reader = await s.sqlCommand("SELECT * FROM users", parameters);
                List<Account>Accounts = new List<Account>();
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
                return Accounts;
            }

            public async Task<List<Account>> GetAccounts(string mail, string pass){
                IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("p1", mail),
                    new NpgsqlParameter("p2", Encoding.UTF8.GetBytes(pass))
                };
                NpgsqlDataReader reader = await s.sqlCommand(
                    "SELECT * FROM users WHERE mail = (@p1) AND pass::bytea = sha256((@p2))", 
                    parameters);
                List<Account>Accounts = new List<Account>();
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
                return Accounts;
            }

            public async Task<Account?> GetAccount(string mail, string pass)
            {
                List<Account> Accounts = await GetAccounts(mail, pass);

                if (Accounts.Count != 1) {
                    return null;
                }
                
                return Accounts.FirstOrDefault();
            }
        }
}
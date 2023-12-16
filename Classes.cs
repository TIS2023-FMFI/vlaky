using System;
using code.Services;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Npgsql;

namespace code.Models
{
    public class Account{
        public Account(){}
        public int Id{get;set;}
        public string Name{get;set;}
        public string Mail{get;set;}
        public int Privileges{get;set;}
        public string Pass{get;set;}

    }

    public class AccountManager{
        List<Account>Accounts = new List<Account>();
        SQLService s = new SQLService(new DbConnectionService());
        public AccountManager(){
            //nacitame Accounts do listu
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
            NpgsqlDataReader reader = s.sqlCommand("INSERT INTO users (id, name, mail, privileges, pass) VALUES ($1,$2,$3,$4,$5)", parameters);
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
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM users WHERE id = $1", parameters);
            reader.Close();
        }

        public List<Account> GetAccounts(){
            return Accounts;
        }

    }

    public abstract class Note(){
        public int UserId{get;set;}
        public string Text{get;set;}
        public SQLService s = new SQLService(new DbConnectionService());
        public abstract void UpdateText(string new_text);
        public abstract void DeleteSelf();
    }

    public class TrainNote:Note{
        public int TrainId{get;set;}
        public TrainNote(int tid,int uid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",uid));
            parameters.Append(new NpgsqlParameter("train_id",tid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM train_comments WHERE user_id = $1 AND train_id = $2", parameters);
            if(reader.HasRows){
                reader.Read();
                TrainId = tid;
                UserId = uid;
                Text = "";
                reader.Close();
            }else{
                reader.Close();
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("train_id",tid));
                parameters.Append(new NpgsqlParameter("user_id",uid));
                parameters.Append(new NpgsqlParameter("text",""));
                reader = s.sqlCommand("INSERT INTO train_comments (train_id,user_id,text) VALUES ($1,$2,$3)", parameters);
                reader.Close();
            }
        }
        
        public override void DeleteSelf(){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM train_comments WHERE user_id = $1 AND train_id = $2", parameters);
            reader.Close();
        }

        public override void UpdateText(string new_text)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE train_comments SET text = $1 WHERE user_id = $2 AND train_id = $3", parameters);
            reader.Close();
        }
    }
    public class WagonNote:Note{
        public int WagonId{get;set;}
        public WagonNote(int vid,int uid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",uid));
            parameters.Append(new NpgsqlParameter("wagon_id",vid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM wagon_comments WHERE user_id = $1 AND wagon_id = $2", parameters);
            if(reader.HasRows){
                reader.Read();
                WagonId = (int)reader[0];
                UserId = (int)reader[1];
                Text = (string)reader[3];
                reader.Close();
            }else{
                reader.Close();
                WagonId = vid;
                UserId = uid;
                Text = "";
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("wagon_id",vid));
                parameters.Append(new NpgsqlParameter("user_id",uid));
                parameters.Append(new NpgsqlParameter("text",""));
                reader = s.sqlCommand("INSERT INTO wagon_comments (wagon_id,user_id,text) VALUES ($1,$2,$3)", parameters);
                reader.Close();
            }
        }
        public override void DeleteSelf(){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("wagon_id",WagonId));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM wagon_comments WHERE user_id = $1 AND wagon_id = $2", parameters);
            reader.Close();
        }
        public override void UpdateText(string new_text)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("wagon_id",WagonId));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagon_comments SET text = $1 WHERE user_id = $2 AND wagon_id = $3", parameters);
            reader.Close();
        }
    }



    public class BlackBoardNote:Note{
    
        public DateTime Date{get;set;}
        int Id{get;set;}
        public int Priority{get;set;}

        public BlackBoardNote(int nid,int uid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("id",nid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM board_comments WHERE id = $1", parameters);
            if(reader.HasRows){
                reader.Read();
                Id = (int)reader[0];
                UserId = (int)reader[1];
                Text = (string)reader[2];
                Date = (DateTime)reader[3];
                Priority = (int)reader[4];
                reader.Close();
            }else{
                reader.Close();
                Id = nid;
                UserId = uid;
                Text = "";
                Date = DateTime.Now;
                Priority = 5;
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",nid));
                parameters.Append(new NpgsqlParameter("user_id",UserId));
                parameters.Append(new NpgsqlParameter("text",""));
                parameters.Append(new NpgsqlParameter("date",DateTime.Now));
                parameters.Append(new NpgsqlParameter("priority",5));
                reader = s.sqlCommand("INSERT INTO board_comments (id,user_id,text,date,priority) VALUES ($1,$2,$3,$4,$5)", parameters);
                reader.Close();
            }
        }

        public override void DeleteSelf()
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM board_comments WHERE user_id = $1 AND id = $2", parameters);
            reader.Close();
        }

        public void SetPriority(int np){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("priority",np));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE board_comments SET priority = $1 WHERE user_id = $2 AND id = $3", parameters);
            reader.Close();
        }
        public int GetPriority(){
            return Priority;
        }

        public override void UpdateText(string new_text)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("date",Date));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            Date = DateTime.Now;
            NpgsqlDataReader reader = s.sqlCommand("UPDATE board_comments SET text = $1,date = $2 WHERE user_id = $3 AND id = $4", parameters);
            reader.Close();
        }
    }

    public class BlackBoard{
        List<BlackBoardNote>Notes = new List<BlackBoardNote>();
        SQLService s = new SQLService(new DbConnectionService());
        public BlackBoard(){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM board_notes", parameters);

            while(reader.Read()){
                var a = new BlackBoardNote((int)reader[0],(int)reader[1]);
                Notes.Add(a);
            }
            reader.Close();
        }
        public void AddNote(BlackBoardNote n){
            Notes.Add(n);
        }
        public void RemoveNote(BlackBoardNote n){
            Notes.Remove(n);
            n.DeleteSelf();
        }
        public List<BlackBoardNote> GetNotesFromUser(int uid){
            List<BlackBoardNote>Notes2 = new List<BlackBoardNote>();
            foreach(var n in Notes){
                if(n.UserId==uid){
                    Notes2.Add(n);
                }
            }
            return Notes2;
        }
        public List<BlackBoardNote> GetNotes(){
            return Notes;
        }

    }

    public class Wagon{
        int State{get;set;}
        public int Id{get;set;}
        public int TrainId{get;set;}
        int NOrder{get;set;}
        SQLService s = new SQLService(new DbConnectionService());
        public Wagon(int vid,int tid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("id",vid));
            parameters.Append(new NpgsqlParameter("train_id",tid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM wagons WHERE id = $1 AND train_id = $2", parameters);
            if(reader.HasRows){
                reader.Read();
                Id = (int)reader[0];
                TrainId = (int)reader[1];
                NOrder = (int)reader[2];
                State = (int)reader[3];
                reader.Close();
            }else{
                reader.Close();
                Id = vid;
                TrainId = tid;
                NOrder = -1;
                State = -1;
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",vid));
                parameters.Append(new NpgsqlParameter("train_id",tid));
                parameters.Append(new NpgsqlParameter("n_order",-1));
                parameters.Append(new NpgsqlParameter("state",-1));
                reader = s.sqlCommand("INSERT INTO wagons (id,user_id,text,date,priority) VALUES ($1,$2,$3,$4,$5)", parameters);
                reader.Close();
            }
        }
        public void setNOrder(int no){
            NOrder = no;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("n_order",no));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagons SET n_order = $1 WHERE train_id = $2 AND id = $3", parameters);
            reader.Close();
        }
        public int getNOrder(){
            return NOrder;
        }

        public void setState(int nstatus){
            State = nstatus;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("state",nstatus));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagons SET state = $1 WHERE train_id = $2 AND id = $3", parameters);
            reader.Close();
        }
        public int getState(){
            return State;
        }
        
    }

    public class TrainTemplate{
        SQLService s = new SQLService(new DbConnectionService());
        public int Id{get;set;}
        string Name{get;set;}
        double MaxLength{get;set;}
        string Destination{get;set;}
        public TrainTemplate(int nid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("id",nid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM wagons WHERE id = $1", parameters);
            if(reader.HasRows){
                reader.Read();
                Id = (int)reader[0];
                Name = (string)reader[1];
                Destination = (string)reader[2];
                MaxLength = (int)reader[3];
                reader.Close();
            }else{
                reader.Close();
                Id = nid;
                Name = "";
                Destination = "";
                MaxLength = -1.0;
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",nid));
                parameters.Append(new NpgsqlParameter("name",""));
                parameters.Append(new NpgsqlParameter("destination",""));
                parameters.Append(new NpgsqlParameter("max_leanght",-1.0));
                reader = s.sqlCommand("INSERT INTO templates (id,name,destination,max_leanght) VALUES ($1,$2,$3,$4)", parameters);
                reader.Close();
            }
        }
        public void SetName(string nn){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("name",nn));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE templates SET name = $1 WHERE id = $2", parameters);
            reader.Close();

        }
        public string GetName(){
            return Name;
        }
        public void SetMaxLength(double nl){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("max_leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE templates SET max_leanght = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public double GetMaxLenght(){
            return MaxLength;
        }

    }

    public class Train{
        List<Wagon>Wagons = new List<Wagon>();
        SQLService s = new SQLService(new DbConnectionService());
        public int Id{get;set;}
        string Name{get;set;}
        bool Coll{get;set;}
        string Destination{get;set;}
        double MaxLength{get;set;}
        double Lenght{get;set;}
        DateTime Date{get;set;}
        int Status{get;set;}
        
        public Train(int nid){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("id",nid));
            NpgsqlDataReader reader = s.sqlCommand("SELECT * FROM wagons WHERE id = $1", parameters);
            if(reader.HasRows){
                reader.Read();
                Id = (int)reader[0];
                Name = (string)reader[1];
                Destination = (string)reader[2];
                Status = (int)reader[3];
                Date = (DateTime)reader[4];
                Coll = (bool)reader[5];
                //nwagons skip
                MaxLength = (int)reader[7];
                Lenght = (double)reader[8];
                reader.Close();
            }else{
                reader.Close();
                Id = nid;
                Name = "";
                Destination = "";
                Status = -1;
                Date = DateTime.Now;
                Coll = false;
                //nwagons skip
                MaxLength = -1;
                Lenght = -1;
                parameters = new List<NpgsqlParameter>();
                parameters.Append(new NpgsqlParameter("id",nid));
                parameters.Append(new NpgsqlParameter("name",""));
                parameters.Append(new NpgsqlParameter("destination",""));
                parameters.Append(new NpgsqlParameter("status",-1));
                parameters.Append(new NpgsqlParameter("date",DateTime.Now));
                parameters.Append(new NpgsqlParameter("coll",false));
                parameters.Append(new NpgsqlParameter("max_leanght",-1.0));
                parameters.Append(new NpgsqlParameter("leanght",-1.0));
                reader = s.sqlCommand("INSERT INTO templates (id,name,destination,max_leanght) VALUES ($1,$2,$3,$4)", parameters);
                reader.Close();
            }
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("train_id",Id));
            reader = s.sqlCommand("SELECT * FROM wagons WHERE train_id = $1", parameters);

            while(reader.Read()){
                var a = new Wagon((int)reader[0],Id);
                Wagons.Add(a);
            }

            reader.Close();

        }

        public void AddVagon(Wagon w){
            Wagons.Add(w);
        }
        public void SeColl(bool nd){
            Coll = nd;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("coll",nd));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET coll = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public bool GetColl(){
            return Coll;

        }
        public void SetDestination(string nd){
            Destination = nd;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("destination",nd));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET destination = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public string GetDestination(){
            return Destination;
        }
        public void SetDate(DateTime nl){
            Date = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("date",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET date = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public DateTime GetDate(){
            return Date;
        }
        public void SetStatus(int ns){
            Status = ns;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("state",ns));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET state = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public int getStatus(){
            return Status;
        }
        public void SetName(string nn){
            Name = nn;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("name",nn));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET name = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public string GetName(){
            return Name;
        }

        public void SetMaxLenght(double nl){
            MaxLength = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("max_leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET max_leanght = $1 WHERE id = $2", parameters);
            reader.Close();
        }
        public double GetMaxLenght(){
            return MaxLength;
        }

        public void SetLenght(double nl){
            Lenght = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET leanght = $1 WHERE id = $2", parameters);
            reader.Close();
        }

        public double GetLenght(){
            return Lenght;
        }

    }
    
}
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
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM users WHERE id = ($1)", parameters);
            reader.Close();
        }
        public async void UpdatePassWord(string new_passs,SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("pass",new_passs),new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE users SET pass = sha256(($1)) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void UpdateMail(string new_mail,SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("pass",new_mail),new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE users SET mail = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

    }

    
    public abstract class Note(){
        public int UserId{get;set;}
        public string Text{get;set;}
        public abstract void UpdateText(string new_text,SQLService s);
        public abstract void DeleteSelf(SQLService s);
    }

    public class TrainNote:Note{
        public int TrainId{get;set;}
        public TrainNote(){}
        
        public override async void DeleteSelf(SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("user_id",UserId), new NpgsqlParameter("train_id",TrainId)];
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM train_comments WHERE user_id = ($1) AND train_id = ($2)", parameters);
            reader.Close();
        }

        public override async void UpdateText(string new_text,SQLService s)
        {
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("text",new_text),
                new NpgsqlParameter("user_id",UserId),
                new NpgsqlParameter("train_id",TrainId),
            ];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE train_comments SET text = ($1) WHERE user_id = ($2) AND train_id = ($3)", parameters);
            reader.Close();
        }
    }
    public class WagonNote:Note{
        public int WagonId{get;set;}
        public WagonNote(){}
        public override async void DeleteSelf(SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("user_id",UserId), new NpgsqlParameter("wagon_id",WagonId)];
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM wagon_comments WHERE user_id = ($1) AND wagon_id = ($2)", parameters);
            reader.Close();
        }
        public override async void UpdateText(string new_text,SQLService s)
        {
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("text",new_text),
                new NpgsqlParameter("user_id",UserId),
                new NpgsqlParameter("wagon_id",WagonId),
            ];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE wagon_comments SET text = ($1) WHERE user_id = ($2) AND wagon_id = ($3)", parameters);
            reader.Close();
        }
    }



    public class BlackBoardNote:Note{
    
        public DateTime Date{get;set;}
        public int Id{get;set;}
        public int Priority{get;set;}
        public BlackBoardNote(){}

        public override async void DeleteSelf(SQLService s)
        {
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("user_id",UserId), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("DELETE FROM board_comments WHERE user_id = ($1) AND id = ($2)", parameters);
            reader.Close();
        }

        public async void SetPriority(int np,SQLService s){
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("priority",np),
                new NpgsqlParameter("user_id",UserId),
                new NpgsqlParameter("id",Id),
            ];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE board_comments SET priority = ($1) WHERE user_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }

        public override async void UpdateText(string new_text,SQLService s)
        {
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("text",new_text),
                new NpgsqlParameter("date",DateTime.Now),
                new NpgsqlParameter("user_id",UserId),
                new NpgsqlParameter("id",Id),
            ];
            Date = DateTime.Now;
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE board_comments SET text = ($1),date = ($2) WHERE user_id = ($3) AND id = ($4)", parameters);
            reader.Close();
        }
    }


    public class Wagon{
        public int State{get;set;}
        public int Id{get;set;}
        public int TrainId{get;set;}
        public int NOrder{get;set;}
        public Wagon(){}
        public async void setNOrder(int no,SQLService s){
            NOrder = no;
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("n_order",no),
                new NpgsqlParameter("train_id",TrainId),
                new NpgsqlParameter("id",Id),
            ];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE wagons SET n_order = ($1) WHERE train_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }

        public async void setState(int nstatus,SQLService s){
            State = nstatus;
            List<NpgsqlParameter> parameters =
            [
                new NpgsqlParameter("state",nstatus),
                new NpgsqlParameter("train_id",TrainId),
                new NpgsqlParameter("id",Id),
            ];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE wagons SET state = ($1) WHERE train_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }
        
    }

    public class TrainTemplate{
        public int Id{get;set;}
        public string Name{get;set;}
        public double MaxLength{get;set;}
        public string Destination{get;set;}
        public TrainTemplate(){}
        public async void SetName(string nn,SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("name",nn), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE templates SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetMaxLength(double nl,SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("max_leanght",nl), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE templates SET max_leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

    }

    public class Train{
        List<Wagon>Wagons = new List<Wagon>();
        public int Id{get;set;}
        public string Name{get;set;}
        public bool Coll{get;set;}
        public string Destination{get;set;}
        public double MaxLength{get;set;}
        public double Lenght{get;set;}
        public DateTime Date{get;set;}
        public int Status{get;set;}
        
        public Train(){}

        public void AddVagon(Wagon w){
            Wagons.Add(w);
        }
        public async void SeColl(bool nd,SQLService s){
            Coll = nd;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("coll",nd), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET coll = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetDestination(string nd,SQLService s){
            Destination = nd;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("destination",nd), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET destination = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetDate(DateTime nl,SQLService s){
            Date = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("date",nl), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET date = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetStatus(int ns,SQLService s){
            Status = ns;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("state",ns), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET state = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetName(string nn,SQLService s){
            Name = nn;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("name",nn), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public async void SetMaxLenght(double nl,SQLService s){
            MaxLength = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("max_leanght",nl), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET max_leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public async void SetLenght(double nl,SQLService s){
            Lenght = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("leanght",nl), new NpgsqlParameter("id",Id)];
            NpgsqlDataReader reader = await s.sqlCommand("UPDATE trains SET leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

    }
    
}
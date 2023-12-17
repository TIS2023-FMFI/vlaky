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

    
    public abstract class Note(){
        public int UserId{get;set;}
        public string Text{get;set;}
        public abstract void UpdateText(string new_text,SQLService s);
        public abstract void DeleteSelf(SQLService s);
    }

    public class TrainNote:Note{
        public int TrainId{get;set;}
        public TrainNote(){}
        
        public override void DeleteSelf(SQLService s){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM train_comments WHERE user_id = ($1) AND train_id = ($2)", parameters);
            reader.Close();
        }

        public override void UpdateText(string new_text,SQLService s)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE train_comments SET text = ($1) WHERE user_id = ($2) AND train_id = ($3)", parameters);
            reader.Close();
        }
    }
    public class WagonNote:Note{
        public int WagonId{get;set;}
        public WagonNote(){}
        public override void DeleteSelf(SQLService s){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("wagon_id",WagonId));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM wagon_comments WHERE user_id = ($1) AND wagon_id = ($2)", parameters);
            reader.Close();
        }
        public override void UpdateText(string new_text,SQLService s)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("wagon_id",WagonId));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagon_comments SET text = ($1) WHERE user_id = ($2) AND wagon_id = ($3)", parameters);
            reader.Close();
        }
    }



    public class BlackBoardNote:Note{
    
        public DateTime Date{get;set;}
        public int Id{get;set;}
        public int Priority{get;set;}
        public BlackBoardNote(){}

        public override void DeleteSelf(SQLService s)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("DELETE FROM board_comments WHERE user_id = ($1) AND id = ($2)", parameters);
            reader.Close();
        }

        public void SetPriority(int np,SQLService s){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("priority",np));
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE board_comments SET priority = ($1) WHERE user_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }

        public override void UpdateText(string new_text,SQLService s)
        {
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("text",new_text));
            parameters.Append(new NpgsqlParameter("date",DateTime.Now));
            Date = DateTime.Now;
            parameters.Append(new NpgsqlParameter("user_id",UserId));
            parameters.Append(new NpgsqlParameter("id",Id));
            Date = DateTime.Now;
            NpgsqlDataReader reader = s.sqlCommand("UPDATE board_comments SET text = ($1),date = ($2) WHERE user_id = ($3) AND id = ($4)", parameters);
            reader.Close();
        }
    }


    public class Wagon{
        public int State{get;set;}
        public int Id{get;set;}
        public int TrainId{get;set;}
        public int NOrder{get;set;}
        public Wagon(){}
        public void setNOrder(int no,SQLService s){
            NOrder = no;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("n_order",no));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagons SET n_order = ($1) WHERE train_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }

        public void setState(int nstatus,SQLService s){
            State = nstatus;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("state",nstatus));
            parameters.Append(new NpgsqlParameter("train_id",TrainId));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE wagons SET state = ($1) WHERE train_id = ($2) AND id = ($3)", parameters);
            reader.Close();
        }
        
    }

    public class TrainTemplate{
        public int Id{get;set;}
        public string Name{get;set;}
        public double MaxLength{get;set;}
        public string Destination{get;set;}
        public TrainTemplate(){}
        public void SetName(string nn,SQLService s){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("name",nn));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE templates SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public void SetMaxLength(double nl,SQLService s){
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("max_leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE templates SET max_leanght = ($1) WHERE id = ($2)", parameters);
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
        public void SeColl(bool nd,SQLService s){
            Coll = nd;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("coll",nd));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET coll = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public void SetDestination(string nd,SQLService s){
            Destination = nd;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("destination",nd));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET destination = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public void SetDate(DateTime nl,SQLService s){
            Date = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("date",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET date = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public void SetStatus(int ns,SQLService s){
            Status = ns;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("state",ns));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET state = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public void SetName(string nn,SQLService s){
            Name = nn;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("name",nn));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public void SetMaxLenght(double nl,SQLService s){
            MaxLength = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("max_leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET max_leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public void SetLenght(double nl,SQLService s){
            Lenght = nl;
            IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters = new List<NpgsqlParameter>();
            parameters.Append(new NpgsqlParameter("leanght",nl));
            parameters.Append(new NpgsqlParameter("id",Id));
            NpgsqlDataReader reader = s.sqlCommand("UPDATE trains SET leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

    }
    
}
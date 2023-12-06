

using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;

namespace code.Models
{
    public class Account{

        [JsonPropertyName("Id")]
        public int Id{get;set;}

        [JsonPropertyName("Type")]
        public string Type{get;set;}

    }

    public class AccountManager{
        List<Account>Accounts = new List<Account>();
        public AccountManager(){
            //nacitame Accounts do listu
        }
        public bool Empty{get;set;}
        public void AddAccount(Account n){
            Accounts.Add(n);
            //update db
        }
        public void RemoveAccount(int id){
            foreach(var u in Accounts){
                if(u.Id==id){
                    Accounts.Remove(u);
                }
            }
            //update db
        }
        public void RemoveAll(){
            Accounts.Clear();
            //update db
        }
        public List<Account> GetAccounts(){
            return Accounts;
        }

        // public List<BlackBoardNote> SortedNotes(){
          
        // }

    }


    public class Note{
        public int id;
        string text;
        public Note(int nid){
            id = nid;
            //na zaklade db text
        }
        public void SetText(string ntext){
            text = ntext;
            //update db
        }
        public string GetText(){
            return text;
        }
    }

    public class BlackBoardNote:Note{
        public BlackBoardNote(int nid):base(nid){}
        public string Color{get;set;}
    }

    public class BlackBoard{
        List<BlackBoardNote>Notes = new List<BlackBoardNote>();
        public BlackBoard(){
            //nacitame notes do listu
        }
        public bool Empty{get;set;}
        public void AddNote(BlackBoardNote n){
            Notes.Add(n);
            //update db
        }
        public void RemoveNote(BlackBoardNote n){
            Notes.Remove(n);
            //update db
        }
        public void RemoveAll(){
            Notes.Clear();
            //update db
        }
        public List<BlackBoardNote> GetNotes(){
            return Notes;
        }

        // public List<BlackBoardNote> SortedNotes(){
          
        // }

    }

    public class Wagon{
        string status;
        int id;
        Wagon(int nid){
            id = nid;
            //nastavime status podla db
        }

        public void setStatus(string nstatus){
            status = nstatus;
            //update db
        }
        public string getStatus(){
            return status;
        }
        
    }

    public class TrainTemplate{
        int id;
        string name;
        double maxLength;
        public TrainTemplate(int nid){
            id = nid;
        }
        public void SetName(string nn){
            name = nn;
            //update db
        }
        public string GetName(){
            return name;
        }
        public void SetMaxLength(double nl){
            maxLength = nl;
        }
        public double GetMaxLenght(){
            return maxLength;
        }

    }

    public class Train{
        List<Wagon>Wagons = new List<Wagon>();
        int id;
        string name;
        bool dutiable;
        string destination;
        string leavingTime;
        string status;
        
        public Train(int nid){
            id = nid;
            //nacitame z db do listu a aj parametre
        }
        public int GetId(){
            return id;
        }
        public void AddVagon(Wagon w){
            Wagons.Add(w);
        }
        public void SetDutiable(bool nd){
            dutiable = nd;
        }
        public bool GetDutiable(){
            return dutiable;

        }
        public void SetDestination(string nd){
            destination = nd;
            //update db
        }
        public string GetDestination(){
            return destination;
        }
        public void SetLeavingTime(string nl){
            leavingTime = nl;
            //update db
        }
        public string GetLeavingTime(){
            return leavingTime;
        }
        public void SetStatus(string ns){
            status = ns;
            //update db
        }
        public string getStatus(){
            return status;
        }
        public void SetName(string nn){
            name = nn;
            //update db
        }
        public string GetName(){
            return name;
        }

    }



}
using code.Services;
using Npgsql;

namespace code.Models
{
    public class Train{
        public List<Wagon>Wagons = new List<Wagon>();
        public int Id{get;set;}
        public string Name{get;set;}
        public bool Coll{get;set;}
        public string Destination{get;set;}
        public double MaxLength{get;set;}
        public double Lenght{get;set;}
        public DateTime Date{get;set;}
        public int Status{get;set;}
        public int nWagons{get;set;}

        public Train(){}

        public void AddVagon(Wagon w){
            Wagons.Add(w);
        }
        public async void SeColl(bool nd,SQLService s){
            Coll = nd;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("coll",nd), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET coll = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetDestination(string nd,SQLService s){
            Destination = nd;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("destination",nd), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET destination = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetDate(DateTime nl,SQLService s){
            Date = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("date",nl), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET date = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetStatus(int ns,SQLService s){
            Status = ns;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("state",ns), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET state = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
        public async void SetName(string nn,SQLService s){
            Name = nn;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("name",nn), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public async void SetMaxLenght(double nl,SQLService s){
            MaxLength = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("max_leanght",nl), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET max_leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

        public async void SetLenght(double nl,SQLService s){
            Lenght = nl;
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("leanght",nl), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE trains SET leanght = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }

    }
}
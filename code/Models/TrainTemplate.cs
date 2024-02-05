using code.Services;
using Npgsql;

namespace code.Models
{
    public class TrainTemplate{
        public int Id{get;set;}
        public string Name{get;set;}
        public string Destination{get;set;}
        public TrainTemplate(){}
        public async void SetName(string nn,SQLService s){
            List<NpgsqlParameter> parameters = [new NpgsqlParameter("name",nn), new NpgsqlParameter("id",Id)];
            MyReader reader = await s.sqlCommand("UPDATE templates SET name = ($1) WHERE id = ($2)", parameters);
            reader.Close();
        }
    }
}
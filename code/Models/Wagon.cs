using code.Services;
using Npgsql;

namespace code.Models
{
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
}
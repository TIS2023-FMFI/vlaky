using System;
using Npgsql;
using code.Models;

namespace code.Services
{
    public class WagonManagerService
	{
		SQLService s;
		public WagonManagerService(SQLService s)
		{
			this.s = s;
		}

        public void AddWagon(Wagon w)
        {
            string sql = "INSERT INTO wagons (train_id, n_order, state) VALUES (($1),($2),($3))";
			
			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("train_id", w.TrainId));
			parameters.Add(new NpgsqlParameter("n_order", w.NOrder));
			parameters.Add(new NpgsqlParameter("state", w.State));
			
			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);
			reader.Close();
        }

        public List<Wagon> GetWagonsByTrainId(int tid)
		{
			
			string sql = "SELECT * from wagons WHERE train_id = ($1)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("train_id", tid));

			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			List<Wagon> wagons = new List<Wagon>();
			while(reader.Read())
			{
				var temp = new Wagon
                {
                    Id = (int)reader[0];
                    TrainId = (int)reader[1];
                    NOrder = (int)reader[2];
                    State = (int)reader[3];
                }
				wagons.Add(temp);
			}
			reader.Close();

			return wagons;
		}

        public void UpdateWagon(Wagon w)
		{
			string sql = "UDPATE wagons SET train_id = ($2), n_order = ($3), state = ($4) WHERE id = ($1)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("id", w.Id));
			parameters.Add(new NpgsqlParameter("train_id", w.TrainId));
			parameters.Add(new NpgsqlParameter("n_order", w.NOrder));
			parameters.Add(new NpgsqlParameter("state", w.State));
		
			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			reader.Close();
		}


		public void DeleteWagon(Wagon w)
		{
			string sql = "DELETE FROM wagons WHERE id = ($1)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", w.Id));

			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			reader.Close();
		}
    }
}

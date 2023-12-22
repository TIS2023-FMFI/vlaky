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

        public async void AddWagon(Wagon w)
        {
            string sql = "INSERT INTO wagons (train_id, n_order, state) VALUES ((@p1),(@p2),(@p3))";
			
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", w.TrainId));
			parameters.Add(new NpgsqlParameter("p2", w.NOrder));
			parameters.Add(new NpgsqlParameter("p3", w.State));
			
			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);
			reader.Close();
        }

        public async Task<List<Wagon>> GetWagonsByTrainId(int tid)
		{
			
			string sql = "SELECT * from wagons WHERE train_id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", tid));

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			List<Wagon> wagons = new List<Wagon>();
			while(reader.Read())
			{
				var temp = new Wagon
                {
                    Id = (int)reader[0],
                    TrainId = (int)reader[1],
                    NOrder = (int)reader[2],
                    State = (int)reader[3]
                };
				wagons.Add(temp);
			}
			reader.Close();

			return wagons;
		}

        public async void UpdateWagon(Wagon w)
		{
			string sql = "UDPATE wagons SET train_id = (@p2), n_order = (@p3), state = (@p4) WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", w.Id));
			parameters.Add(new NpgsqlParameter("p2", w.TrainId));
			parameters.Add(new NpgsqlParameter("p3", w.NOrder));
			parameters.Add(new NpgsqlParameter("p4", w.State));
		
			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}


		public async void DeleteWagon(Wagon w)
		{
			string sql = "DELETE FROM wagons WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", w.Id));

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}
    }
}

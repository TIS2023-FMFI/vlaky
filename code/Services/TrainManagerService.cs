using System;
using Npgsql;
using code.Models;

namespace code.Services
{
	public class TrainManagerService
	{
		private SQLService s;
		public TrainManagerService(SQLService s)
		{
			this.s = s;
		}

		public async void AddTrain(Train trn)
		{
			
			string sql = "INSERT INTO trains (name, destination, state, date, coll, n_wagons, max_lenght, lenght) VALUES ((@p1),(@p2),(@p3),(@p4),(@p5),(@p6),(@p7),(@p8))";
			
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", trn.Name));
			parameters.Add(new NpgsqlParameter("p2", trn.Destination));
			parameters.Add(new NpgsqlParameter("p3", trn.Status));
			parameters.Add(new NpgsqlParameter("p4", trn.Date));
			parameters.Add(new NpgsqlParameter("p5", trn.Coll));
			parameters.Add(new NpgsqlParameter("p6", trn.Wagons.Count));
			parameters.Add(new NpgsqlParameter("p7", trn.MaxLength));
			parameters.Add(new NpgsqlParameter("p8", trn.Lenght));
			
			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);
			reader.Close();
			
		}

		public async Task<List<Train>> GetTrainsByDate(string from, string to)
		{
			WagonManagerService WMService = new WagonManagerService(s);
			string sql = "SELECT * from trains WHERE date BETWEEN (@p1) AND (@p2)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", from));
			parameters.Add(new NpgsqlParameter("p2", to));

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			List<Train> trains = new List<Train>();
			while(reader.Read())
			{
				var temp = new Train
				{
					Id = (int)reader[0],
					Name = (string)reader[1],
					Destination = (string)reader[2],
					Status = (int)reader[3],
					Date = (DateTime)reader[4],
					Coll = (bool)reader[5],
					
					MaxLength=(int)reader[7],
					Lenght = (double)reader[8]
				};
				trains.Add(temp);
			}
			reader.Close();
			foreach (Train train in trains)
			{
				train.Wagons = await WMService.GetWagonsByTrainId(train.Id);
			}

			return trains;
		}

		public async void UpdateTrain(Train trn)
		{
			string sql = "UDPATE trains SET name = (@p2), destination = (@p3), state = (@p4), date = (@p5), coll=(@p6) " 
													+ "n_wagons = (@p7), max_lenght = (@p8), lenght = (@p9) "
																						+ "WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", trn.Id));
			parameters.Add(new NpgsqlParameter("p2", trn.Name));
			parameters.Add(new NpgsqlParameter("p3", trn.Destination));
			parameters.Add(new NpgsqlParameter("p4", trn.Status));
			parameters.Add(new NpgsqlParameter("p5", trn.Date));
			parameters.Add(new NpgsqlParameter("p6", trn.Coll));
			parameters.Add(new NpgsqlParameter("p7", trn.Wagons.Count));
			parameters.Add(new NpgsqlParameter("p8", trn.MaxLength));
			parameters.Add(new NpgsqlParameter("p9", trn.Lenght));
		
			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public async void DeleteTrain(Train trn)
		{
			string sql = "DELETE FROM trains WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", trn.Id));

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}
	
	}	

}

	

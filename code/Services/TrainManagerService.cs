using System;
using Npgsql;
using code.Models;

namespace code.Services
{
	public class TrainManagerService
	{
		public TrainManagerService()
		{
			
		}

		public void AddTrain(Train trn)
		{
			
			string sql = "INSERT INTO trains (name, destination, state, date, coll, n_wagons, max_lenght, lenght) VALUES ($1,$2,$3,$4,$5,$6,$7,$8)";
			
			NpgsqlParameter param1 = new NpgsqlParameter("name", trn.GetName());
			NpgsqlParameter param2 = new NpgsqlParameter("destination", trn.GetDestination());
			NpgsqlParameter param3 = new NpgsqlParameter("state", trn.getStatus());
			NpgsqlParameter param4 = new NpgsqlParameter("date", trn.GetLeavingTime());
			NpgsqlParameter param5 = new NpgsqlParameter("coll", trn.GetDutiable());
			NpgsqlParameter param6 = new NpgsqlParameter("n_wagons", trn.GetWagonsCount());
			NpgsqlParameter param7 = new NpgsqlParameter("max_lenght", trn.GetMaxLenght());
			NpgsqlParameter param8 = new NpgsqlParameter("lenght", trn.GetLenght());
			
			NpgsqlParameter[] parameters = new[] {param1,param2,param3,param4,param5,param6,param7,param8};
			NpgsqlDataReader reader = sqlCommand(sql,parameters);
			reader.Close();
			
		}

		public List<Train> GetTrainsByDate(string from, string to)
		{
			
			string sql = "SELECT * from trains WHERE date BETWEEN ($1) AND ($2)";
			
			NpgsqlParameter param1 = new NpgsqlParameter("date", from);
			NpgsqlParameter param2 = new NpgsqlParameter("date", to);
			
			NpgsqlParameter[] parameters = new[] {param1,param2};
			NpgsqlDataReader reader = sqlCommand(sql,parameters);

			List<Train> trains = new List<Train>();
			while(reader.Read())
			{
				Train temp = new Train(reader[0]);
				temp.SetName(reader[1]);
				temp.SetDestination(reader[2]);
				temp.SetStatus(reader[3]);
				temp.SetLeavingTime(reader[4]);
				temp.SetDutiable(reader[5]);
				temp.SetWagonsCount(reader[6]);
				temp.SetMaxLenght(reader[7]);
				temp.SetLenght(reader[8]);
				
				trains.Add(temp);
			}
			reader.Close();

			return trains;
		}

		public void UpdateTrainState(int trnid, string trnstate)
		{
			string sql = "UDPATE trains SET state = ($1) WHERE id = ($2)";

			NpgsqlParameter param1 = new NpgsqlParameter("state", trnstate);
			NpgsqlParameter param2 = new NpgsqlParameter("id", trnid);
			
			NpgsqlParameter[] parameters = new[] {param1,param2};
			NpgsqlDataReader reader = sqlCommand(sql,parameters);

			reader.Close();
		}
	
	}	

}

	

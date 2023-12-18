﻿using System;
using Npgsql;
using code.Models;

namespace code.Services
{
	public class TrainManagerService
	{
		SQLService s;
		public TrainManagerService(SQLService s, DbConnectionService connectionService)
		{
			this.s = s;
		}

		public void AddTrain(Train trn)
		{
			
			string sql = "INSERT INTO trains (name, destination, state, date, coll, n_wagons, max_lenght, lenght) VALUES ($1,$2,$3,$4,$5,$6,$7,$8)";
			
			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("name", trn.Name));
			parameters.Add(new NpgsqlParameter("destination", trn.Destination));
			parameters.Add(new NpgsqlParameter("state", trn.Status));
			parameters.Add(new NpgsqlParameter("date", trn.Date));
			parameters.Add(new NpgsqlParameter("coll", trn.Coll));
			parameters.Add(new NpgsqlParameter("n_wagons", trn. trn.Wagons.Count));
			parameters.Add(new NpgsqlParameter("max_lenght", trn.MaxLength));
			parameters.Add(new NpgsqlParameter("lenght", trn.Lenght));
			
			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);
			reader.Close();
			
		}

		public List<Train> GetTrainsByDate(string from, string to)
		{
			WagonManagerService WMService = new WagonManagerService(s);
			string sql = "SELECT * from trains WHERE date BETWEEN ($1) AND ($2)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("date", from));
			parameters.Add(new NpgsqlParameter("date", to));

			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			List<Train> trains = new List<Train>();
			while(reader.Read())
			{
				var temp = new Train
				{
					Id = (int)reader[0];
					Name = (string)reader[1];
					Destination = (string)reader[2];
					Status = (int)reader[3];
					Date = (DateTime)reader[4];
					Coll = (bool)reader[5];
					Wagons = WMService.GetWagonsByTrainId((int)reader[0]);
					MaxLength=(int)reader[7];
					Lenght = (double)reader[8];
				}
				
				trains.Add(temp);
			}
			reader.Close();

			return trains;
		}

		public void UpdateTrain(Train trn)
		{
			string sql = "UDPATE trains SET name = ($2), destination = ($3), state = ($4), date = ($5), " 
													+ "n_wagons = ($6), max_lenght = ($7), lenght = ($8) "
																						+ "WHERE id = ($1)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", trn.Id));
			parameters.Add(new NpgsqlParameter("name", trn.Name));
			parameters.Add(new NpgsqlParameter("destination", trn.Destination));
			parameters.Add(new NpgsqlParameter("state", trn.Status));
			parameters.Add(new NpgsqlParameter("date", trn.Date));
			parameters.Add(new NpgsqlParameter("coll", trn.Coll));
			parameters.Add(new NpgsqlParameter("n_wagons", trn.Wagons.Count));
			parameters.Add(new NpgsqlParameter("max_lenght", trn.MaxLength));
			parameters.Add(new NpgsqlParameter("lenght", trn.Lenght));
		
			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public void DeleteTrain(Train trn)
		{
			string sql = "DELETE FROM trains WHERE id = ($1)";

			IEnumerable<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", trn.Id));

			NpgsqlDataReader reader = s.sqlCommand(sql,parameters);

			reader.Close();
		}
	
	}	

}

	

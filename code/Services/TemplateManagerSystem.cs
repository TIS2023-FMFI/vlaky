using System;
using Npgsql;
using code.Models;

namespace code.Services
{
	public class TemplateManagerService
	{
		private SQLService s;
		public TemplateManagerService(SQLService s)
		{
			this.s = s;
		}

		public async void AddTemplate(TrainTemplate tmp)
		{
			string sql = "INSERT INTO templates (name, destination, max_lenght) VALUES ((@p1),(@p2),(@p3))";
			
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("p1", tmp.Name),
				new NpgsqlParameter("p2", tmp.Destination),
				new NpgsqlParameter("p3", tmp.MaxLength)
			};
			
			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);
			reader.Close();
			
		}

		public async Task<List<TrainTemplate>> GetTemplates()
		{
			string sql = "SELECT * from templates";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

            List<TrainTemplate> templates = new List<TrainTemplate>();
			while(reader.Read())
			{
				var tmp = new TrainTemplate
				{
					Id = (int)reader[0],
					Name = (string)reader[1],
					Destination = (string)reader[2],	
					MaxLength=(int)reader[3]
				};
                templates.Add(tmp);
			}
			reader.Close();
			return templates;
		}

		public async void UpdateTemplate(TrainTemplate tmp)
		{
			string sql = "UDPATE templates SET name = (@p2), destination = (@p3), max_lenght = (@p4) WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter("p1", tmp.Id),
				new NpgsqlParameter("p2", tmp.Name),
				new NpgsqlParameter("p3", tmp.Destination),
				new NpgsqlParameter("p4", tmp.MaxLength)
			};

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public async void DeleteTemplate(TrainTemplate tmp)
		{
			string sql = "DELETE FROM templates WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
			new NpgsqlParameter("p1", tmp.Id)
			};

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}
	
	}	

}
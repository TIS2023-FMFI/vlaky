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

		public async Task AddTemplate(TrainTemplate tmp)
		{
			string sql = "INSERT INTO templates (name, destination) VALUES ((@p1),(@p2))";
			
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("p1", tmp.Name),
				new NpgsqlParameter("p2", tmp.Destination),
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
				};
                templates.Add(tmp);
			}
			reader.Close();
			return templates;
		}

		public async Task UpdateTemplate(TrainTemplate tmp)
		{
			string sql = "UPDATE templates SET name = (@p2), destination = (@p3) WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter("p1", tmp.Id),
				new NpgsqlParameter("p2", tmp.Name),
				new NpgsqlParameter("p3", tmp.Destination),
			};

			NpgsqlDataReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public async Task DeleteTemplate(TrainTemplate tmp)
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
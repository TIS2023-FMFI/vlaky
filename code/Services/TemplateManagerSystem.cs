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
			
			MyReader reader = await s.sqlCommand(sql,parameters);
			reader.Close();
			
		}

		public async Task<List<TrainTemplate>> GetTemplates()
		{
			string sql = "SELECT * from templates ORDER BY id";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

			MyReader myreader = await s.sqlCommand(sql,parameters);
			NpgsqlDataReader reader = myreader.Reader;

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
			myreader.Close();
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

			MyReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public async Task DeleteTemplate(TrainTemplate tmp)
		{
			string sql = "DELETE FROM templates WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
			new NpgsqlParameter("p1", tmp.Id)
			};

			MyReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}

		public async Task<TrainTemplate> GetTemplateById(int templateId)
		{
			string sql = "SELECT * FROM templates WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("p1", templateId)
			};

			MyReader myreader = await s.sqlCommand(sql, parameters);
			NpgsqlDataReader reader = myreader.Reader;

			TrainTemplate t = null;
			if (reader.Read())
			{
				t = new TrainTemplate
				{
					Id = (int)reader["id"],
					Name = (string)reader["name"],
					Destination = (string)reader["destination"]
				};
			}

			myreader.Close();
			return t;
		}

		public async Task RemoveTemplateById(int templateId)
        {
            string sql = "DELETE FROM templates WHERE id = @p1";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", templateId)
            };

            MyReader reader = await s.sqlCommand(sql, parameters);
            reader.Close();
        }
	
	}	

}
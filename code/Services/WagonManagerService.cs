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

        public async Task AddWagon(Wagon w)
        {
            string sql = "INSERT INTO wagons (train_id, n_order, state) VALUES ((@p1),(@p2),(@p3))";
			
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", w.TrainId));
			parameters.Add(new NpgsqlParameter("p2", w.NOrder));
			parameters.Add(new NpgsqlParameter("p3", w.State));
			
			MyReader reader = await s.sqlCommand(sql,parameters);
			reader.Close();
        }

        public async Task<List<Wagon>> GetWagonsByTrainId(int tid)
		{
			
			string sql = "SELECT * from wagons WHERE train_id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("p1", tid));

			MyReader myreader = await s.sqlCommand(sql,parameters);
            NpgsqlDataReader reader = myreader.Reader;

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
			myreader.Close();

			return wagons;
		}

        public async Task UpdateWagon(Wagon w)
		{
			string sql = "UPDATE wagons SET train_id = (@p2), n_order = (@p3), state = (@p4) WHERE id = (@p1)";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", w.Id));
			parameters.Add(new NpgsqlParameter("p2", w.TrainId));
			parameters.Add(new NpgsqlParameter("p3", w.NOrder));
			parameters.Add(new NpgsqlParameter("p4", w.State));
		
			MyReader reader = await s.sqlCommand(sql,parameters);

			reader.Close();
		}


		public async Task DeleteWagon(Wagon w)
        {
            string deleteWagonSql = "DELETE FROM wagons WHERE id = (@p2)";

            List<NpgsqlParameter> deleteWagonParams = new List<NpgsqlParameter>();
            deleteWagonParams.Add(new NpgsqlParameter("p2", w.Id));

            MyReader deleteWagonReader = await s.sqlCommand(deleteWagonSql, deleteWagonParams);

            deleteWagonReader.Close();
        }


		public async Task AddWagonNote(WagonNote note)
        {
            string sql = "INSERT INTO wagon_comments (wagon_id, user_id, text) VALUES ((@p1), (@p2), (@p3))";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", note.WagonId),
                new NpgsqlParameter("p2", note.UserId),
                new NpgsqlParameter("p3", note.Text)
            };

            MyReader reader = await s.sqlCommand(sql, parameters);
            reader.Close();
        }

		public async Task<List<WagonNote>> GetWagonNotesByWagonId(int wagonId)
        {
            string sql = @"
                SELECT wagon_id, user_id, text, name
                FROM wagon_comments LEFT JOIN users
                ON wagon_comments.user_id = users.id
                WHERE wagon_id = @p1
                ORDER BY wagon_comments.id";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", wagonId)
            };

            MyReader myreader = await s.sqlCommand(sql, parameters);
            NpgsqlDataReader reader = myreader.Reader;
            
            List<WagonNote> wagonNotes = new List<WagonNote>();
            while(reader.Read())
            {
                var note = new WagonNote
                {
                    WagonId = (int)reader["wagon_id"],
                    UserId = (int)reader["user_id"],
                    Text = reader["text"].ToString(),
                    UserName = reader["name"].ToString()
                };
                wagonNotes.Add(note);
            }
            myreader.Close();

            return wagonNotes;
        }
    }
}

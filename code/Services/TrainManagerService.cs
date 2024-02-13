using System;
using Npgsql;
using code.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace code.Services
{
    public class TrainManagerService
    {
        private SQLService s;
        public TrainManagerService(SQLService s)
        {
            this.s = s;
        }

        public async Task<int> AddTrain(Train trn)
        {
            WagonManagerService WMService = new WagonManagerService(s);
            string sql = "INSERT INTO trains (name, destination, state, date, coll, n_wagons, max_leanght, leanght) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8) RETURNING id";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", trn.Name),
                new NpgsqlParameter("p2", trn.Destination),
                new NpgsqlParameter("p3", trn.Status),
                new NpgsqlParameter("p4", trn.Date),
                new NpgsqlParameter("p5", trn.Coll),
                new NpgsqlParameter("p6", trn.nWagons), 
                new NpgsqlParameter("p7", trn.MaxLength),
                new NpgsqlParameter("p8", trn.Lenght)
            };

            int trainId = 0;
            MyReader myreader = await s.sqlCommand(sql, parameters);
            NpgsqlDataReader reader = myreader.Reader;
            if (reader.Read())
            {
                trainId = (int)reader[0];
            }
            myreader.Close();

            for (int i = 1; i <= trn.nWagons; i++)
            {
                Wagon w = new Wagon
                {
                    TrainId = trainId,
                    NOrder = i,
                    State = 0
                };
                await WMService.AddWagon(w);
            }

            return trainId;
        }


        public async Task<List<Train>> GetTrainsByDate(DateTime from, DateTime to)
        {
            WagonManagerService WMService = new WagonManagerService(s);
            string sql = "SELECT * from trains WHERE date BETWEEN (@p1) AND (@p2)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", from));
            parameters.Add(new NpgsqlParameter("p2", to));

            MyReader myreader = await s.sqlCommand(sql, parameters);
            NpgsqlDataReader reader = myreader.Reader;

            List<Train> trains = new List<Train>();
            while (reader.Read())
            {
                var temp = new Train
                {
                    Id = (int)reader[0],
                    Name = (string)reader[1],
                    Destination = (string)reader[2],
                    Status = (int)reader[3],
                    Date = (DateTime)reader[4],
                    Coll = (bool)reader[5],
                    nWagons = (int)reader[6],
                    MaxLength = (double)reader[7],
                    Lenght = (double)reader[8]
                };
                trains.Add(temp);
            }
            myreader.Close();
            foreach (Train train in trains)
            {
                train.Wagons = await WMService.GetWagonsByTrainId(train.Id);
            }

            return trains;
        }

        public async Task UpdateTrain(Train trn)
        {
            WagonManagerService WMService = new WagonManagerService(s);
            var existingWagons = (await WMService.GetWagonsByTrainId(trn.Id))
                .OrderBy(wagon => wagon.NOrder)
                .ToList();
            string sql = "UPDATE trains SET name = @p2, destination = @p3, state = @p4, date = @p5, coll = @p6, n_wagons = @p7, max_leanght = @p8, leanght = @p9 WHERE id = @p1";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", trn.Id),
                new NpgsqlParameter("p2", trn.Name),
                new NpgsqlParameter("p3", trn.Destination),
                new NpgsqlParameter("p4", trn.Status),
                new NpgsqlParameter("p5", trn.Date),
                new NpgsqlParameter("p6", trn.Coll),
                new NpgsqlParameter("p7", trn.Wagons.Count),
                new NpgsqlParameter("p8", trn.MaxLength),
                new NpgsqlParameter("p9", trn.Lenght)
            };

            await s.sqlCommand(sql, parameters);

            if (existingWagons.Count > trn.nWagons)
            {
                for (int i = trn.nWagons; i < existingWagons.Count; i++)
                {
                    await WMService.DeleteWagon(existingWagons[i]);
                }
            }
            else if (existingWagons.Count < trn.nWagons)
            {
                for (int i = existingWagons.Count + 1; i <= trn.nWagons; i++)
                {
                    await WMService.AddWagon(new Wagon
                    {
                        TrainId = trn.Id,
                        NOrder = i,
                        State = 0
                    });
                }
            }
        }

        public async Task DeleteTrain(Train trn)
        {
            string sql = "DELETE FROM trains WHERE id = (@p1)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", trn.Id));

            MyReader reader = await s.sqlCommand(sql, parameters);

            reader.Close();
        }

        public async Task DeleteTrainById(int trainId)
        {
            Train train = await GetTrainById(trainId);
            
            if (train == null)
            {
                return;
            }

            // Delete the train
            string deleteTrainSql = "DELETE FROM trains WHERE id = @p1";
            List<NpgsqlParameter> trainParameters = new List<NpgsqlParameter>();
            trainParameters.Add(new NpgsqlParameter("p1", trainId));
            
            await s.sqlCommand(deleteTrainSql, trainParameters);
        }

        public async Task<Train> GetTrainById(int trainId)
        {
            string sql = "SELECT * FROM trains WHERE id = (@p1)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", trainId));

            MyReader myreader = await s.sqlCommand(sql, parameters);
            NpgsqlDataReader reader = myreader.Reader;

            Train train = null;
            if (reader.Read())
            {
                train = new Train
                {
                    Id = (int)reader[0],
                    Name = (string)reader[1],
                    Destination = (string)reader[2],
                    Status = (int)reader[3],
                    Date = (DateTime)reader[4],
                    Coll = (bool)reader[5],
                    nWagons = (int)reader[6],
                    MaxLength = (double)reader[7],
                    Lenght = (double)reader[8]
                };
            }
            myreader.Close();

            if (train != null)
            {
                WagonManagerService WMService = new WagonManagerService(s);
                train.Wagons = await WMService.GetWagonsByTrainId(train.Id);

                if (train.Wagons != null)
                {
                    foreach (Wagon w in train.Wagons) {
                        w.Notes = await WMService.GetWagonNotesByWagonId(w.Id);
                    }
                }
            }

            return train;
        }

        public async Task<TrainNote> GetTrainNoteByTrainId(int trainId)
        {
            string sql = "SELECT * FROM train_comments WHERE train_id = (@p1)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("p1", trainId));

            MyReader myreader = await s.sqlCommand(sql, parameters);
            NpgsqlDataReader reader = myreader.Reader;

            TrainNote trainNote = null;
            if (reader.Read())
            {
                trainNote = new TrainNote
                {
                    TrainId = (int)reader[0],
                    UserId = (int)reader[1],
                    Text = (string)reader[2],
                };
            }
            myreader.Close();

            return trainNote;
        }

        public async Task UpdateTrainNote(TrainNote note)
        {
            string sql = "UPDATE train_comments SET text = (@p2), user_id = (@p3) WHERE train_id = (@p1)";
            
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", note.TrainId),
                new NpgsqlParameter("p2", note.Text),
                new NpgsqlParameter("p3", note.UserId)
            };

            await s.sqlCommand(sql, parameters);
        }

        public async Task AddTrainNote(TrainNote trainNote)
        {
            string sql = "INSERT INTO train_comments (train_id, user_id, text) VALUES (@p1, @p2, @p3)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", trainNote.TrainId),
                new NpgsqlParameter("p2", trainNote.UserId),
                new NpgsqlParameter("p3", trainNote.Text)
            };

            await s.sqlCommand(sql, parameters);
        }

        public async Task UpdateTrainNWagons(int trainId, int newNWagons)
        {
            string sql = "UPDATE trains SET n_wagons = (@p2) WHERE id = (@p1)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", trainId),
                new NpgsqlParameter("p2", newNWagons)
            };

            await s.sqlCommand(sql, parameters);
        }

        public async Task UpdateTrainStatus(int trainId, int newStatus)
        {
            string sql = "UPDATE trains SET state = (@p2) WHERE id = (@p1)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("p1", trainId),
                new NpgsqlParameter("p2", newStatus)
            };

            await s.sqlCommand(sql, parameters);
        }
    }
}

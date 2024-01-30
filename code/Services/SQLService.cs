using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace code.Services
{
    public class SQLService
    {
        private DbConnectionService DbService;
        private NpgsqlConnection connection;

        public SQLService(DbConnectionService DbService)
        {
            this.DbService = DbService;
            connection = DbService.getConnection();
        }

        /**
            sql sa musi rovnat celemu spravnemu prikazu kde parametre(konkretne hodnoty) su nahradene "(@px)"
            a x zacina od 1 a inkrementuje, samotne parametre premenit na NpgsqlParameter nasledovne:
                NpgsqlParameter param = new NpgsqlParameter("px", value);
            a poslat v zozname do parameters
            vystup vyberte nasledovne:
            NpgsqlDataReader reader = await sqlCommand(sql, parameters)
        */
        public async Task<MyReader> sqlCommand(string sql, IEnumerable<NpgsqlParameter> parameters)
        {
            refreshConnection();

            Task<MyReader> task = null;
            MyReader reader = null;

            while (task == null || (task.IsFaulted && task.Exception.InnerException.GetType() == typeof(NpgsqlException)))
            {
                task = sqlExecuteCommandAsync(sql, parameters);
                reader = await task;
            }

            return reader;
        }

        private async Task<MyReader> sqlExecuteCommandAsync(string sql, IEnumerable<NpgsqlParameter> parameters)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            return new MyReader(connection, await cmd.ExecuteReaderAsync());
        }

        private void refreshConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }
            connection = DbService.getConnection();
        }
    }

    public class MyReader : IDisposable
    {
        private NpgsqlConnection Connection;
        public NpgsqlDataReader Reader {get; private set;}

        public MyReader(NpgsqlConnection connection, NpgsqlDataReader reader)
        {
            Connection = connection;
            Reader = reader;
        }

        public void Close()
        {
            Reader.Close();
            Connection.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}

using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace code.Services
{
    public class SQLService : IDisposable
    {
        private DbConnectionService DbService;
        private NpgsqlConnection connection;

        public SQLService(DbConnectionService DbService)
        {
            this.DbService = DbService;
            connection = DbService.getConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        /**
            sql sa musi rovnat celemu spravnemu prikazu kde parametre(konkretne hodnoty) su nahradene "(@px)"
            a x zacina od 1 a inkrementuje, samotne parametre premenit na NpgsqlParameter nasledovne:
                NpgsqlParameter param = new NpgsqlParameter("px", value);
            a poslat v zozname do parameters
            vystup vyberte nasledovne:
            NpgsqlDataReader reader = await sqlCommand(sql, parameters)
        */
        public async Task<NpgsqlDataReader> sqlCommand(string sql, IEnumerable<NpgsqlParameter> parameters)
        {
            refreshConnection();

            Task<NpgsqlDataReader> task = null;
            NpgsqlDataReader reader = null;

            while (task == null || (task.IsFaulted && task.Exception.InnerException.GetType() == typeof(NpgsqlException)))
            {
                task = sqlExecuteCommandAsync(sql, parameters);
                reader = await task;
            }

            return reader;
        }

        private async Task<NpgsqlDataReader> sqlExecuteCommandAsync(string sql, IEnumerable<NpgsqlParameter> parameters)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            return await cmd.ExecuteReaderAsync();
        }

        private void refreshConnection()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
            connection = DbService.getConnection();
        }
    }
}

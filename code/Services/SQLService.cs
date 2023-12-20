using Npgsql;

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

        ~SQLService()
        {
                connection.Dispose();
                connection = null;
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
            Task<NpgsqlDataReader> reader = null;

            while (reader == null || (reader.IsFaulted && reader.Exception.InnerException.GetType() == typeof(NpgsqlException))) {
                reader = sqlExecuteCommandAsync(sql, parameters);
            }

            return await reader;
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
    }
}
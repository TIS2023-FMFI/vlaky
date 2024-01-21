using Npgsql;

namespace code.Services
{
    public class DbConnectionService 
    {
        private static string CONNECTION_STRING;

        private NpgsqlDataSource dataSource;

        public DbConnectionService() 
        {
            dataSource = NpgsqlDataSource.Create(CONNECTION_STRING);
        }
        public DbConnectionService(string connectionString) 
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
        }

        public static void configureService(ConfigurationManager conf) 
        {
            CONNECTION_STRING = conf.GetConnectionString("DefaultConnection");
        }

        public NpgsqlConnection getConnection() 
        {
            return dataSource.OpenConnection();
        }
    }
}
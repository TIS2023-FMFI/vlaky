using System.Transactions;
using Microsoft.VisualBasic;
using Npgsql;
using NpgsqlTypes;

namespace code.Services
{
    public class SQLService
    {
        private readonly DbConnectionService connectionService;
        private NpgsqlConnection connection = null;
        private NpgsqlTransaction transaction = null;
        
        public SQLService(DbConnectionService connectionService) 
        {
            this.connectionService = connectionService;
        }

        private void getConnection() 
        {
            if (connection == null) 
            {
                connection = connectionService.getConnection();
            }
        }

        public void transactionBegin()
        {
            if (transaction == null) 
            {
                transaction = connection.BeginTransaction();
            }
        }

        public void transactionEnd()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
        }

        /**
            sql sa musi rovnat celemu spravnemu prikazu kde parametre(konkretne hodnoty) su nahradene "($x)"
            a x zacina od 1 a inkrementuje, samotne parametre premenit na NpgsqlParameter nasledovne:
                NpgsqlParameter param = new NpgsqlParameter("column_name", value);
            a poslat v zozname do parameters
        */
        public NpgsqlDataReader sqlCommand(string sql, IEnumerable<NpgsqlParameter> parameters)
        {
            getConnection();
            bool oneTimeTransaction = false;
            if (transaction == null) {
                transactionBegin();
                oneTimeTransaction = true;
            }

            NpgsqlCommand cmd = new NpgsqlCommand(sql, connection, transaction);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            NpgsqlDataReader reader =  cmd.ExecuteReader();

            if (oneTimeTransaction) {
                transactionEnd();
            }

            return reader;
        }
    }
}
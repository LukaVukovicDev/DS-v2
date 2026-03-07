using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace CoworkingApp.BusinessLogic.Adapters
{
    public static class SqlAdapterFactory
    {
        public static ISqlAdapter CreateAdapter(string connectionString)
        {
            if (IsMsSql(connectionString))
                return new MsSqlAdapter();
            else
                return new MySqlAdapter();
        }

        public static IDbConnection CreateConnection(string connectionString)
        {
            if (IsMsSql(connectionString))
                return new SqlConnection(connectionString);
            else
                return new MySqlConnection(connectionString);
        }

        private static bool IsMsSql(string connectionString)
        {
            return connectionString.Contains("Server=") &&
                   connectionString.Contains("Database=");
        }
    }
}

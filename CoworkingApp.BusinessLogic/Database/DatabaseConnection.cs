using CoworkingApp.BusinessLogic.Adapters;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Database
{
    public class DatabaseConnection
    {
        private ISqlAdapter _adapter;
        private static DatabaseConnection _instance;

        private IDbConnection _connection;

        private static readonly object _lock = new object();

        private DatabaseConnection(string connectionString)
        {
            _connection = SqlAdapterFactory.CreateConnection(connectionString);
            _adapter = SqlAdapterFactory.CreateAdapter(connectionString);
        }

        public static DatabaseConnection GetInstance(string connectionString)
        {
            if(_instance == null)
            {
                lock (_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new DatabaseConnection(connectionString);
                    }
                }
            }
            return _instance;
        }
        public IDbConnection GetConnection()
        {
            return _connection;
        }
        public ISqlAdapter GetAdapter()
        {
            return _adapter;
        }
    }
}

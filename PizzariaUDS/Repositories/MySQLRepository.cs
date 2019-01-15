using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace PizzariaUDS.Repositories
{
    public class MySQLRepository : IDisposable
    {

        private MySqlConnection _connection = null;
        private string _connectionStringId;
        private string _connectionString;

        public MySQLRepository()
            : this("MySQL")
        {

        }

        public MySQLRepository(string connectionStringId)
        {
            _connectionStringId = connectionStringId;
        }

        public virtual string ConnnectionStringId
        {
            get
            {
                return _connectionStringId;
            }
        }

        protected MySqlConnection Database
        {
            get
            {
                if (_connection == null)
                {
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings[ConnnectionStringId].ConnectionString;
                    }

                    _connection = new MySqlConnection(_connectionString);
                }

                return _connection;
            }
        }

        public MySQLRepository UseConnectionStringId(string connectionStringId)
        {
            _connectionStringId = connectionStringId;

            return this;
        }

        public MySQLRepository UseConnectionString(string connectionString)
        {
            _connectionString = connectionString;

            return this;
        }

        public virtual void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

    }
}

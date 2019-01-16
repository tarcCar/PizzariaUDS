using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Configuration;
using System.Data;

namespace PizzariaUDS.Repositories
{
    public class MySQLRepository : IDisposable, IRepository
    {

        private MySqlConnection _connection = null;
        private string _connectionStringId;
        private string _connectionString;
        private IConfiguration configuration;

        public MySQLRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            _connectionStringId = "MySQL";
        }

        
        public virtual string ConnnectionStringId
        {
            get
            {
                return _connectionStringId;
            }
        }

        public IDbConnection Database
        {
            get
            {
                if (_connection == null)
                {
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        _connectionString = configuration.GetConnectionString(_connectionStringId);
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

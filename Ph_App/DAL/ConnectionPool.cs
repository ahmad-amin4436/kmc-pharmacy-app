using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Ph_App.Database;

namespace Ph_App.DAL
{
    public static class ConnectionPool
    {
        private static readonly string _connectionString = DatabaseConfig.ConnectionString;
        private static readonly int _maxPoolSize = 100;
        private static readonly int _minPoolSize = 5;
        private static readonly int _connectionTimeout = 30;

        static ConnectionPool()
        {
            // Configure connection pool settings
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString)
            {
                MaxPoolSize = _maxPoolSize,
                MinPoolSize = _minPoolSize,
                ConnectTimeout = _connectionTimeout,
                Pooling = true,
                ConnectionReset = true,
                //ConnectionLifetime = 300, // 5 minutes
                Enlist = true,
                LoadBalanceTimeout = 300
            };
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public static void TestConnection()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
            }
        }

        public static async Task TestConnectionAsync()
        {
            using (var connection = await GetConnectionAsync())
            {
                // Connection is already open
            }
        }
    }
}

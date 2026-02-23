using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Ph_App.Database
{
    public static class DatabaseConfig
    {
        // Connection string - modify according to your SQL Server setup
        public static string ConnectionString 
        { 
            get 
            {
                // For SQL Server Express with connection pooling
                return "Server=.\\SQLEXPRESS;Database=PharmacyDB;Trusted_Connection=True;MultipleActiveResultSets=True;Max Pool Size=100;Min Pool Size=5;Connect Timeout=30;";
                
                // For SQL Server Standard
                // return "Server=YOUR_SERVER_NAME;Database=PharmacyDB;User ID=your_username;Password=your_password;MultipleActiveResultSets=True;Max Pool Size=100;Min Pool Size=5;Connect Timeout=30;";
                
                // For LocalDB
                // return "Server=(localdb)\\MSSQLLocalDB;Database=PharmacyDB;Trusted_Connection=True;MultipleActiveResultSets=True;Max Pool Size=100;Min Pool Size=5;Connect Timeout=30;";
            }
        }

        // Create a new connection
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // Test database connection
        public static bool TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }

        // Test database connection asynchronously
        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }

        // Get connection string without pooling for specific operations
        public static string GetConnectionStringWithoutPooling()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString)
            {
                Pooling = false
            };
            return builder.ToString();
        }
    }
}

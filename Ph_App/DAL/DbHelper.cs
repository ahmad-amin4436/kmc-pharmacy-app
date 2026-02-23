using Ph_App.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Ph_App.DAL
{
    public static class DbHelper
    {
        private static readonly string _connString = GetConnectionStringFromConfig("PharmacyDb");

        private static string GetConnectionStringFromConfig(string name)
        {
            try
            {
                var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                if (!File.Exists(configPath)) return DatabaseConfig.ConnectionString;
                var doc = new XmlDocument();
                doc.Load(configPath);
                var node = doc.SelectSingleNode($"/configuration/connectionStrings/add[@name='{name}']");
                if (node?.Attributes == null) return DatabaseConfig.ConnectionString;
                var attr = node.Attributes["connectionString"];
                return attr?.Value ?? DatabaseConfig.ConnectionString;
            }
            catch
            {
                return DatabaseConfig.ConnectionString;
            }
        }

        public static SqlConnection GetConnection() => new SqlConnection(_connString);

        public static async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task<object> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                await conn.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }

        public static async Task<DataTable> ExecuteDataTableAsync(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                var dt = new DataTable();
                await Task.Run(() => da.Fill(dt));
                return dt;
            }
        }

        // Synchronous versions for backward compatibility
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length >0)
                cmd.Parameters.AddRange(parameters);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}

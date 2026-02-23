using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ph_App.Database;

namespace Ph_App.DAL
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;

        protected BaseRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
            _tableName = typeof(T).Name;
        }

        // Abstract methods that must be implemented by derived classes
        public virtual T GetById(int id)
        {
            throw new NotImplementedException("GetById must be overridden in derived repository");
        }

        public virtual IEnumerable<T> GetAll()
        {
            throw new NotImplementedException("GetAll must be overridden in derived repository");
        }

        public virtual IEnumerable<T> GetByFilter(string filter)
        {
            throw new NotImplementedException("GetByFilter must be overridden in derived repository");
        }

        public virtual T Add(T entity)
        {
            throw new NotImplementedException("Add must be overridden in derived repository");
        }

        public virtual T Update(T entity)
        {
            throw new NotImplementedException("Update must be overridden in derived repository");
        }

        public virtual bool Delete(int id)
        {
            throw new NotImplementedException("Delete must be overridden in derived repository");
        }

        public virtual bool Delete(T entity)
        {
            throw new NotImplementedException("Delete must be overridden in derived repository");
        }

        public virtual int Count()
        {
            throw new NotImplementedException("Count must be overridden in derived repository");
        }

        public virtual int CountByFilter(string filter)
        {
            throw new NotImplementedException("CountByFilter must be overridden in derived repository");
        }

        // Helper methods that can be used by derived classes
        protected void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }

        protected T ExecuteScalarQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    AddParameters(command, parameters);
                }
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapFromReader(reader);
                    }
                }
            }
            return null;
        }

        protected IEnumerable<T> ExecuteReaderQuery(string query, Dictionary<string, object> parameters = null)
        {
            var result = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    AddParameters(command, parameters);
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapFromReader(reader));
                    }
                }
            }
            return result;
        }

        protected int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    AddParameters(command, parameters);
                }
                
                return command.ExecuteNonQuery();
            }
        }

        protected object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                if (parameters != null)
                {
                    AddParameters(command, parameters);
                }
                
                return command.ExecuteScalar();
            }
        }

        protected abstract T MapFromReader(IDataReader reader);

        // Helper methods for creating INSERT and UPDATE commands
        protected virtual SqlCommand CreateInsertCommand(SqlConnection connection, T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "ID" && p.Name != $"{typeof(T).Name}ID")
                .ToList();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var query = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({parameterNames}); SELECT SCOPE_IDENTITY();";

            var command = new SqlCommand(query, connection);
            
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                command.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
            }

            return command;
        }

        protected virtual SqlCommand CreateUpdateCommand(SqlConnection connection, T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "ID" && p.Name != $"{typeof(T).Name}ID")
                .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var idProperty = properties.FirstOrDefault(p => p.Name.EndsWith("ID")) ?? 
                           typeof(T).GetProperties().FirstOrDefault(p => p.Name == "ID");

            var idValue = idProperty?.GetValue(entity);
            var idColumn = idProperty?.Name ?? "ID";

            var query = $"UPDATE {_tableName} SET {setClause} WHERE {idColumn} = @{idColumn}";

            var command = new SqlCommand(query, connection);
            
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                command.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
            }
            
            if (idValue != null)
            {
                command.Parameters.AddWithValue($"@{idColumn}", idValue);
            }

            return command;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class UserRepository : BaseRepository<User>
    {
        public override User GetById(int id)
        {
            var query = @"
                SELECT * FROM Users 
                WHERE UserID = @UserID AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<User> GetAll()
        {
            var query = @"
                SELECT * FROM Users 
                WHERE IsDeleted = 0 
                ORDER BY Username";
            
            return ExecuteReaderQuery(query);
        }

        public User GetByUsername(string username)
        {
            var query = @"
                SELECT * FROM Users 
                WHERE Username = @Username AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Username", username }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public User Authenticate(string username, string passwordHash)
        {
            var query = @"
                SELECT * FROM Users 
                WHERE Username = @Username AND PasswordHash = @PasswordHash AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Username", username },
                { "@PasswordHash", passwordHash }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override User Add(User entity)
        {
            var query = @"
                INSERT INTO Users (Username, PasswordHash, Role, CreatedDate)
                VALUES (@Username, @PasswordHash, @Role, @CreatedDate);
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Username", entity.Username },
                { "@PasswordHash", entity.PasswordHash },
                { "@Role", entity.Role ?? "User" },
                { "@CreatedDate", DateTime.Now }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.UserID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        public override User Update(User entity)
        {
            var query = @"
                UPDATE Users 
                SET Username = @Username, Role = @Role, ModifiedDate = @ModifiedDate
                WHERE UserID = @UserID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Username", entity.Username },
                { "@Role", entity.Role },
                { "@ModifiedDate", DateTime.Now },
                { "@UserID", entity.UserID }
            };
            
            ExecuteNonQuery(query, parameters);
            return entity;
        }

        public override bool Delete(int id)
        {
            var query = @"
                UPDATE Users 
                SET IsDeleted = 1, ModifiedDate = @ModifiedDate 
                WHERE UserID = @UserID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", id },
                { "@ModifiedDate", DateTime.Now }
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        protected override User MapFromReader(IDataReader reader)
        {
            return new User
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                Username = reader["Username"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                Role = reader["Role"].ToString(),
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
            };
        }

        protected override SqlCommand CreateInsertCommand(SqlConnection connection, User entity)
        {
            var command = new SqlCommand(@"
                INSERT INTO Users (Username, PasswordHash, Role, CreatedDate)
                VALUES (@Username, @PasswordHash, @Role, @CreatedDate);
                SELECT SCOPE_IDENTITY();", connection);
            
            command.Parameters.AddWithValue("@Username", entity.Username);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@Role", entity.Role ?? "User");
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            
            return command;
        }

        protected override SqlCommand CreateUpdateCommand(SqlConnection connection, User entity)
        {
            var command = new SqlCommand(@"
                UPDATE Users 
                SET Username = @Username, Role = @Role, ModifiedDate = @ModifiedDate
                WHERE UserID = @UserID", connection);
            
            command.Parameters.AddWithValue("@Username", entity.Username);
            command.Parameters.AddWithValue("@Role", entity.Role);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            command.Parameters.AddWithValue("@UserID", entity.UserID);
            
            return command;
        }

        public int GetNextUserId()
        {
            var query = "sp_GetNextUserId";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // Additional ADO.NET specific methods
        public IEnumerable<User> GetUsersByRole(string role)
        {
            var query = @"
                SELECT * FROM Users 
                WHERE Role = @Role AND IsDeleted = 0 
                ORDER BY Username";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Role", role }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public bool IsUsernameExists(string username, int? excludeUserId = null)
        {
            var query = @"
                SELECT COUNT(*) FROM Users 
                WHERE Username = @Username AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Username", username }
            };
            
            if (excludeUserId.HasValue)
            {
                query += " AND UserID != @ExcludeUserID";
                parameters.Add("@ExcludeUserID", excludeUserId.Value);
            }
            
            return Convert.ToInt32(ExecuteScalar(query, parameters)) > 0;
        }

        public override IEnumerable<User> GetByFilter(string filter)
        {
            var query = @"
                SELECT * FROM Users 
                WHERE IsDeleted = 0 
                AND (Username LIKE @Filter OR Role LIKE @Filter)
                ORDER BY Username";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM Users WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM Users 
                WHERE IsDeleted = 0 
                AND (Username LIKE @Filter OR Role LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(User entity)
        {
            return Delete(entity.UserID);
        }

        public int GetTotalUsersCount()
        {
            var query = "SELECT COUNT(*) FROM Users WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }
    }
}

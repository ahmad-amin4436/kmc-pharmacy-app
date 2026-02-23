using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class AuditLogRepository : BaseRepository<AuditLogEntry>
    {
        public override AuditLogEntry GetById(int id)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE al.LogID = @LogID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@LogID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<AuditLogEntry> GetAll()
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                ORDER BY al.ActionDateTime DESC";
            
            return ExecuteReaderQuery(query);
        }

        public IEnumerable<AuditLogEntry> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE al.ActionDateTime BETWEEN @FromDate AND @ToDate
                ORDER BY al.ActionDateTime DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@FromDate", fromDate },
                { "@ToDate", toDate }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public IEnumerable<AuditLogEntry> GetByUser(int userId)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE al.UserID = @UserID
                ORDER BY al.ActionDateTime DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public IEnumerable<AuditLogEntry> GetByActionType(string actionType)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE al.ActionType = @ActionType
                ORDER BY al.ActionDateTime DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@ActionType", actionType }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public IEnumerable<AuditLogEntry> GetByTable(string tableAffected)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE al.TableAffected = @TableAffected
                ORDER BY al.ActionDateTime DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@TableAffected", tableAffected }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override AuditLogEntry Add(AuditLogEntry entity)
        {
            var query = @"
                INSERT INTO AuditLogs (
                    UserID, ActionType, TableAffected, RecordID, 
                    OldValue, NewValue, ActionDateTime, IPAddress, UserAgent
                )
                VALUES (
                    @UserID, @ActionType, @TableAffected, @RecordID,
                    @OldValue, @NewValue, @ActionDateTime, @IPAddress, @UserAgent
                );
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", entity.UserID },
                { "@ActionType", entity.ActionType },
                { "@TableAffected", entity.TableAffected },
                { "@RecordID", entity.RecordID },
                { "@OldValue", entity.OldValue },
                { "@NewValue", entity.NewValue },
                { "@ActionDateTime", entity.ActionDateTime },
                { "@IPAddress", entity.IPAddress },
                { "@UserAgent", entity.UserAgent }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.LogID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        protected override AuditLogEntry MapFromReader(IDataReader reader)
        {
            return new AuditLogEntry
            {
                LogID = Convert.ToInt32(reader["LogID"]),
                UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : (int?)null,
                    Username = reader["Username"]?.ToString(),
                ActionType = reader["ActionType"].ToString(),
                TableAffected = reader["TableAffected"].ToString(),
                RecordID = reader["RecordID"]?.ToString(),
                OldValue = reader["OldValue"]?.ToString(),
                NewValue = reader["NewValue"]?.ToString(),
                ActionDateTime = Convert.ToDateTime(reader["ActionDateTime"]),
                IPAddress = reader["IPAddress"]?.ToString(),
                UserAgent = reader["UserAgent"]?.ToString()
            };
        }

        protected override SqlCommand CreateInsertCommand(SqlConnection connection, AuditLogEntry entity)
        {
            var command = new SqlCommand(@"
                INSERT INTO AuditLogs (
                    UserID, ActionType, TableAffected, RecordID, 
                    OldValue, NewValue, ActionDateTime, IPAddress, UserAgent
                )
                VALUES (
                    @UserID, @ActionType, @TableAffected, @RecordID,
                    @OldValue, @NewValue, @ActionDateTime, @IPAddress, @UserAgent
                );
                SELECT SCOPE_IDENTITY();", connection);
            
            command.Parameters.AddWithValue("@UserID", entity.UserID);
            command.Parameters.AddWithValue("@ActionType", entity.ActionType);
            command.Parameters.AddWithValue("@TableAffected", entity.TableAffected);
            command.Parameters.AddWithValue("@RecordID", entity.RecordID);
            command.Parameters.AddWithValue("@OldValue", entity.OldValue);
            command.Parameters.AddWithValue("@NewValue", entity.NewValue);
            command.Parameters.AddWithValue("@ActionDateTime", entity.ActionDateTime);
            command.Parameters.AddWithValue("@IPAddress", entity.IPAddress);
            command.Parameters.AddWithValue("@UserAgent", entity.UserAgent);
            
            return command;
        }

        protected override SqlCommand CreateUpdateCommand(SqlConnection connection, AuditLogEntry entity)
        {
            // Audit logs should not be updated - they are immutable
            throw new NotSupportedException("Audit log entries cannot be updated");
        }

        public override bool Delete(int id)
        {
            // Audit logs should not be deleted - they are immutable
            throw new NotSupportedException("Audit log entries cannot be deleted");
        }

        public override IEnumerable<AuditLogEntry> GetByFilter(string filter)
        {
            var query = @"
                SELECT al.*, u.Username 
                FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE (al.ActionType LIKE @Filter OR al.TableAffected LIKE @Filter OR u.Username LIKE @Filter OR al.NewValue LIKE @Filter)
                ORDER BY al.ActionDateTime DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override AuditLogEntry Update(AuditLogEntry entity)
        {
            // Audit logs should not be updated - they are immutable
            throw new NotSupportedException("Audit log entries cannot be updated");
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM AuditLogs";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM AuditLogs al 
                LEFT JOIN Users u ON al.UserID = u.UserID 
                WHERE (al.ActionType LIKE @Filter OR al.TableAffected LIKE @Filter OR u.Username LIKE @Filter OR al.NewValue LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(AuditLogEntry entity)
        {
            // Audit logs should not be deleted - they are immutable
            throw new NotSupportedException("Audit log entries cannot be deleted");
        }

        public int GetNextAuditLogId()
        {
            var query = "sp_GetNextAuditLogId";
            
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

        // Specialized audit logging methods using pure ADO.NET
    public void LogUserLogin(int userId, string username, string ipAddress = null, string userAgent = null)
        {
            var query = @"
                INSERT INTO AuditLogs (
                    UserID, ActionType, TableAffected, RecordID, 
                    OldValue, NewValue, ActionDateTime, IPAddress, UserAgent
                )
                VALUES (
                    @UserID, @ActionType, @TableAffected, @RecordID,
                    @OldValue, @NewValue, @ActionDateTime, @IPAddress, @UserAgent
                )";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId },
                { "@ActionType", "LOGIN" },
                { "@TableAffected", "Users" },
                { "@RecordID", userId.ToString() },
                { "@OldValue", "" },
                { "@NewValue", $"User {username} logged in" },
                { "@ActionDateTime", DateTime.Now },
                { "@IPAddress", ipAddress },
                { "@UserAgent", userAgent }
            };
            
            ExecuteNonQuery(query, parameters);
        }

    public void LogUserLogout(int userId, string username, string ipAddress = null, string userAgent = null)
        {
            var query = @"
                INSERT INTO AuditLogs (
                    UserID, ActionType, TableAffected, RecordID, 
                    OldValue, NewValue, ActionDateTime, IPAddress, UserAgent
                )
                VALUES (
                    @UserID, @ActionType, @TableAffected, @RecordID,
                    @OldValue, @NewValue, @ActionDateTime, @IPAddress, @UserAgent
                )";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId },
                { "@ActionType", "LOGOUT" },
                { "@TableAffected", "Users" },
                { "@RecordID", userId.ToString() },
                { "@OldValue", "" },
                { "@NewValue", $"User {username} logged out" },
                { "@ActionDateTime", DateTime.Now },
                { "@IPAddress", ipAddress },
                { "@UserAgent", userAgent }
            };
            
            ExecuteNonQuery(query, parameters);
        }

        public void LogUserAction(int? userId, string actionType, string tableAffected, string recordId, string oldValue, string newValue, string ipAddress = null, string userAgent = null)
        {
            // if caller passed null, try to use currently authenticated user from context
            if (!userId.HasValue)
            {
                try
                {
                    var current = Ph_App.Database.PharmacyDBContext.CurrentUser;
                    if (current != null)
                        userId = current.UserID;
                }
                catch
                {
                    // ignore any errors here and leave userId as null
                }
            }
            var query = @"
                INSERT INTO AuditLogs (
                    UserID, ActionType, TableAffected, RecordID, 
                    OldValue, NewValue, ActionDateTime, IPAddress, UserAgent
                )
                VALUES (
                    @UserID, @ActionType, @TableAffected, @RecordID,
                    @OldValue, @NewValue, @ActionDateTime, @IPAddress, @UserAgent
                )";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId },
                { "@ActionType", actionType },
                { "@TableAffected", tableAffected },
                { "@RecordID", recordId },
                { "@OldValue", oldValue },
                { "@NewValue", newValue },
                { "@ActionDateTime", DateTime.Now },
                { "@IPAddress", ipAddress },
                { "@UserAgent", userAgent }
            };
            
            ExecuteNonQuery(query, parameters);
        }

        // Additional ADO.NET specific methods
        public int GetAuditLogCount(DateTime? fromDate = null, DateTime? toDate = null, int? userId = null, string actionType = null)
        {
            var query = "SELECT COUNT(*) FROM AuditLogs WHERE 1=1";
            var parameters = new Dictionary<string, object>();
            
            if (fromDate.HasValue)
            {
                query += " AND ActionDateTime >= @FromDate";
                parameters.Add("@FromDate", fromDate.Value);
            }
            
            if (toDate.HasValue)
            {
                query += " AND ActionDateTime <= @ToDate";
                parameters.Add("@ToDate", toDate.Value);
            }
            
            if (userId.HasValue)
            {
                query += " AND UserID = @UserID";
                parameters.Add("@UserID", userId.Value);
            }
            
            if (!string.IsNullOrEmpty(actionType))
            {
                query += " AND ActionType = @ActionType";
                parameters.Add("@ActionType", actionType);
            }
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public IEnumerable<string> GetActionTypes()
        {
            var query = @"
                SELECT DISTINCT ActionType 
                FROM AuditLogs 
                ORDER BY ActionType";
            
            var result = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["ActionType"].ToString());
                    }
                }
            }
            return result;
        }

        public IEnumerable<string> GetAffectedTables()
        {
            var query = @"
                SELECT DISTINCT TableAffected 
                FROM AuditLogs 
                ORDER BY TableAffected";
            
            var result = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["TableAffected"].ToString());
                    }
                }
            }
            return result;
        }

    }
}

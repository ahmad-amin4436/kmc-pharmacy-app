using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        public override Customer GetById(int id)
        {
            var query = @"
                SELECT * FROM Customers 
                WHERE CustomerID = @CustomerID AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<Customer> GetAll()
        {
            var query = @"
                SELECT * FROM Customers 
                WHERE IsDeleted = 0 
                ORDER BY CustomerName";
            
            return ExecuteReaderQuery(query);
        }

        public Customer GetByName(string customerName)
        {
            var query = @"
                SELECT * FROM Customers 
                WHERE CustomerName = @CustomerName AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerName", customerName }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override Customer Add(Customer entity)
        {
            var query = @"
                INSERT INTO Customers (CustomerName, Contact, Phone, Email, Address, Balance, CreatedDate)
                VALUES (@CustomerName, @Contact, @Phone, @Email, @Address, @Balance, @CreatedDate);
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerName", entity.CustomerName },
                { "@Contact", entity.Contact },
                { "@Phone", entity.Phone },
                { "@Email", entity.Email },
                { "@Address", entity.Address },
                { "@Balance", entity.Balance },
                { "@CreatedDate", DateTime.Now }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.CustomerID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        public override Customer Update(Customer entity)
        {
            var query = @"
                UPDATE Customers 
                SET CustomerName = @CustomerName, Contact = @Contact, Phone = @Phone, 
                    Email = @Email, Address = @Address, Balance = @Balance, ModifiedDate = @ModifiedDate
                WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerName", entity.CustomerName },
                { "@Contact", entity.Contact },
                { "@Phone", entity.Phone },
                { "@Email", entity.Email },
                { "@Address", entity.Address },
                { "@Balance", entity.Balance },
                { "@ModifiedDate", DateTime.Now },
                { "@CustomerID", entity.CustomerID }
            };
            
            ExecuteNonQuery(query, parameters);
            return entity;
        }

        public override bool Delete(int id)
        {
            var query = @"
                UPDATE Customers 
                SET IsDeleted = 1, ModifiedDate = @ModifiedDate 
                WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", id },
                { "@ModifiedDate", DateTime.Now }
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        protected override Customer MapFromReader(IDataReader reader)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                Contact = reader["Contact"]?.ToString(),
                Phone = reader["Phone"]?.ToString(),
                Email = reader["Email"]?.ToString(),
                Address = reader["Address"]?.ToString(),
                Balance = Convert.ToDecimal(reader["Balance"]),
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : (DateTime?)null
            };
        }

        public int GetNextCustomerId()
        {
            var query = "sp_GetNextCustomerId";
            
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

        public override IEnumerable<Customer> GetByFilter(string filter)
        {
            var query = @"
                SELECT * FROM Customers 
                WHERE IsDeleted = 0 
                AND (CustomerName LIKE @Filter OR Phone LIKE @Filter OR Email LIKE @Filter)
                ORDER BY CustomerName";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM Customers WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM Customers 
                WHERE IsDeleted = 0 
                AND (CustomerName LIKE @Filter OR Phone LIKE @Filter OR Email LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(Customer entity)
        {
            return Delete(entity.CustomerID);
        }

        public void UpdateBalance(int customerId, decimal newBalance)
        {
            var query = @"
                UPDATE Customers 
                SET Balance = @Balance, ModifiedDate = @ModifiedDate
                WHERE CustomerID = @CustomerID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId },
                { "@Balance", newBalance },
                { "@ModifiedDate", DateTime.Now }
            };
            
            ExecuteNonQuery(query, parameters);
        }

        public IEnumerable<Customer> GetDebtors()
        {
            var query = @"
                SELECT * FROM Customers 
                WHERE IsDeleted = 0 AND Balance < 0
                ORDER BY Balance";
            
            return ExecuteReaderQuery(query);
        }
    }
}

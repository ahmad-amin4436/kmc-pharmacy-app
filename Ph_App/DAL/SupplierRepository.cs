using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class SupplierRepository : BaseRepository<Supplier>
    {
        public override Supplier GetById(int id)
        {
            var query = @"
                SELECT * FROM Suppliers 
                WHERE SupplierID = @SupplierID AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SupplierID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<Supplier> GetAll()
        {
            var query = @"
                SELECT * FROM Suppliers 
                WHERE IsDeleted = 0 
                ORDER BY SupplierName";
            
            return ExecuteReaderQuery(query);
        }

        public Supplier GetByName(string supplierName)
        {
            var query = @"
                SELECT * FROM Suppliers 
                WHERE SupplierName = @SupplierName AND IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SupplierName", supplierName }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override Supplier Add(Supplier entity)
        {
            var query = @"
                INSERT INTO Suppliers (SupplierName, ContactPerson, Phone, Email, Address, CreatedDate)
                VALUES (@SupplierName, @ContactPerson, @Phone, @Email, @Address, @CreatedDate);
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SupplierName", entity.SupplierName },
                { "@ContactPerson", entity.ContactPerson },
                { "@Phone", entity.Phone },
                { "@Email", entity.Email },
                { "@Address", entity.Address },
                { "@CreatedDate", DateTime.Now }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.SupplierID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        public override Supplier Update(Supplier entity)
        {
            var query = @"
                UPDATE Suppliers 
                SET SupplierName = @SupplierName, ContactPerson = @ContactPerson, Phone = @Phone, 
                    Email = @Email, Address = @Address, ModifiedDate = @ModifiedDate
                WHERE SupplierID = @SupplierID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SupplierName", entity.SupplierName },
                { "@ContactPerson", entity.ContactPerson },
                { "@Phone", entity.Phone },
                { "@Email", entity.Email },
                { "@Address", entity.Address },
                { "@ModifiedDate", DateTime.Now },
                { "@SupplierID", entity.SupplierID }
            };
            
            ExecuteNonQuery(query, parameters);
            return entity;
        }

        public override bool Delete(int id)
        {
            var query = @"
                UPDATE Suppliers 
                SET IsDeleted = 1, ModifiedDate = @ModifiedDate 
                WHERE SupplierID = @SupplierID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SupplierID", id },
                { "@ModifiedDate", DateTime.Now }
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        protected override Supplier MapFromReader(IDataReader reader)
        {
            return new Supplier
            {
                SupplierID = Convert.ToInt32(reader["SupplierID"]),
                SupplierName = reader["SupplierName"].ToString(),
                ContactPerson = reader["ContactPerson"]?.ToString(),
                Phone = reader["Phone"]?.ToString(),
                Email = reader["Email"]?.ToString(),
                Address = reader["Address"]?.ToString(),
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : (DateTime?)null
            };
        }

        public int GetNextSupplierId()
        {
            var query = "sp_GetNextSupplierId";
            
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

        public override IEnumerable<Supplier> GetByFilter(string filter)
        {
            var query = @"
                SELECT * FROM Suppliers 
                WHERE IsDeleted = 0 
                AND (SupplierName LIKE @Filter OR ContactPerson LIKE @Filter OR Phone LIKE @Filter OR Email LIKE @Filter)
                ORDER BY SupplierName";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM Suppliers WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM Suppliers 
                WHERE IsDeleted = 0 
                AND (SupplierName LIKE @Filter OR ContactPerson LIKE @Filter OR Phone LIKE @Filter OR Email LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(Supplier entity)
        {
            return Delete(entity.SupplierID);
        }
    }
}

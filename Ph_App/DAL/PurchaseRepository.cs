using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class PurchaseRepository : BaseRepository<Purchase>
    {
        public override Purchase GetById(int id)
        {
            var query = @"
                SELECT p.*, s.SupplierName, u.Username 
                FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.PurchaseID = @PurchaseID AND p.IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@PurchaseID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<Purchase> GetAll()
        {
            var query = @"
                SELECT p.*, s.SupplierName, u.Username 
                FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.IsDeleted = 0 
                ORDER BY p.PurchaseDate DESC";
            
            return ExecuteReaderQuery(query);
        }

        public IEnumerable<Purchase> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            var query = @"
                SELECT p.*, s.SupplierName, u.Username 
                FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.IsDeleted = 0 
                AND p.PurchaseDate BETWEEN @FromDate AND @ToDate
                ORDER BY p.PurchaseDate DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@FromDate", fromDate },
                { "@ToDate", toDate }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public Purchase GetByInvoiceNo(string invoiceNo)
        {
            var query = @"
                SELECT p.*, s.SupplierName, u.Username 
                FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.InvoiceNo = @InvoiceNo AND p.IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@InvoiceNo", invoiceNo }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override Purchase Add(Purchase entity)
        {
            var query = @"
                INSERT INTO Purchases (
                    PurchaseDate, InvoiceNo, SupplierID, TotalAmount, Discount, NetAmount, 
                    PaidAmount, PaymentMethod, Notes, UserID, CreatedDate
                )
                VALUES (
                    @PurchaseDate, @InvoiceNo, @SupplierID, @TotalAmount, @Discount, @NetAmount,
                    @PaidAmount, @PaymentMethod, @Notes, @UserID, @CreatedDate
                );
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@PurchaseDate", entity.PurchaseDate },
                { "@InvoiceNo", entity.InvoiceNo },
                { "@SupplierID", entity.SupplierID },
                { "@TotalAmount", entity.TotalAmount },
                { "@Discount", entity.Discount },
                { "@NetAmount", entity.NetAmount },
                { "@PaidAmount", entity.PaidAmount },
                { "@PaymentMethod", entity.PaymentMethod },
                { "@Notes", entity.Notes },
                { "@UserID", entity.UserID },
                { "@CreatedDate", DateTime.Now }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.PurchaseID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        public override Purchase Update(Purchase entity)
        {
            var query = @"
                UPDATE Purchases 
                SET PurchaseDate = @PurchaseDate, InvoiceNo = @InvoiceNo, SupplierID = @SupplierID,
                    TotalAmount = @TotalAmount, Discount = @Discount, NetAmount = @NetAmount,
                    PaidAmount = @PaidAmount, PaymentMethod = @PaymentMethod, Notes = @Notes,
                    UserID = @UserID, ModifiedDate = @ModifiedDate
                WHERE PurchaseID = @PurchaseID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@PurchaseDate", entity.PurchaseDate },
                { "@InvoiceNo", entity.InvoiceNo },
                { "@SupplierID", entity.SupplierID },
                { "@TotalAmount", entity.TotalAmount },
                { "@Discount", entity.Discount },
                { "@NetAmount", entity.NetAmount },
                { "@PaidAmount", entity.PaidAmount },
                { "@PaymentMethod", entity.PaymentMethod },
                { "@Notes", entity.Notes },
                { "@UserID", entity.UserID },
                { "@ModifiedDate", DateTime.Now },
                { "@PurchaseID", entity.PurchaseID }
            };
            
            ExecuteNonQuery(query, parameters);
            return entity;
        }

        public override bool Delete(int id)
        {
            var query = @"
                UPDATE Purchases 
                SET IsDeleted = 1, ModifiedDate = @ModifiedDate 
                WHERE PurchaseID = @PurchaseID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@PurchaseID", id },
                { "@ModifiedDate", DateTime.Now }
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        protected override Purchase MapFromReader(IDataReader reader)
        {
            var purchase = new Purchase
            {
                PurchaseID = Convert.ToInt32(reader["PurchaseID"]),
                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"]),
                InvoiceNo = reader["InvoiceNo"].ToString(),
                SupplierID = Convert.ToInt32(reader["SupplierID"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                Discount = Convert.ToDecimal(reader["Discount"]),
                NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                PaymentMethod = reader["PaymentMethod"]?.ToString(),
                Notes = reader["Notes"]?.ToString(),
                UserID = Convert.ToInt32(reader["UserID"]),
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : (DateTime?)null
            };

            // Load related data
            purchase.Details = GetPurchaseDetails(purchase.PurchaseID).ToList();
            
            return purchase;
        }

        public int GetNextPurchaseId()
        {
            var query = "sp_GetNextPurchaseId";
            
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

        public int GetNextPurchaseDetailId()
        {
            var query = "sp_GetNextPurchaseDetailId";
            
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

        public override IEnumerable<Purchase> GetByFilter(string filter)
        {
            var query = @"
                SELECT p.*, s.SupplierName, u.Username 
                FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.IsDeleted = 0 
                AND (p.InvoiceNo LIKE @Filter OR s.SupplierName LIKE @Filter OR u.Username LIKE @Filter)
                ORDER BY p.PurchaseDate DESC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM Purchases WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM Purchases p 
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID 
                LEFT JOIN Users u ON p.UserID = u.UserID 
                WHERE p.IsDeleted = 0 
                AND (p.InvoiceNo LIKE @Filter OR s.SupplierName LIKE @Filter OR u.Username LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(Purchase entity)
        {
            return Delete(entity.PurchaseID);
        }

        private IEnumerable<PurchaseDetail> GetPurchaseDetails(int purchaseId)
        {
            var result = new List<PurchaseDetail>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT pd.*, m.MedicineName 
                    FROM PurchaseDetails pd 
                    LEFT JOIN Medicines m ON pd.MedicineID = m.MedicineID 
                    WHERE pd.PurchaseID = @PurchaseId", connection);
                
                command.Parameters.AddWithValue("@PurchaseId", purchaseId);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PurchaseDetail
                        {
                            PurchaseDetailID = Convert.ToInt32(reader["PurchaseDetailID"]),
                            PurchaseID = Convert.ToInt32(reader["PurchaseID"]),
                            MedicineID = Convert.ToInt32(reader["MedicineID"]),
                            QuantityPacks = Convert.ToInt32(reader["QuantityPacks"]),
                            QuantityStrips = Convert.ToInt32(reader["QuantityStrips"]),
                            QuantityTablets = Convert.ToInt32(reader["QuantityTablets"]),
                            PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
            }
            return result;
        }

        public void AddPurchaseDetail(PurchaseDetail detail)
        {
            var query = @"
                INSERT INTO PurchaseDetails (PurchaseID, MedicineID, QuantityPacks, QuantityStrips, QuantityTablets, PurchasePrice, Total, CreatedDate)
                VALUES (@PurchaseID, @MedicineID, @QuantityPacks, @QuantityStrips, @QuantityTablets, @PurchasePrice, @Total, @CreatedDate)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@PurchaseID", detail.PurchaseID },
                { "@MedicineID", detail.MedicineID },
                { "@QuantityPacks", detail.QuantityPacks },
                { "@QuantityStrips", detail.QuantityStrips },
                { "@QuantityTablets", detail.QuantityTablets },
                { "@PurchasePrice", detail.PurchasePrice },
                { "@Total", detail.Total },
                { "@CreatedDate", DateTime.Now }
            };
            
            ExecuteNonQuery(query, parameters);
        }
    }
}

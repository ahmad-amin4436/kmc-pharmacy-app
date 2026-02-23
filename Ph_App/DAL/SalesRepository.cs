using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class SalesRepository : BaseRepository<Sale>
    {
        public override Sale GetById(int id)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT s.*, c.CustomerName, u.Username 
                    FROM Sales s 
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
                    LEFT JOIN Users u ON s.UserID = u.UserID 
                    WHERE s.SaleID = @Id AND s.IsDeleted = 0", connection);
                command.Parameters.AddWithValue("@Id", id);
                
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

        public override IEnumerable<Sale> GetAll()
        {
            var result = new List<Sale>();
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT s.*, c.CustomerName, u.Username 
                    FROM Sales s 
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
                    LEFT JOIN Users u ON s.UserID = u.UserID 
                    WHERE s.IsDeleted = 0 
                    ORDER BY s.SaleDate DESC", connection);
                
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

        public IEnumerable<Sale> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            var result = new List<Sale>();

            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();

                var command = new SqlCommand(@"
            SELECT s.*, c.CustomerName, u.Username 
            FROM Sales s 
            LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
            LEFT JOIN Users u ON s.UserID = u.UserID 
            WHERE s.IsDeleted = 0 
            AND s.SaleDate >= @FromDate 
            AND s.SaleDate < DATEADD(DAY, 1, @ToDate)
            ORDER BY s.SaleDate DESC", connection);

                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Date;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Date;

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

        public Sale GetByInvoiceNo(string invoiceNo)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT s.*, c.CustomerName, u.Username 
                    FROM Sales s 
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
                    LEFT JOIN Users u ON s.UserID = u.UserID 
                    WHERE s.InvoiceNo = @InvoiceNo AND s.IsDeleted = 0", connection);
                command.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                
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

        public override Sale Add(Sale entity)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    INSERT INTO Sales (
                        SaleDate, InvoiceNo, TotalAmount, Discount, NetAmount, 
                        PaidAmount, PaymentMethod, Notes, CustomerID, UserID, CreatedDate
                    )
                    VALUES (
                        @SaleDate, @InvoiceNo, @TotalAmount, @Discount, @NetAmount,
                        @PaidAmount, @PaymentMethod, @Notes, @CustomerID, @UserID, @CreatedDate
                    );
                    SELECT SCOPE_IDENTITY();", connection);
                
                command.Parameters.AddWithValue("@SaleDate", entity.SaleDate);
                command.Parameters.AddWithValue("@InvoiceNo", entity.InvoiceNo);
                command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
                command.Parameters.AddWithValue("@Discount", entity.Discount);
                command.Parameters.AddWithValue("@NetAmount", entity.NetAmount);
                command.Parameters.AddWithValue("@PaidAmount", entity.PaidAmount);
                command.Parameters.AddWithValue("@PaymentMethod", entity.PaymentMethod);
                command.Parameters.AddWithValue("@Notes", entity.Notes);
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value =
                    entity.CustomerID.HasValue
                        ? (object)entity.CustomerID.Value
                        : DBNull.Value;
                command.Parameters.AddWithValue("@UserID", entity.UserID);
                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                
                entity.SaleID = Convert.ToInt32(command.ExecuteScalar());
                
                // Insert sale details
                if (entity.Details != null && entity.Details.Any())
                {
                    
                    foreach (var detail in entity.Details)
                    {
                        try
                        {
                            var detailCommand = new SqlCommand(@"
                                INSERT INTO SaleDetails (
                                    SaleID, MedicineID, QuantityPacks, QuantityStrips, 
                                    QuantityTablets, SalePrice, Total, CreatedDate
                                )
                                VALUES (
                                    @SaleID, @MedicineID, @QuantityPacks, @QuantityStrips,
                                    @QuantityTablets, @SalePrice, @Total, @CreatedDate
                                );", connection);
                            
                            detailCommand.Parameters.AddWithValue("@SaleID", entity.SaleID);
                            detailCommand.Parameters.AddWithValue("@MedicineID", detail.MedicineID);
                            detailCommand.Parameters.AddWithValue("@QuantityPacks", detail.QuantityPacks);
                            detailCommand.Parameters.AddWithValue("@QuantityStrips", detail.QuantityStrips);
                            detailCommand.Parameters.AddWithValue("@QuantityTablets", detail.QuantityTablets);
                            detailCommand.Parameters.AddWithValue("@SalePrice", detail.SalePrice);
                            detailCommand.Parameters.AddWithValue("@Total", detail.Total);
                            detailCommand.Parameters.AddWithValue("@CreatedDate", detail.CreatedDate);
                            
                            var result = detailCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue with other details
                            System.Diagnostics.Debug.WriteLine($"Error inserting sale detail: {ex.Message}");
                        }
                    }
                }
                
                return entity;
            }
        }

        public override bool Delete(int id)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Sales SET IsDeleted = 1, ModifiedDate = @ModifiedDate WHERE SaleID = @SaleID", connection);
                command.Parameters.AddWithValue("@SaleID", id);
                command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                return command.ExecuteNonQuery() > 0;
            }
        }

        protected override Sale MapFromReader(IDataReader reader)
        {
            var sale = new Sale
            {
                SaleID = Convert.ToInt32(reader["SaleID"]),
                SaleDate = Convert.ToDateTime(reader["SaleDate"]),
                InvoiceNo = reader["InvoiceNo"].ToString(),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                Discount = Convert.ToDecimal(reader["Discount"]),
                NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                PaymentMethod = reader["PaymentMethod"]?.ToString(),
                Notes = reader["Notes"]?.ToString(),
                CustomerID = reader["CustomerID"] != DBNull.Value ? Convert.ToInt32(reader["CustomerID"]) : (int?)null,
                UserID = Convert.ToInt32(reader["UserID"]),
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : (DateTime?)null
            };

            // Load related data
            sale.Details = GetSaleDetails(sale.SaleID).ToList();
            
            return sale;
        }

        protected override SqlCommand CreateInsertCommand(SqlConnection connection, Sale entity)
        {
            var command = new SqlCommand(@"
                INSERT INTO Sales (
                    SaleDate, InvoiceNo, TotalAmount, Discount, NetAmount, 
                    PaidAmount, PaymentMethod, Notes, CustomerID, UserID, CreatedDate
                )
                VALUES (
                    @SaleDate, @InvoiceNo, @TotalAmount, @Discount, @NetAmount,
                    @PaidAmount, @PaymentMethod, @Notes, @CustomerID, @UserID, @CreatedDate
                );
                SELECT SCOPE_IDENTITY();", connection);
            
            command.Parameters.AddWithValue("@SaleDate", entity.SaleDate);
            command.Parameters.AddWithValue("@InvoiceNo", entity.InvoiceNo);
            command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
            command.Parameters.AddWithValue("@Discount", entity.Discount);
            command.Parameters.AddWithValue("@NetAmount", entity.NetAmount);
            command.Parameters.AddWithValue("@PaidAmount", entity.PaidAmount);
            command.Parameters.AddWithValue("@PaymentMethod", entity.PaymentMethod);
            command.Parameters.AddWithValue("@Notes", entity.Notes);
            command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
            command.Parameters.AddWithValue("@UserID", entity.UserID);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            
            return command;
        }

        protected override SqlCommand CreateUpdateCommand(SqlConnection connection, Sale entity)
        {
            var command = new SqlCommand(@"
                UPDATE Sales 
                SET SaleDate = @SaleDate, InvoiceNo = @InvoiceNo, TotalAmount = @TotalAmount, Discount = @Discount,
                    NetAmount = @NetAmount, PaidAmount = @PaidAmount, PaymentMethod = @PaymentMethod, 
                    Notes = @Notes, CustomerID = @CustomerID, UserID = @UserID, ModifiedDate = @ModifiedDate
                WHERE SaleID = @SaleID", connection);
            
            command.Parameters.AddWithValue("@SaleDate", entity.SaleDate);
            command.Parameters.AddWithValue("@InvoiceNo", entity.InvoiceNo);
            command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
            command.Parameters.AddWithValue("@Discount", entity.Discount);
            command.Parameters.AddWithValue("@NetAmount", entity.NetAmount);
            command.Parameters.AddWithValue("@PaidAmount", entity.PaidAmount);
            command.Parameters.AddWithValue("@PaymentMethod", entity.PaymentMethod);
            command.Parameters.AddWithValue("@Notes", entity.Notes);
            command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
            command.Parameters.AddWithValue("@UserID", entity.UserID);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            command.Parameters.AddWithValue("@SaleID", entity.SaleID);
            
            return command;
        }

        public int GetNextSaleId()
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("sp_GetNextSaleId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int GetNextSaleDetailId()
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("sp_GetNextSaleDetailId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public override IEnumerable<Sale> GetByFilter(string filter)
        {
            var result = new List<Sale>();
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT s.*, c.CustomerName, u.Username 
                    FROM Sales s 
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
                    LEFT JOIN Users u ON s.UserID = u.UserID 
                    WHERE s.IsDeleted = 0 
                    AND (s.InvoiceNo LIKE @Filter OR c.CustomerName LIKE @Filter OR u.Username LIKE @Filter)
                    ORDER BY s.SaleDate DESC", connection);
                
                command.Parameters.AddWithValue("@Filter", $"%{filter}%");
                
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

        public override Sale Update(Sale entity)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = CreateUpdateCommand(connection, entity);
                command.ExecuteNonQuery();
            }
            return entity;
        }

        public override int Count()
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM Sales WHERE IsDeleted = 0", connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public override int CountByFilter(string filter)
        {
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT COUNT(*) FROM Sales s 
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID 
                    LEFT JOIN Users u ON s.UserID = u.UserID 
                    WHERE s.IsDeleted = 0 
                    AND (s.InvoiceNo LIKE @Filter OR c.CustomerName LIKE @Filter OR u.Username LIKE @Filter)", connection);
                
                command.Parameters.AddWithValue("@Filter", $"%{filter}%");
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public override bool Delete(Sale entity)
        {
            return Delete(entity.SaleID);
        }

        private IEnumerable<SaleDetail> GetSaleDetails(int saleId)
        {
            var result = new List<SaleDetail>();
            using (var connection = DatabaseConfig.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT sd.*, m.MedicineName 
                    FROM SaleDetails sd 
                    LEFT JOIN Medicines m ON sd.MedicineID = m.MedicineID 
                    WHERE sd.SaleID = @SaleId", connection);
                
                command.Parameters.AddWithValue("@SaleId", saleId);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SaleDetail
                        {
                            SaleDetailID = Convert.ToInt32(reader["SaleDetailID"]),
                            SaleID = Convert.ToInt32(reader["SaleID"]),
                            MedicineID = Convert.ToInt32(reader["MedicineID"]),
                            QuantityPacks = Convert.ToInt32(reader["QuantityPacks"]),
                            QuantityStrips = Convert.ToInt32(reader["QuantityStrips"]),
                            QuantityTablets = Convert.ToInt32(reader["QuantityTablets"]),
                            SalePrice = Convert.ToDecimal(reader["SalePrice"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        });
                    }
                }
            }
            return result;
        }
    }
}

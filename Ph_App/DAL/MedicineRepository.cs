using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ph_App.Models;

namespace Ph_App.DAL
{
    public class MedicineRepository : BaseRepository<Medicine>
    {
        public override Medicine GetById(int id)
        {
            var query = @"
                SELECT m.*, s.SupplierName 
                FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.MedicineID = @MedicineID AND m.IsDeleted = 0";
            
            var parameters = new Dictionary<string, object>
            {
                { "@MedicineID", id }
            };
            
            return ExecuteScalarQuery(query, parameters);
        }

        public override IEnumerable<Medicine> GetAll()
        {
            var query = @"
                SELECT m.*
                FROM Medicines m 
                WHERE m.IsDeleted = 0 
                ORDER BY m.MedicineName";
            
            return ExecuteReaderQuery(query);
        }

        public IEnumerable<Medicine> GetBySearchTerm(string searchTerm)
        {
            var query = @"
                SELECT m.*, s.SupplierName 
                FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.IsDeleted = 0 
                AND (m.MedicineName LIKE @SearchTerm OR m.GenericName LIKE @SearchTerm OR m.Company LIKE @SearchTerm)
                ORDER BY m.MedicineName";
            
            var parameters = new Dictionary<string, object>
            {
                { "@SearchTerm", $"%{searchTerm}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public IEnumerable<Medicine> GetExpiringMedicines(DateTime fromDate, DateTime toDate)
        {
            var query = @"
                SELECT m.*, s.SupplierName 
                FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.IsDeleted = 0 
                AND m.ExpiryDate BETWEEN @FromDate AND @ToDate
                ORDER BY m.ExpiryDate";
            
            var parameters = new Dictionary<string, object>
            {
                { "@FromDate", fromDate },
                { "@ToDate", toDate }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public IEnumerable<Medicine> GetLowStockMedicines()
        {
            var query = @"
                SELECT m.*, s.SupplierName 
                FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.IsDeleted = 0 
                AND m.CurrentStockPacks <= m.MinimumStockLevel
                ORDER BY m.CurrentStockPacks";
            
            return ExecuteReaderQuery(query);
        }

        public override Medicine Add(Medicine entity)
        {
            var query = @"
                INSERT INTO Medicines (
                    MedicineName, GenericName, Company, BatchNo, ExpiryDate,
                    PackQuantity, StripQuantity, TabletQuantity,
                    PurchasePricePerPack, PurchasePricePerStrip, PurchasePricePerTablet,
                    SalePricePerPack, SalePricePerStrip, SalePricePerTablet,
                    CurrentStockPacks, CurrentStockStrips, CurrentStockTablets,
                    MinimumStockLevel, SupplierID, CreatedDate
                )
                VALUES (
                    @MedicineName, @GenericName, @Company, @BatchNo, @ExpiryDate,
                    @PackQuantity, @StripQuantity, @TabletQuantity,
                    @PurchasePricePerPack, @PurchasePricePerStrip, @PurchasePricePerTablet,
                    @SalePricePerPack, @SalePricePerStrip, @SalePricePerTablet,
                    @CurrentStockPacks, @CurrentStockStrips, @CurrentStockTablets,
                    @MinimumStockLevel, @SupplierID, @CreatedDate
                );
                SELECT SCOPE_IDENTITY();";
            
            var parameters = new Dictionary<string, object>
            {
                { "@MedicineName", entity.MedicineName },
                { "@GenericName", entity.GenericName },
                { "@Company", entity.Company },
                { "@BatchNo", entity.BatchNo },
                { "@ExpiryDate", entity.ExpiryDate },
                { "@PackQuantity", entity.PackQuantity },
                { "@StripQuantity", entity.StripQuantity },
                { "@TabletQuantity", entity.TabletQuantity },
                { "@PurchasePricePerPack", entity.PurchasePricePerPack },
                { "@PurchasePricePerStrip", entity.PurchasePricePerStrip },
                { "@PurchasePricePerTablet", entity.PurchasePricePerTablet },
                { "@SalePricePerPack", entity.SalePricePerPack },
                { "@SalePricePerStrip", entity.SalePricePerStrip },
                { "@SalePricePerTablet", entity.SalePricePerTablet },
                { "@CurrentStockPacks", entity.CurrentStockPacks },
                { "@CurrentStockStrips", entity.CurrentStockStrips },
                { "@CurrentStockTablets", entity.CurrentStockTablets },
                { "@MinimumStockLevel", entity.MinimumStockLevel },
                { "@SupplierID", entity.SupplierID },
                { "@CreatedDate", DateTime.Now }
            };
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                AddParameters(command, parameters);
                entity.MedicineID = Convert.ToInt32(command.ExecuteScalar());
            }
            
            return entity;
        }

        public override Medicine Update(Medicine entity)
        {
            var query = @"
                UPDATE Medicines 
                SET MedicineName = @MedicineName, GenericName = @GenericName, Company = @Company,
                    BatchNo = @BatchNo, ExpiryDate = @ExpiryDate,
                    PackQuantity = @PackQuantity, StripQuantity = @StripQuantity, TabletQuantity = @TabletQuantity,
                    PurchasePricePerPack = @PurchasePricePerPack, PurchasePricePerStrip = @PurchasePricePerStrip, PurchasePricePerTablet = @PurchasePricePerTablet,
                    SalePricePerPack = @SalePricePerPack, SalePricePerStrip = @SalePricePerStrip, SalePricePerTablet = @SalePricePerTablet,
                    CurrentStockPacks = @CurrentStockPacks, CurrentStockStrips = @CurrentStockStrips, CurrentStockTablets = @CurrentStockTablets,
                    MinimumStockLevel = @MinimumStockLevel, SupplierID = @SupplierID, ModifiedDate = @ModifiedDate
                WHERE MedicineID = @MedicineID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@MedicineName", entity.MedicineName },
                { "@GenericName", entity.GenericName },
                { "@Company", entity.Company },
                { "@BatchNo", entity.BatchNo },
                { "@ExpiryDate", entity.ExpiryDate },
                { "@PackQuantity", entity.PackQuantity },
                { "@StripQuantity", entity.StripQuantity },
                { "@TabletQuantity", entity.TabletQuantity },
                { "@PurchasePricePerPack", entity.PurchasePricePerPack },
                { "@PurchasePricePerStrip", entity.PurchasePricePerStrip },
                { "@PurchasePricePerTablet", entity.PurchasePricePerTablet },
                { "@SalePricePerPack", entity.SalePricePerPack },
                { "@SalePricePerStrip", entity.SalePricePerStrip },
                { "@SalePricePerTablet", entity.SalePricePerTablet },
                { "@CurrentStockPacks", entity.CurrentStockPacks },
                { "@CurrentStockStrips", entity.CurrentStockStrips },
                { "@CurrentStockTablets", entity.CurrentStockTablets },
                { "@MinimumStockLevel", entity.MinimumStockLevel },
                { "@SupplierID", entity.SupplierID },
                { "@ModifiedDate", DateTime.Now },
                { "@MedicineID", entity.MedicineID }
            };
            
            ExecuteNonQuery(query, parameters);
            return entity;
        }

        public override bool Delete(int id)
        {
            var query = @"
                UPDATE Medicines 
                SET IsDeleted = 1, ModifiedDate = @ModifiedDate 
                WHERE MedicineID = @MedicineID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@MedicineID", id },
                { "@ModifiedDate", DateTime.Now }
            };
            
            return ExecuteNonQuery(query, parameters) > 0;
        }

        protected override Medicine MapFromReader(IDataReader reader)
        {
            return new Medicine
            {
                MedicineID = Convert.ToInt32(reader["MedicineID"]),
                MedicineName = reader["MedicineName"].ToString(),
                GenericName = reader["GenericName"]?.ToString(),
                Company = reader["Company"]?.ToString(),
                BatchNo = reader["BatchNo"]?.ToString(),
                ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiryDate"]) : (DateTime?)null,
                PackQuantity = Convert.ToInt32(reader["PackQuantity"]),
                StripQuantity = Convert.ToInt32(reader["StripQuantity"]),
                TabletQuantity = Convert.ToInt32(reader["TabletQuantity"]),
                PurchasePricePerPack = Convert.ToDecimal(reader["PurchasePricePerPack"]),
                PurchasePricePerStrip = Convert.ToDecimal(reader["PurchasePricePerStrip"]),
                PurchasePricePerTablet = Convert.ToDecimal(reader["PurchasePricePerTablet"]),
                SalePricePerPack = Convert.ToDecimal(reader["SalePricePerPack"]),
                SalePricePerStrip = Convert.ToDecimal(reader["SalePricePerStrip"]),
                SalePricePerTablet = Convert.ToDecimal(reader["SalePricePerTablet"]),
                CurrentStockPacks = Convert.ToInt32(reader["CurrentStockPacks"]),
                CurrentStockStrips = Convert.ToInt32(reader["CurrentStockStrips"]),
                CurrentStockTablets = Convert.ToInt32(reader["CurrentStockTablets"]),
                MinimumStockLevel = Convert.ToInt32(reader["MinimumStockLevel"]),
                SupplierID = reader["SupplierID"] != DBNull.Value ? Convert.ToInt32(reader["SupplierID"]) : (int?)null,
                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedDate"]) : (DateTime?)null
            };
        }

        protected override SqlCommand CreateInsertCommand(SqlConnection connection, Medicine entity)
        {
            var command = new SqlCommand(@"
                INSERT INTO Medicines (
                    MedicineName, GenericName, Company, BatchNo, ExpiryDate,
                    PackQuantity, StripQuantity, TabletQuantity,
                    PurchasePricePerPack, PurchasePricePerStrip, PurchasePricePerTablet,
                    SalePricePerPack, SalePricePerStrip, SalePricePerTablet,
                    CurrentStockPacks, CurrentStockStrips, CurrentStockTablets,
                    MinimumStockLevel, SupplierID, CreatedDate
                )
                VALUES (
                    @MedicineName, @GenericName, @Company, @BatchNo, @ExpiryDate,
                    @PackQuantity, @StripQuantity, @TabletQuantity,
                    @PurchasePricePerPack, @PurchasePricePerStrip, @PurchasePricePerTablet,
                    @SalePricePerPack, @SalePricePerStrip, @SalePricePerTablet,
                    @CurrentStockPacks, @CurrentStockStrips, @CurrentStockTablets,
                    @MinimumStockLevel, @SupplierID, @CreatedDate
                );
                SELECT SCOPE_IDENTITY();", connection);
            
            // Add all parameters
            command.Parameters.AddWithValue("@MedicineName", entity.MedicineName);
            command.Parameters.AddWithValue("@GenericName", entity.GenericName);
            command.Parameters.AddWithValue("@Company", entity.Company);
            command.Parameters.AddWithValue("@BatchNo", entity.BatchNo);
            command.Parameters.AddWithValue("@ExpiryDate", entity.ExpiryDate);
            command.Parameters.AddWithValue("@PackQuantity", entity.PackQuantity);
            command.Parameters.AddWithValue("@StripQuantity", entity.StripQuantity);
            command.Parameters.AddWithValue("@TabletQuantity", entity.TabletQuantity);
            command.Parameters.AddWithValue("@PurchasePricePerPack", entity.PurchasePricePerPack);
            command.Parameters.AddWithValue("@PurchasePricePerStrip", entity.PurchasePricePerStrip);
            command.Parameters.AddWithValue("@PurchasePricePerTablet", entity.PurchasePricePerTablet);
            command.Parameters.AddWithValue("@SalePricePerPack", entity.SalePricePerPack);
            command.Parameters.AddWithValue("@SalePricePerStrip", entity.SalePricePerStrip);
            command.Parameters.AddWithValue("@SalePricePerTablet", entity.SalePricePerTablet);
            command.Parameters.AddWithValue("@CurrentStockPacks", entity.CurrentStockPacks);
            command.Parameters.AddWithValue("@CurrentStockStrips", entity.CurrentStockStrips);
            command.Parameters.AddWithValue("@CurrentStockTablets", entity.CurrentStockTablets);
            command.Parameters.AddWithValue("@MinimumStockLevel", entity.MinimumStockLevel);
            command.Parameters.AddWithValue("@SupplierID", entity.SupplierID);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            
            return command;
        }

        protected override SqlCommand CreateUpdateCommand(SqlConnection connection, Medicine entity)
        {
            var command = new SqlCommand(@"
                UPDATE Medicines 
                SET MedicineName = @MedicineName, GenericName = @GenericName, Company = @Company,
                    BatchNo = @BatchNo, ExpiryDate = @ExpiryDate,
                    PackQuantity = @PackQuantity, StripQuantity = @StripQuantity, TabletQuantity = @TabletQuantity,
                    PurchasePricePerPack = @PurchasePricePerPack, PurchasePricePerStrip = @PurchasePricePerStrip, PurchasePricePerTablet = @PurchasePricePerTablet,
                    SalePricePerPack = @SalePricePerPack, SalePricePerStrip = @SalePricePerStrip, SalePricePerTablet = @SalePricePerTablet,
                    CurrentStockPacks = @CurrentStockPacks, CurrentStockStrips = @CurrentStockStrips, CurrentStockTablets = @CurrentStockTablets,
                    MinimumStockLevel = @MinimumStockLevel, SupplierID = @SupplierID, ModifiedDate = @ModifiedDate
                WHERE MedicineID = @MedicineID", connection);
            
            // Add all parameters
            command.Parameters.AddWithValue("@MedicineName", entity.MedicineName);
            command.Parameters.AddWithValue("@GenericName", entity.GenericName);
            command.Parameters.AddWithValue("@Company", entity.Company);
            command.Parameters.AddWithValue("@BatchNo", entity.BatchNo);
            command.Parameters.AddWithValue("@ExpiryDate", entity.ExpiryDate);
            command.Parameters.AddWithValue("@PackQuantity", entity.PackQuantity);
            command.Parameters.AddWithValue("@StripQuantity", entity.StripQuantity);
            command.Parameters.AddWithValue("@TabletQuantity", entity.TabletQuantity);
            command.Parameters.AddWithValue("@PurchasePricePerPack", entity.PurchasePricePerPack);
            command.Parameters.AddWithValue("@PurchasePricePerStrip", entity.PurchasePricePerStrip);
            command.Parameters.AddWithValue("@PurchasePricePerTablet", entity.PurchasePricePerTablet);
            command.Parameters.AddWithValue("@SalePricePerPack", entity.SalePricePerPack);
            command.Parameters.AddWithValue("@SalePricePerStrip", entity.SalePricePerStrip);
            command.Parameters.AddWithValue("@SalePricePerTablet", entity.SalePricePerTablet);
            command.Parameters.AddWithValue("@CurrentStockPacks", entity.CurrentStockPacks);
            command.Parameters.AddWithValue("@CurrentStockStrips", entity.CurrentStockStrips);
            command.Parameters.AddWithValue("@CurrentStockTablets", entity.CurrentStockTablets);
            command.Parameters.AddWithValue("@MinimumStockLevel", entity.MinimumStockLevel);
            command.Parameters.AddWithValue("@SupplierID", entity.SupplierID);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            command.Parameters.AddWithValue("@MedicineID", entity.MedicineID);
            
            return command;
        }

        public int GetNextMedicineId()
        {
            var query = "sp_GetNextMedicineId";
            
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
        public void UpdateStock(int medicineId, int packs, int strips, int tablets)
        {
            var query = @"
                UPDATE Medicines 
                SET CurrentStockPacks = @CurrentStockPacks, 
                    CurrentStockStrips = @CurrentStockStrips, 
                    CurrentStockTablets = @CurrentStockTablets,
                    ModifiedDate = @ModifiedDate
                WHERE MedicineID = @MedicineID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@MedicineID", medicineId },
                { "@CurrentStockPacks", packs },
                { "@CurrentStockStrips", strips },
                { "@CurrentStockTablets", tablets },
                { "@ModifiedDate", DateTime.Now }
            };
            
            ExecuteNonQuery(query, parameters);
        }

        public override IEnumerable<Medicine> GetByFilter(string filter)
        {
            var query = @"
                SELECT m.*, s.SupplierName 
                FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.IsDeleted = 0 
                AND (m.MedicineName LIKE @Filter OR m.GenericName LIKE @Filter OR m.Company LIKE @Filter OR s.SupplierName LIKE @Filter)
                ORDER BY m.MedicineName";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return ExecuteReaderQuery(query, parameters);
        }

        public override int Count()
        {
            var query = "SELECT COUNT(*) FROM Medicines WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public override int CountByFilter(string filter)
        {
            var query = @"
                SELECT COUNT(*) FROM Medicines m 
                LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID 
                WHERE m.IsDeleted = 0 
                AND (m.MedicineName LIKE @Filter OR m.GenericName LIKE @Filter OR m.Company LIKE @Filter OR s.SupplierName LIKE @Filter)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@Filter", $"%{filter}%" }
            };
            
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public override bool Delete(Medicine entity)
        {
            return Delete(entity.MedicineID);
        }

        public int GetTotalMedicinesCount()
        {
            var query = "SELECT COUNT(*) FROM Medicines WHERE IsDeleted = 0";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        public decimal GetTotalInventoryValue()
        {
            var query = @"
                SELECT SUM(CurrentStockPacks * SalePricePerPack) 
                FROM Medicines 
                WHERE IsDeleted = 0";
            
            var result = ExecuteScalar(query);
            return result != null ? Convert.ToDecimal(result) : 0;
        }
    }
}

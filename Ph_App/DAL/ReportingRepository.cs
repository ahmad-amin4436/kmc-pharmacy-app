using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Ph_App.Database;

namespace Ph_App.DAL
{
    public static class ReportingRepository
    {
        private static readonly string _connectionString = DatabaseConfig.ConnectionString;

        // Sales Reports
        public static DataTable GetDailySalesReport(DateTime date)
        {
            var query = "sp_GetDailySalesReport";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@SaleDate", date);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetProfitReport(DateTime fromDate, DateTime toDate)
        {
            var query = "sp_GetProfitReport";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetExpiryReport(int monthsAhead = 3)
        {
            var query = "sp_GetExpiryReport";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@MonthsAhead", monthsAhead);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetLowStockReport()
        {
            var query = "sp_GetLowStockReport";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        // Custom Reports
        public static DataTable GetSalesByEmployee(DateTime fromDate, DateTime toDate)
        {
            var query = @"
                SELECT 
                    u.Username,
                    COUNT(s.SaleID) AS TotalSales,
                    SUM(s.NetAmount) AS TotalRevenue,
                    AVG(s.NetAmount) AS AverageSale,
                    MAX(s.NetAmount) AS HighestSale,
                    MIN(s.NetAmount) AS LowestSale
                FROM Sales s
                INNER JOIN Users u ON s.UserID = u.UserID
                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                    AND s.IsDeleted = 0
                GROUP BY u.UserID, u.Username
                ORDER BY TotalRevenue DESC";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetCustomerSalesReport(DateTime fromDate, DateTime toDate, int topCustomers = 20)
        {
            var query = @"
                SELECT TOP (@TopCustomers)
                    c.CustomerName,
                    c.Phone,
                    COUNT(s.SaleID) AS PurchaseCount,
                    SUM(s.NetAmount) AS TotalSpent,
                    AVG(s.NetAmount) AS AveragePurchase,
                    MAX(s.SaleDate) AS LastPurchaseDate
                FROM Sales s
                LEFT JOIN Customers c ON s.CustomerID = c.CustomerID
                WHERE s.SaleDate BETWEEN @FromDate AND @ToDate
                    AND s.IsDeleted = 0
                GROUP BY c.CustomerID, c.CustomerName, c.Phone
                HAVING COUNT(s.SaleID) > 0
                ORDER BY TotalSpent DESC";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                command.Parameters.AddWithValue("@TopCustomers", topCustomers);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetMonthlyTrendReport(int months = 12)
        {
            var query = @"
                SELECT 
                    YEAR(s.SaleDate) AS Year,
                    MONTH(s.SaleDate) AS Month,
                    DATENAME(MONTH, s.SaleDate) AS MonthName,
                    COUNT(s.SaleID) AS TotalSales,
                    SUM(s.NetAmount) AS TotalRevenue,
                    SUM(s.Discount) AS TotalDiscount,
                    AVG(s.NetAmount) AS AverageSale
                FROM Sales s
                WHERE s.SaleDate >= DATEADD(MONTH, -@Months, GETDATE())
                    AND s.IsDeleted = 0
                GROUP BY YEAR(s.SaleDate), MONTH(s.SaleDate), DATENAME(MONTH, s.SaleDate)
                ORDER BY Year, Month";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Months", months);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public static DataTable GetSupplierPerformanceReport(DateTime fromDate, DateTime toDate)
        {
            var query = @"
                SELECT 
                    s.SupplierName,
                    COUNT(DISTINCT p.PurchaseID) AS PurchaseCount,
                    SUM(p.NetAmount) AS TotalPurchases,
                    AVG(p.NetAmount) AS AveragePurchase,
                    COUNT(DISTINCT pd.MedicineID) AS UniqueMedicines,
                    SUM(pd.QuantityPacks + pd.QuantityStrips + pd.QuantityTablets) AS TotalItems
                FROM Purchases p
                INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID
                INNER JOIN PurchaseDetails pd ON p.PurchaseID = pd.PurchaseID
                WHERE p.PurchaseDate BETWEEN @FromDate AND @ToDate
                    AND p.IsDeleted = 0
                GROUP BY s.SupplierID, s.SupplierName
                ORDER BY TotalPurchases DESC";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        // Dashboard Summary Data
        public static DashboardSummary GetDashboardSummary()
        {
            var summary = new DashboardSummary();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                // Today's Sales
                var todayQuery = @"
                    SELECT 
                        COUNT(*) AS SalesCount,
                        ISNULL(SUM(NetAmount), 0) AS TotalSales,
                        ISNULL(SUM(PaidAmount), 0) AS TotalReceived
                    FROM Sales 
                    WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE) 
                        AND IsDeleted = 0";
                
                using (var command = new SqlCommand(todayQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary.TodaySalesCount = Convert.ToInt32(reader["SalesCount"]);
                        summary.TodaySalesAmount = Convert.ToDecimal(reader["TotalSales"]);
                        summary.TodayReceivedAmount = Convert.ToDecimal(reader["TotalReceived"]);
                    }
                }
                
                // Low Stock Count
                var lowStockQuery = @"
                    SELECT COUNT(*) 
                    FROM Medicines 
                    WHERE IsDeleted = 0 
                        AND CurrentStockPacks <= MinimumStockLevel";
                
                summary.LowStockCount = Convert.ToInt32(new SqlCommand(lowStockQuery, connection).ExecuteScalar());
                
                // Expiring Soon Count
                var expiryQuery = @"
                    SELECT COUNT(*) 
                    FROM Medicines 
                    WHERE IsDeleted = 0 
                        AND ExpiryDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE())
                        AND (CurrentStockPacks + CurrentStockStrips + CurrentStockTablets) > 0";
                
                summary.ExpiringSoonCount = Convert.ToInt32(new SqlCommand(expiryQuery, connection).ExecuteScalar());
                
                // Total Medicines
                var medicineCountQuery = "SELECT COUNT(*) FROM Medicines WHERE IsDeleted = 0";
                summary.TotalMedicines = Convert.ToInt32(new SqlCommand(medicineCountQuery, connection).ExecuteScalar());
                
                // Total Customers
                var customerCountQuery = "SELECT COUNT(*) FROM Customers WHERE IsDeleted = 0";
                summary.TotalCustomers = Convert.ToInt32(new SqlCommand(customerCountQuery, connection).ExecuteScalar());
                
                // Total Suppliers
                var supplierCountQuery = "SELECT COUNT(*) FROM Suppliers WHERE IsDeleted = 0";
                summary.TotalSuppliers = Convert.ToInt32(new SqlCommand(supplierCountQuery, connection).ExecuteScalar());
            }
            
            return summary;
        }

        // Async versions
        public static async Task<DataTable> GetDailySalesReportAsync(DateTime date)
        {
            var query = "sp_GetDailySalesReport";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@SaleDate", date);
                
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    await Task.Run(() => adapter.Fill(table));
                    return table;
                }
            }
        }

        public static async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var summary = new DashboardSummary();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                // Today's Sales
                var todayQuery = @"
                    SELECT 
                        COUNT(*) AS SalesCount,
                        ISNULL(SUM(NetAmount), 0) AS TotalSales,
                        ISNULL(SUM(PaidAmount), 0) AS TotalReceived
                    FROM Sales 
                    WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE) 
                        AND IsDeleted = 0";
                
                using (var command = new SqlCommand(todayQuery, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        summary.TodaySalesCount = Convert.ToInt32(reader["SalesCount"]);
                        summary.TodaySalesAmount = Convert.ToDecimal(reader["TotalSales"]);
                        summary.TodayReceivedAmount = Convert.ToDecimal(reader["TotalReceived"]);
                    }
                }
                
                // Low Stock Count
                var lowStockQuery = @"
                    SELECT COUNT(*) 
                    FROM Medicines 
                    WHERE IsDeleted = 0 
                        AND CurrentStockPacks <= MinimumStockLevel";
                
                summary.LowStockCount = Convert.ToInt32(await new SqlCommand(lowStockQuery, connection).ExecuteScalarAsync());
                
                // Expiring Soon Count
                var expiryQuery = @"
                    SELECT COUNT(*) 
                    FROM Medicines 
                    WHERE IsDeleted = 0 
                        AND ExpiryDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE())
                        AND (CurrentStockPacks + CurrentStockStrips + CurrentStockTablets) > 0";
                
                summary.ExpiringSoonCount = Convert.ToInt32(await new SqlCommand(expiryQuery, connection).ExecuteScalarAsync());
                
                // Total Medicines
                var medicineCountQuery = "SELECT COUNT(*) FROM Medicines WHERE IsDeleted = 0";
                summary.TotalMedicines = Convert.ToInt32(await new SqlCommand(medicineCountQuery, connection).ExecuteScalarAsync());
                
                // Total Customers
                var customerCountQuery = "SELECT COUNT(*) FROM Customers WHERE IsDeleted = 0";
                summary.TotalCustomers = Convert.ToInt32(await new SqlCommand(customerCountQuery, connection).ExecuteScalarAsync());
                
                // Total Suppliers
                var supplierCountQuery = "SELECT COUNT(*) FROM Suppliers WHERE IsDeleted = 0";
                summary.TotalSuppliers = Convert.ToInt32(await new SqlCommand(supplierCountQuery, connection).ExecuteScalarAsync());
            }
            
            return summary;
        }
    }

    public class DashboardSummary
    {
        public int TodaySalesCount { get; set; }
        public decimal TodaySalesAmount { get; set; }
        public decimal TodayReceivedAmount { get; set; }
        public int LowStockCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public int TotalMedicines { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSuppliers { get; set; }
    }
}

using System;
using System.Linq;
using Ph_App.DAL;
using Ph_App.Models;

namespace Ph_App.Database
{
    public class PharmacyDBContext
    {
        // Repository instances - static properties
        public static UserRepository Users { get; private set; }
        public static MedicineRepository Medicines { get; private set; }
        public static SalesRepository Sales { get; private set; }
        public static CustomerRepository Customers { get; private set; }
        public static SupplierRepository Suppliers { get; private set; }
        public static PurchaseRepository Purchases { get; private set; }
        public static AuditLogRepository AuditLogs { get; private set; }
    // Currently logged-in user (set at login)
    public static Ph_App.Models.User CurrentUser { get; set; }

        // Singleton pattern for database context
        private static PharmacyDBContext _instance;
        private static readonly object _lock = new object();

        public static PharmacyDBContext Instance
        {
            get
            {
                // Ensure instance is created before accessing repositories
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = new PharmacyDBContext();
                    }
                }
                return _instance;
            }
        }

        private PharmacyDBContext()
        {
            // Initialize repositories first
            Users = new UserRepository();
            Medicines = new MedicineRepository();
            Sales = new SalesRepository();
            Customers = new CustomerRepository();
            Suppliers = new SupplierRepository();
            Purchases = new PurchaseRepository();
            AuditLogs = new AuditLogRepository();
        }

        // Static initialization method
        public static void Initialize()
        {
            if (Instance == null)
            {
                //Instance = new PharmacyDBContext();
            }
        }

        // Test database connection
        public bool TestConnection()
        {
            return DatabaseConfig.TestConnection();
        }

        // Initialize database with sample data
        public void InitializeSampleData()
        {
            try
            {
                // Add sample users
                Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BLL.AuthService.HashPassword("admin123"),
                    Role = "Admin",
                    CreatedDate = DateTime.Now
                });

                Users.Add(new User
                {
                    Username = "cashier",
                    PasswordHash = BLL.AuthService.HashPassword("cashier123"),
                    Role = "Cashier",
                    CreatedDate = DateTime.Now
                });

                // Add sample medicines
                Medicines.Add(new Medicine
                {
                    MedicineName = "Paracetamol",
                    GenericName = "Acetaminophen",
                    Company = "ABC Pharma",
                    BatchNo = "BATCH001",
                    ExpiryDate = DateTime.Now.AddYears(2),
                    PackQuantity = 10,
                    StripQuantity = 10,
                    TabletQuantity = 100,
                    PurchasePricePerPack = 50.00m,
                    SalePricePerPack = 60.00m,
                    CurrentStockPacks = 100,
                    CurrentStockStrips = 100,
                    CurrentStockTablets = 1000,
                    MinimumStockLevel = 20,
                    CreatedDate = DateTime.Now
                });

                Medicines.Add(new Medicine
                {
                    MedicineName = "Aspirin",
                    GenericName = "Acetylsalicylic Acid",
                    Company = "XYZ Pharma",
                    BatchNo = "BATCH002",
                    ExpiryDate = DateTime.Now.AddYears(1),
                    PackQuantity = 20,
                    StripQuantity = 10,
                    TabletQuantity = 10,
                    PurchasePricePerPack = 30.00m,
                    SalePricePerPack = 40.00m,
                    CurrentStockPacks = 50,
                    CurrentStockStrips = 50,
                    CurrentStockTablets = 50,
                    MinimumStockLevel = 5,
                    CreatedDate = DateTime.Now
                });

                // Get admin user for logging
                var adminUser = Users.GetByUsername("admin");

                // Log initialization
                AuditLogs.LogUserAction(adminUser.UserID, "SYSTEM", "Database", "INIT", "", "Database initialized with sample data");
            }
            catch (Exception ex)
            {
                // Log initialization error
                AuditLogs.LogUserAction(null, "ERROR", "Database", "INIT", "", $"Database initialization failed: {ex.Message}");
            }
        }

        // Utility methods to replace InMemoryDataStore methods
        public int GetNextUserId()
        {
            return Users.GetNextUserId();
        }

        public int GetNextMedicineId()
        {
            return Medicines.GetNextMedicineId();
        }

        public int GetNextSaleId()
        {
            return Sales.GetNextSaleId();
        }

        public int GetNextCustomerId()
        {
            return Customers.GetNextCustomerId();
        }

        public int GetNextSupplierId()
        {
            return Suppliers.GetNextSupplierId();
        }

        public int GetNextPurchaseId()
        {
            return Purchases.GetNextPurchaseId();
        }

        public int GetNextAuditLogId()
        {
            return AuditLogs.GetNextAuditLogId();
        }

        // Dispose method for cleanup
        public void Dispose()
        {
            // No explicit disposal needed for repositories
            // They handle their own connections via using statements
        }
    }
}

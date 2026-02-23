-- Pharmacy Management System Database Schema
-- Complete ADO.NET Implementation Schema

-- Drop existing database if it exists
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'PharmacyDB')
BEGIN
    ALTER DATABASE PharmacyDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PharmacyDB;
END
GO

-- Create the database
CREATE DATABASE PharmacyDB
GO

USE PharmacyDB
GO

-- Create Tables

-- Users Table
CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Cashier')),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

-- Suppliers Table
CREATE TABLE Suppliers
(
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(50) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

-- Customers Table
CREATE TABLE Customers
(
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    Contact NVARCHAR(20) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

-- Medicines Table
CREATE TABLE Medicines
(
    MedicineID INT IDENTITY(1,1) PRIMARY KEY,
    MedicineName NVARCHAR(100) NOT NULL,
    GenericName NVARCHAR(100) NULL,
    Company NVARCHAR(100) NULL,
    BatchNo NVARCHAR(50) NULL,
    ExpiryDate DATE NULL,
    PackQuantity INT NOT NULL DEFAULT 1,
    StripQuantity INT NOT NULL DEFAULT 1,
    TabletQuantity INT NOT NULL DEFAULT 1,
    PurchasePricePerPack DECIMAL(18,2) NOT NULL DEFAULT 0,
    PurchasePricePerStrip DECIMAL(18,2) NOT NULL DEFAULT 0,
    PurchasePricePerTablet DECIMAL(18,2) NOT NULL DEFAULT 0,
    SalePricePerPack DECIMAL(18,2) NOT NULL DEFAULT 0,
    SalePricePerStrip DECIMAL(18,2) NOT NULL DEFAULT 0,
    SalePricePerTablet DECIMAL(18,2) NOT NULL DEFAULT 0,
    CurrentStockPacks INT NOT NULL DEFAULT 0,
    CurrentStockStrips INT NOT NULL DEFAULT 0,
    CurrentStockTablets INT NOT NULL DEFAULT 0,
    MinimumStockLevel INT NOT NULL DEFAULT 0,
    SupplierID INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Medicines_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);
GO

-- Purchases Table
CREATE TABLE Purchases
(
    PurchaseID INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseDate DATETIME NOT NULL DEFAULT GETDATE(),
    InvoiceNo NVARCHAR(50) NOT NULL UNIQUE,
    SupplierID INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    Discount DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    PaymentMethod NVARCHAR(20) NULL,
    Notes NVARCHAR(500) NULL,
    UserID INT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Purchases_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
    CONSTRAINT FK_Purchases_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- PurchaseDetails Table
CREATE TABLE PurchaseDetails
(
    PurchaseDetailID INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseID INT NOT NULL,
    MedicineID INT NOT NULL,
    QuantityPacks INT NOT NULL DEFAULT 0,
    QuantityStrips INT NOT NULL DEFAULT 0,
    QuantityTablets INT NOT NULL DEFAULT 0,
    PurchasePrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_PurchaseDetails_Purchases FOREIGN KEY (PurchaseID) REFERENCES Purchases(PurchaseID),
    CONSTRAINT FK_PurchaseDetails_Medicines FOREIGN KEY (MedicineID) REFERENCES Medicines(MedicineID)
);
GO

-- Sales Table
CREATE TABLE Sales
(
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
    InvoiceNo NVARCHAR(50) NOT NULL UNIQUE,
    CustomerID INT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    Discount DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    ChangeAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    PaymentMethod NVARCHAR(20) NULL,
    Notes NVARCHAR(500) NULL,
    UserID INT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Sales_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_Sales_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- SaleDetails Table
CREATE TABLE SaleDetails
(
    SaleDetailID INT IDENTITY(1,1) PRIMARY KEY,
    SaleID INT NOT NULL,
    MedicineID INT NOT NULL,
    QuantityPacks INT NOT NULL DEFAULT 0,
    QuantityStrips INT NOT NULL DEFAULT 0,
    QuantityTablets INT NOT NULL DEFAULT 0,
    SalePrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SaleDetails_Sales FOREIGN KEY (SaleID) REFERENCES Sales(SaleID),
    CONSTRAINT FK_SaleDetails_Medicines FOREIGN KEY (MedicineID) REFERENCES Medicines(MedicineID)
);
GO

-- AuditLogs Table
CREATE TABLE AuditLogs
(
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NULL,
    ActionType NVARCHAR(50) NOT NULL,
    TableAffected NVARCHAR(50) NOT NULL,
    RecordID NVARCHAR(50) NULL,
    OldValue NVARCHAR(MAX) NULL,
    NewValue NVARCHAR(MAX) NULL,
    ActionDateTime DATETIME NOT NULL DEFAULT GETDATE(),
    IPAddress NVARCHAR(50) NULL,
    UserAgent NVARCHAR(500) NULL,
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- Create Indexes for Performance
CREATE INDEX IX_Medicines_MedicineName ON Medicines(MedicineName) WHERE IsDeleted = 0;
CREATE INDEX IX_Medicines_ExpiryDate ON Medicines(ExpiryDate) WHERE IsDeleted = 0;
CREATE INDEX IX_Medicines_CurrentStock ON Medicines(CurrentStockPacks) WHERE IsDeleted = 0;
CREATE INDEX IX_Sales_SaleDate ON Sales(SaleDate) WHERE IsDeleted = 0;
CREATE INDEX IX_Sales_InvoiceNo ON Sales(InvoiceNo) WHERE IsDeleted = 0;
CREATE INDEX IX_Purchases_PurchaseDate ON Purchases(PurchaseDate) WHERE IsDeleted = 0;
CREATE INDEX IX_AuditLogs_ActionDateTime ON AuditLogs(ActionDateTime);
CREATE INDEX IX_Customers_CustomerName ON Customers(CustomerName) WHERE IsDeleted = 0;
CREATE INDEX IX_Suppliers_SupplierName ON Suppliers(SupplierName) WHERE IsDeleted = 0;
GO

-- Create Stored Procedures for Performance

-- Get Next ID Procedures
CREATE PROCEDURE sp_GetNextUserId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(UserID), 0) + 1 FROM Users;
END
GO

CREATE PROCEDURE sp_GetNextMedicineId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(MedicineID), 0) + 1 FROM Medicines;
END
GO

CREATE PROCEDURE sp_GetNextSaleId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(SaleID), 0) + 1 FROM Sales;
END
GO

CREATE PROCEDURE sp_GetNextPurchaseId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(PurchaseID), 0) + 1 FROM Purchases;
END
GO

CREATE PROCEDURE sp_GetNextCustomerId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(CustomerID), 0) + 1 FROM Customers;
END
GO

CREATE PROCEDURE sp_GetNextSupplierId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(SupplierID), 0) + 1 FROM Suppliers;
END
GO

CREATE PROCEDURE sp_GetNextAuditLogId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(LogID), 0) + 1 FROM AuditLogs;
END
GO

CREATE PROCEDURE sp_GetNextSaleDetailId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(SaleDetailID), 0) + 1 FROM SaleDetails;
END
GO

CREATE PROCEDURE sp_GetNextPurchaseDetailId
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(MAX(PurchaseDetailID), 0) + 1 FROM PurchaseDetails;
END
GO

-- Sales Reports Procedures
CREATE PROCEDURE sp_GetDailySalesReport
    @SaleDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        COUNT(*) AS TotalSales,
        SUM(TotalAmount) AS GrossSales,
        SUM(Discount) AS TotalDiscount,
        SUM(NetAmount) AS NetSales,
        SUM(PaidAmount) AS TotalReceived
    FROM Sales 
    WHERE CAST(SaleDate AS DATE) = @SaleDate AND IsDeleted = 0;
END
GO

CREATE PROCEDURE sp_GetProfitReport
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        m.MedicineName,
        SUM(sd.QuantityPacks) AS TotalPacksSold,
        SUM(sd.QuantityStrips) AS TotalStripsSold,
        SUM(sd.QuantityTablets) AS TotalTabletsSold,
        SUM(sd.Total) AS TotalSales,
        SUM(sd.QuantityPacks * m.PurchasePricePerPack + 
            sd.QuantityStrips * m.PurchasePricePerStrip + 
            sd.QuantityTablets * m.PurchasePricePerTablet) AS TotalCost,
        SUM(sd.Total) - SUM(sd.QuantityPacks * m.PurchasePricePerPack + 
            sd.QuantityStrips * m.PurchasePricePerStrip + 
            sd.QuantityTablets * m.PurchasePricePerTablet) AS Profit
    FROM SaleDetails sd
    INNER JOIN Sales s ON sd.SaleID = s.SaleID
    INNER JOIN Medicines m ON sd.MedicineID = m.MedicineID
    WHERE CAST(s.SaleDate AS DATE) BETWEEN @FromDate AND @ToDate 
        AND s.IsDeleted = 0 
        AND m.IsDeleted = 0
    GROUP BY m.MedicineName
    ORDER BY Profit DESC;
END
GO

CREATE PROCEDURE sp_GetExpiryReport
    @MonthsAhead INT = 3
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MedicineName,
        GenericName,
        Company,
        BatchNo,
        ExpiryDate,
        CurrentStockPacks,
        CurrentStockStrips,
        CurrentStockTablets,
        DATEDIFF(MONTH, GETDATE(), ExpiryDate) AS MonthsToExpiry
    FROM Medicines 
    WHERE IsDeleted = 0 
        AND ExpiryDate IS NOT NULL
        AND ExpiryDate <= DATEADD(MONTH, @MonthsAhead, GETDATE())
        AND CurrentStockPacks + CurrentStockStrips + CurrentStockTablets > 0
    ORDER BY ExpiryDate;
END
GO

CREATE PROCEDURE sp_GetLowStockReport
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        MedicineName,
        GenericName,
        Company,
        CurrentStockPacks,
        CurrentStockStrips,
        CurrentStockTablets,
        MinimumStockLevel,
        (CurrentStockPacks + CurrentStockStrips + CurrentStockTablets) AS TotalStock
    FROM Medicines 
    WHERE IsDeleted = 0 
        AND CurrentStockPacks <= MinimumStockLevel
    ORDER BY TotalStock;
END
GO

-- Create Views for Common Queries
CREATE VIEW vActiveMedicines AS
SELECT 
    m.MedicineID,
    m.MedicineName,
    m.GenericName,
    m.Company,
    m.BatchNo,
    m.ExpiryDate,
    m.PackQuantity,
    m.StripQuantity,
    m.TabletQuantity,
    m.PurchasePricePerPack,
    m.PurchasePricePerStrip,
    m.PurchasePricePerTablet,
    m.SalePricePerPack,
    m.SalePricePerStrip,
    m.SalePricePerTablet,
    m.CurrentStockPacks,
    m.CurrentStockStrips,
    m.CurrentStockTablets,
    m.MinimumStockLevel,
    m.SupplierID,
    s.SupplierName
FROM Medicines m
LEFT JOIN Suppliers s ON m.SupplierID = s.SupplierID
WHERE m.IsDeleted = 0;
GO

CREATE VIEW vActiveSales AS
SELECT 
    s.SaleID,
    s.SaleDate,
    s.InvoiceNo,
    s.TotalAmount,
    s.Discount,
    s.NetAmount,
    s.PaidAmount,
    s.ChangeAmount,
    s.PaymentMethod,
    s.Notes,
    s.CustomerID,
    c.CustomerName,
    s.UserID,
    u.Username,
    s.CreatedDate
FROM Sales s
LEFT JOIN Customers c ON s.CustomerID = c.CustomerID
LEFT JOIN Users u ON s.UserID = u.UserID
WHERE s.IsDeleted = 0;
GO

CREATE VIEW vActiveCustomers AS
SELECT 
    CustomerID,
    CustomerName,
    Contact,
    Phone,
    Email,
    Address,
    Balance
FROM Customers 
WHERE IsDeleted = 0;
GO

CREATE VIEW vActiveSuppliers AS
SELECT 
    SupplierID,
    SupplierName,
    ContactPerson,
    Phone,
    Email,
    Address
FROM Suppliers 
WHERE IsDeleted = 0;
GO

-- Create Triggers for Audit Logging
CREATE TRIGGER tr_Users_Audit
ON Users
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @UserID INT;
    DECLARE @OldValue NVARCHAR(MAX);
    DECLARE @NewValue NVARCHAR(MAX);
    
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- UPDATE
        SET @UserID = (SELECT TOP 1 UserID FROM inserted);
        SET @OldValue = (SELECT * FROM deleted FOR XML RAW);
        SET @NewValue = (SELECT * FROM inserted FOR XML RAW);
        
        INSERT INTO AuditLogs (UserID, ActionType, TableAffected, RecordID, OldValue, NewValue)
        VALUES (@UserID, 'UPDATE', 'Users', CAST(@UserID AS NVARCHAR(50)), @OldValue, @NewValue);
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- INSERT
        SET @UserID = (SELECT TOP 1 UserID FROM inserted);
        SET @NewValue = (SELECT * FROM inserted FOR XML RAW);
        
        INSERT INTO AuditLogs (UserID, ActionType, TableAffected, RecordID, NewValue)
        VALUES (@UserID, 'INSERT', 'Users', CAST(@UserID AS NVARCHAR(50)), @NewValue);
    END
    ELSE IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- DELETE
        SET @UserID = (SELECT TOP 1 UserID FROM deleted);
        SET @OldValue = (SELECT * FROM deleted FOR XML RAW);
        
        INSERT INTO AuditLogs (UserID, ActionType, TableAffected, RecordID, OldValue)
        VALUES (@UserID, 'DELETE', 'Users', CAST(@UserID AS NVARCHAR(50)), @OldValue);
    END
END
GO

CREATE TRIGGER tr_Medicines_Audit
ON Medicines
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MedicineID INT;
    DECLARE @OldValue NVARCHAR(MAX);
    DECLARE @NewValue NVARCHAR(MAX);
    
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- UPDATE
        SET @MedicineID = (SELECT TOP 1 MedicineID FROM inserted);
        SET @OldValue = (SELECT * FROM deleted FOR XML RAW);
        SET @NewValue = (SELECT * FROM inserted FOR XML RAW);
        
        INSERT INTO AuditLogs (ActionType, TableAffected, RecordID, OldValue, NewValue)
        VALUES ('UPDATE', 'Medicines', CAST(@MedicineID AS NVARCHAR(50)), @OldValue, @NewValue);
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- INSERT
        SET @MedicineID = (SELECT TOP 1 MedicineID FROM inserted);
        SET @NewValue = (SELECT * FROM inserted FOR XML RAW);
        
        INSERT INTO AuditLogs (ActionType, TableAffected, RecordID, NewValue)
        VALUES ('INSERT', 'Medicines', CAST(@MedicineID AS NVARCHAR(50)), @NewValue);
    END
    ELSE IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- DELETE
        SET @MedicineID = (SELECT TOP 1 MedicineID FROM deleted);
        SET @OldValue = (SELECT * FROM deleted FOR XML RAW);
        
        INSERT INTO AuditLogs (ActionType, TableAffected, RecordID, OldValue)
        VALUES ('DELETE', 'Medicines', CAST(@MedicineID AS NVARCHAR(50)), @OldValue);
    END
END
GO

-- Insert Sample Data
INSERT INTO Users (Username, PasswordHash, Role) VALUES 
('admin', 'AQAAAAEAACcQAAAAEKqgkTJF9hFwU6VqLJQZ5fFJ5gY4sTJk8hFwU6VqLJQZ5fFJ5gY=', 'Admin'),
('cashier', 'AQAAAAEAACcQAAAAEKqgkTJF9hFwU6VqLJQZ5fFJ5gY4sTJk8hFwU6VqLJQZ5fFJ5gY=', 'Cashier');
GO

INSERT INTO Suppliers (SupplierName, ContactPerson, Phone, Email) VALUES 
('ABC Pharma', 'John Doe', '1234567890', 'info@abcpharma.com'),
('XYZ Pharma', 'Jane Smith', '9876543210', 'info@xyzpharma.com');
GO

INSERT INTO Customers (CustomerName, Phone, Email) VALUES 
('Walk-in Customer', '0000000000', 'walkin@pharmacy.com'),
('Regular Customer', '1111111111', 'regular@pharmacy.com');
GO

PRINT 'Pharmacy Database Schema Created Successfully!'

using System;

namespace Ph_App.Models
{
 public class Medicine
 {
 public int MedicineID { get; set; }
 public string MedicineName { get; set; }
 public string GenericName { get; set; }
 public string Company { get; set; }
 public string BatchNo { get; set; }
 public DateTime? ExpiryDate { get; set; }
 public int PackQuantity { get; set; }
 public int StripQuantity { get; set; }
 public int TabletQuantity { get; set; }
 public decimal PurchasePricePerPack { get; set; }
 public decimal PurchasePricePerStrip { get; set; }
 public decimal PurchasePricePerTablet { get; set; }
 public decimal SalePricePerPack { get; set; }
 public decimal SalePricePerStrip { get; set; }
 public decimal SalePricePerTablet { get; set; }
 public int CurrentStockPacks { get; set; }
 public int CurrentStockStrips { get; set; }
 public int CurrentStockTablets { get; set; }
 public int MinimumStockLevel { get; set; }
 public int? SupplierID { get; set; }
 public bool IsDeleted { get; set; }
 public DateTime CreatedDate { get; set; }
 public DateTime? ModifiedDate { get; set; }
 }
}
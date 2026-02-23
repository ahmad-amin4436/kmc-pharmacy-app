using System;
using System.Collections.Generic;

namespace Ph_App.Models
{
 public class Purchase
 {
 public int PurchaseID { get; set; }
 public int SupplierID { get; set; }
 public int UserID { get; set; }
 public DateTime PurchaseDate { get; set; }
 public string InvoiceNo { get; set; }
 public decimal TotalAmount { get; set; }
 public decimal Discount { get; set; }
 public decimal NetAmount { get; set; }
 public decimal PaidAmount { get; set; }
 public string PaymentMethod { get; set; }
 public string Notes { get; set; }
 public bool IsDeleted { get; set; }
 public DateTime CreatedDate { get; set; }
 public DateTime? ModifiedDate { get; set; }
 public List<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
 }

 public class PurchaseDetail
 {
 public int PurchaseDetailID { get; set; }
 public int PurchaseID { get; set; }
 public int MedicineID { get; set; }
 public int QuantityPacks { get; set; }
 public int QuantityStrips { get; set; }
 public int QuantityTablets { get; set; }
 public decimal PurchasePrice { get; set; }
 public decimal Total { get; set; }
 public DateTime? ExpiryDate { get; set; }
 public string BatchNo { get; set; }
 public DateTime CreatedDate { get; set; }
 }
}
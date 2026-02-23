using System;
using System.Collections.Generic;

namespace Ph_App.Models
{
 public class Sale
 {
 public int SaleID { get; set; }
 public int? CustomerID { get; set; }
 public DateTime SaleDate { get; set; }
 public decimal TotalAmount { get; set; }
 public decimal Discount { get; set; }
 public decimal NetAmount { get; set; }
 public decimal PaidAmount { get; set; }
 public decimal ChangeAmount { get; set; }
 public string PaymentMethod { get; set; }
 public string InvoiceNo { get; set; }
 public string Notes { get; set; }
 public int UserID { get; set; }
 public bool IsDeleted { get; set; }
 public DateTime CreatedDate { get; set; }
 public DateTime? ModifiedDate { get; set; }
 public List<SaleDetail> Details { get; set; } = new List<SaleDetail>();
 }

 public class SaleDetail
 {
 public int SaleDetailID { get; set; }
 public int SaleID { get; set; }
 public int MedicineID { get; set; }
 public int QuantityPacks { get; set; }
 public int QuantityStrips { get; set; }
 public int QuantityTablets { get; set; }
 public decimal SalePrice { get; set; }
 public decimal Total { get; set; }
 public DateTime CreatedDate { get; set; }
 }
}
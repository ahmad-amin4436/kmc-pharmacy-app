using System;

namespace Ph_App.Models
{
 public class Customer
 {
 public int CustomerID { get; set; }
 public string CustomerName { get; set; }
 public string Contact { get; set; }
 public string Address { get; set; }
 public string Phone { get; set; }
 public string Email { get; set; }
 public decimal Balance { get; set; }
 public bool IsDeleted { get; set; }
 public DateTime CreatedDate { get; set; }
 public DateTime? ModifiedDate { get; set; }
 }
}
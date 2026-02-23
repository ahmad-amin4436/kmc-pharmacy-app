using System;

namespace Ph_App.Models
{
 public class User
 {
 public int UserID { get; set; }
 public string Username { get; set; }
 public string PasswordHash { get; set; }
 public string Role { get; set; } // Admin / Cashier
 public bool IsDeleted { get; set; }
 public DateTime CreatedDate { get; set; }
 public DateTime? ModifiedDate { get; set; }
 }
}
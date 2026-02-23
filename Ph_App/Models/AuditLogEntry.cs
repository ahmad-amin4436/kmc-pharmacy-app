using System;

namespace Ph_App.Models
{
 public class AuditLogEntry
 {
 public int LogID { get; set; }
 public int? UserID { get; set; }
 public string Username { get; set; }
 public string ActionType { get; set; }
 public string TableAffected { get; set; }
 public string RecordID { get; set; }
 public string OldValue { get; set; }
 public string NewValue { get; set; }
 public DateTime ActionDateTime { get; set; }
 public string IPAddress { get; set; }
 public string UserAgent { get; set; }
 }
}
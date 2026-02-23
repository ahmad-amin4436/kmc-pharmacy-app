using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class AuditLogsViewerForm : ResponsiveForm
    {
        public AuditLogsViewerForm()
        {
            InitializeComponent();
            // Set default date range to current month
            dtDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtDateTo.Value = DateTime.Now;
            LoadAuditLogs();
        }

        private void LoadAuditLogs()
        {
            try
            {
                var dateFrom = dtDateFrom?.Value.Date ?? DateTime.Now.Date;
                var dateTo = dtDateTo?.Value.Date ?? DateTime.Now.Date;
                var selectedActionType = cmbActionType?.SelectedItem?.ToString() ?? string.Empty;
                var selectedUser = cmbUser?.SelectedItem?.ToString() ?? string.Empty;

                // Get all audit logs from database and filter in-memory to avoid repository date-range issues
                var logs = PharmacyDBContext.AuditLogs.GetAll().ToList();

                // Filter by date range (inclusive)
                logs = logs.Where(l => l.ActionDateTime.Date >= dateFrom && l.ActionDateTime.Date <= dateTo).ToList();

                // Filter by action type if selected
                if (!string.IsNullOrEmpty(selectedActionType) && selectedActionType != "All")
                {
                    logs = logs.Where(log => string.Equals(log.ActionType, selectedActionType, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Filter by user if selected
                if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All")
                {
                    if (selectedUser == "System")
                    {
                        // system logs have no UserID
                        logs = logs.Where(log => !log.UserID.HasValue).ToList();
                    }
                    else
                    {
                        var user = PharmacyDBContext.Users.GetByUsername(selectedUser);
                        if (user != null)
                        {
                            logs = logs.Where(log => log.UserID == user.UserID).ToList();
                        }
                    }
                }

                // Order by the actual DateTime then project to display fields
                var data = logs.OrderByDescending(l => l.ActionDateTime).Select(log => new
                {
                    log.LogID,
                    Username = string.IsNullOrEmpty(log.Username) ? GetUsername(log.UserID) : log.Username,
                    log.ActionType,
                    log.TableAffected,
                    log.RecordID,
                    log.OldValue,
                    log.NewValue,
                    ActionDateTime = log.ActionDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ActionDate = log.ActionDateTime.ToString("yyyy-MM-dd"),
                    ActionTime = log.ActionDateTime.ToString("HH:mm:ss"),
                    Severity = GetSeverity(log.ActionType)
                }).ToList();

                // Update form title to show counts
                this.Text = $"Audit Logs - {data.Count()} entries from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}";

                if (dgv != null)
                {
                    dgv.DataSource = null;
                    dgv.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audit logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUsername(int? userId)
        {
            if (!userId.HasValue) return "System";
            var user = PharmacyDBContext.Users.GetById(userId.Value);
            return user?.Username ?? "Unknown";
        }

        private string GetSeverity(string actionType)
        {
            switch (actionType?.ToLower())
            {
                case "delete":
                    return "High";
                case "update":
                    return "Medium";
                case "create":
                case "insert":
                    return "Low";
                case "login":
                case "logout":
                    return "Info";
                default:
                    return "Normal";
            }
        }

        private void PopulateFilters()
        {
            try
            {
                // Populate action types
                if (cmbActionType != null)
                {
                    cmbActionType.Items.Clear();
                    cmbActionType.Items.Add("All");
                    var actionTypes = PharmacyDBContext.AuditLogs.GetAll().Select(log => log.ActionType).Distinct().OrderBy(action => action).ToList();
                    foreach (var action in actionTypes)
                    {
                        cmbActionType.Items.Add(action);
                    }
                    cmbActionType.SelectedIndex = 0;
                }

                // Populate users
                if (cmbUser != null)
                {
                    cmbUser.Items.Clear();
                    cmbUser.Items.Add("All");
                    cmbUser.Items.Add("System");
                    var users = PharmacyDBContext.Users.GetAll().Where(u => !u.IsDeleted).OrderBy(u => u.Username).ToList();
                    foreach (var user in users)
                    {
                        cmbUser.Items.Add(user.Username);
                    }
                    cmbUser.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadAuditLogs();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset date range to current month
                if (dtDateFrom != null)
                    dtDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (dtDateTo != null)
                    dtDateTo.Value = DateTime.Now;
                
                // Reset dropdowns
                if (cmbActionType != null)
                    cmbActionType.SelectedIndex = 0;
                if (cmbUser != null)
                    cmbUser.SelectedIndex = 0;
                
                LoadAuditLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv == null)
                {
                    MessageBox.Show("No data to export", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.csv)|*.csv",
                    Title = "Export Audit Logs",
                    FileName = $"AuditLogs_{DateTime.Now:yyyy-MM-dd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var csv = new System.Text.StringBuilder();
                    
                    // Add headers
                    csv.AppendLine("Log ID,Username,Action Type,Table Affected,Record ID,Old Value,New Value,Action Date,Action Time,Severity");
                    
                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        
                        var values = new string[]
                        {
                            row.Cells["LogID"]?.Value?.ToString() ?? "0",
                            $"\"{row.Cells["Username"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["ActionType"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["TableAffected"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["RecordID"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["OldValue"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["NewValue"]?.Value?.ToString() ?? string.Empty}\"",
                            row.Cells["ActionDate"]?.Value?.ToString() ?? "",
                            row.Cells["ActionTime"]?.Value?.ToString() ?? "",
                            $"\"{row.Cells["Severity"]?.Value?.ToString() ?? string.Empty}\""
                        };
                        
                        csv.AppendLine(string.Join(",", values));
                    }
                    
                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                    MessageBox.Show("Audit logs exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting audit logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AuditLogsViewerForm_Load(object sender, EventArgs e)
        {
            PopulateFilters();
        }
    }
}

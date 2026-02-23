using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class ExpiryReportForm : Form
    {
        public ExpiryReportForm()
        {
            InitializeComponent();
            dtExpiryFrom.Value = DateTime.Now.AddYears(-2).Date;
            dtExpiryTo.Value = DateTime.Now.Date;
            ApplyExpiredMedicinesFilter();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyExpiredMedicinesFilter()
        {
            var name = (txtFilterName?.Text ?? string.Empty).Trim().ToLowerInvariant();
            var expiryFrom = dtExpiryFrom.Value.Date;
            var expiryTo = dtExpiryTo.Value.Date;

            // Get all medicines from database
            var allMedicines = PharmacyDBContext.Medicines.GetAll().ToList();
            
            // Debug: Show total medicines count
            var totalMedicines = allMedicines.Count(m => !m.IsDeleted);
            
            // First apply date range filter to get medicines in selected range
            var list = allMedicines.Where(m => !m.IsDeleted && m.ExpiryDate.HasValue && 
                                                               m.ExpiryDate.Value.Date >= expiryFrom && 
                                                               m.ExpiryDate.Value.Date <= expiryTo).ToList();
            
            // Debug: Show medicines in date range count
            var dateRangeCount = list.Count();
            
            // Apply name filter if provided
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(m => ((m.MedicineName ?? "").ToLowerInvariant().Contains(name) || 
                                       (m.GenericName ?? "").ToLowerInvariant().Contains(name) || 
                                       (m.Company ?? "").ToLowerInvariant().Contains(name) || 
                                       (m.BatchNo ?? "").ToLowerInvariant().Contains(name))).ToList();
            }
            
            // Note: We're showing all medicines in the date range, not just expired ones
            // This allows users to see medicines that will expire soon too

            var data = list.Select(m => new
            {
                m.MedicineID,
                m.MedicineName,
                m.GenericName,
                m.Company,
                m.BatchNo,
                CurrentStock = m.CurrentStockPacks,
                CostPrice = m.PurchasePricePerPack,
                SalePrice = m.SalePricePerPack,
                TotalValue = m.CurrentStockPacks * m.SalePricePerPack,
                ExpiryDate = m.ExpiryDate?.ToString("yyyy-MM-dd"),
                DaysExpired = m.ExpiryDate.HasValue ? (DateTime.Now.Date - m.ExpiryDate.Value.Date).Days : 0,
                Status = GetExpiryStatus(m.ExpiryDate)
            }).OrderBy(m => m.ExpiryDate).ToList();

            // Debug: Update form title to show counts
            this.Text = $"Expiry Report - {data.Count()} medicines in range ({expiryFrom:yyyy-MM-dd} to {expiryTo:yyyy-MM-dd}) of {totalMedicines} total";

            dgv.DataSource = null;
            dgv.DataSource = data;
        }

        private string GetExpiryStatus(DateTime? expiryDate)
        {
            if (!expiryDate.HasValue) return "Unknown";
            
            var daysExpired = (DateTime.Now.Date - expiryDate.Value.Date).Days;
            
            if (daysExpired > 365) return $"Expired ({daysExpired / 365} year{(daysExpired / 365 > 1 ? "s" : "")} ago)";
            if (daysExpired > 30) return $"Expired ({daysExpired} days ago)";
            if (daysExpired > 0) return $"Expired ({daysExpired} days ago)";
            if (daysExpired == 0) return "Expires Today";
            
            return "Valid";
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyExpiredMedicinesFilter();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtFilterName.Text = string.Empty;
            dtExpiryFrom.Value = DateTime.Now.AddYears(-2).Date;
            dtExpiryTo.Value = DateTime.Now.Date;
            ApplyExpiredMedicinesFilter();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.csv)|*.csv",
                    Title = "Export Expiry Report",
                    FileName = $"ExpiryReport_{DateTime.Now:yyyy-MM-dd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var csv = new StringBuilder();
                    
                    // Add headers
                    csv.AppendLine("Medicine ID,Medicine Name,Generic Name,Company,Batch No,Current Stock,Cost Price,Sale Price,Total Value,Expiry Date,Days Expired,Status");
                    
                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        
                        var values = new string[]
                        {
                            row.Cells["MedicineID"].Value?.ToString() ?? "0",
                            $"\"{row.Cells["MedicineName"].Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["GenericName"].Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["Company"].Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["BatchNo"].Value?.ToString() ?? string.Empty}\"",
                            row.Cells["CurrentStock"].Value?.ToString() ?? "0",
                            row.Cells["CostPrice"].Value?.ToString() ?? "0",
                            row.Cells["SalePrice"].Value?.ToString() ?? "0",
                            row.Cells["TotalValue"].Value?.ToString() ?? "0",
                            row.Cells["ExpiryDate"].Value?.ToString() ?? "",
                            row.Cells["DaysExpired"].Value?.ToString() ?? "0",
                            $"\"{row.Cells["Status"].Value?.ToString() ?? string.Empty}\""
                        };
                        
                        csv.AppendLine(string.Join(",", values));
                    }
                    
                    File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                    MessageBox.Show("Expiry report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class LowStockReportForm : ResponsiveForm
    {
        public LowStockReportForm()
        {
            InitializeComponent();
            ApplyLowStockFilter();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyLowStockFilter()
        {
            try
            {
                var name = (txtFilterName?.Text ?? string.Empty).Trim().ToLowerInvariant();
                var minStockFrom = numMinStockFrom != null ? Convert.ToInt32(numMinStockFrom.Value) : 0;
                var minStockTo = numMinStockTo != null ? Convert.ToInt32(numMinStockTo.Value) : 100;

                // Get all medicines from database
                var allMedicines = PharmacyDBContext.Medicines.GetAll().ToList();

                // Filter for low stock medicines only (current stock <= minimum stock level)
                var list = allMedicines.Where(m => !m.IsDeleted && 
                                                                   m.CurrentStockPacks <= m.MinimumStockLevel &&
                                                                   m.MinimumStockLevel >= minStockFrom &&
                                                                   m.MinimumStockLevel <= minStockTo).ToList();

                // Apply name filter if provided
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(m => ((m.MedicineName ?? "").ToLowerInvariant().Contains(name) || 
                                           (m.GenericName ?? "").ToLowerInvariant().Contains(name) || 
                                           (m.Company ?? "").ToLowerInvariant().Contains(name) || 
                                           (m.BatchNo ?? "").ToLowerInvariant().Contains(name))).ToList();
                }

                var data = list.Select(m => new
                {
                    m.MedicineID,
                    m.MedicineName,
                    m.GenericName,
                    m.Company,
                    m.BatchNo,
                    CurrentStock = m.CurrentStockPacks,
                    MinimumStock = m.MinimumStockLevel,
                    StockShortage = m.MinimumStockLevel - m.CurrentStockPacks,
                    CostPrice = m.PurchasePricePerPack,
                    SalePrice = m.SalePricePerPack,
                    TotalValue = m.CurrentStockPacks * m.SalePricePerPack,
                    ExpiryDate = m.ExpiryDate?.ToString("yyyy-MM-dd"),
                    Status = GetStockStatus(m.CurrentStockPacks, m.MinimumStockLevel)
                }).OrderBy(m => m.StockShortage).ToList();

                // Update form title to show counts
                var totalMedicines = allMedicines.Count();
                this.Text = $"Low Stock Report - {data.Count()} low stock of {totalMedicines} total medicines";

                if (dgv != null)
                {
                    dgv.DataSource = null;
                    dgv.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying low stock filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetStockStatus(int currentStock, int minStock)
        {
            var shortage = minStock - currentStock;
            
            if (shortage <= 0) return "Adequate";
            if (shortage == 1) return "Low (1 pack short)";
            if (shortage <= 5) return $"Low ({shortage} packs short)";
            if (shortage <= 10) return $"Critical ({shortage} packs short)";
            
            return $"Urgent ({shortage} packs short)";
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyLowStockFilter();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFilterName != null)
                    txtFilterName.Text = string.Empty;
                if (numMinStockFrom != null)
                    numMinStockFrom.Value = 0;
                if (numMinStockTo != null)
                    numMinStockTo.Value = 100;
                ApplyLowStockFilter();
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
                    Title = "Export Low Stock Report",
                    FileName = $"LowStockReport_{DateTime.Now:yyyy-MM-dd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var csv = new System.Text.StringBuilder();
                    
                    // Add headers
                    csv.AppendLine("Medicine ID,Medicine Name,Generic Name,Company,Batch No,Current Stock,Minimum Stock,Stock Shortage,Cost Price,Sale Price,Total Value,Expiry Date,Status");
                    
                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        
                        var values = new string[]
                        {
                            row.Cells["MedicineID"]?.Value?.ToString() ?? "0",
                            $"\"{row.Cells["MedicineName"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["GenericName"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["Company"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["BatchNo"]?.Value?.ToString() ?? string.Empty}\"",
                            row.Cells["CurrentStock"]?.Value?.ToString() ?? "0",
                            row.Cells["MinimumStock"]?.Value?.ToString() ?? "0",
                            row.Cells["StockShortage"]?.Value?.ToString() ?? "0",
                            row.Cells["CostPrice"]?.Value?.ToString() ?? "0",
                            row.Cells["SalePrice"]?.Value?.ToString() ?? "0",
                            row.Cells["TotalValue"]?.Value?.ToString() ?? "0",
                            row.Cells["ExpiryDate"]?.Value?.ToString() ?? "",
                            $"\"{row.Cells["Status"]?.Value?.ToString() ?? string.Empty}\""
                        };
                        
                        csv.AppendLine(string.Join(",", values));
                    }
                    
                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                    MessageBox.Show("Low stock report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

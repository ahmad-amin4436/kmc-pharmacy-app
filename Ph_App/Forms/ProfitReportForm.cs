using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class ProfitReportForm : ResponsiveForm
    {
        public ProfitReportForm()
        {
            try
            {
                InitializeComponent();
                // Set default date range to current month
                dtDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtDateTo.Value = DateTime.Now;
                ApplyProfitFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Profit Report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyProfitFilter()
        {
            try
            {
                var dateFrom = dtDateFrom?.Value.Date ?? DateTime.Now.Date;
                var dateTo = dtDateTo?.Value.Date ?? DateTime.Now.Date;
                var name = (txtFilterName?.Text ?? string.Empty).Trim().ToLowerInvariant();
                var profitFrom = numMinProfitFrom != null ? Convert.ToDecimal(numMinProfitFrom.Value) : 0;
                var profitTo = numMinProfitTo != null ? Convert.ToDecimal(numMinProfitTo.Value) : 10000;

                // Get sales within date range from database
                var salesInRange = PharmacyDBContext.Sales.GetByDateRange(dateFrom, dateTo).ToList();

                // Get all medicines from database for reference
                var allMedicines = PharmacyDBContext.Medicines.GetAll().ToList();

                // Create profit data from sales
                var profitData = new List<object>();

                foreach (var sale in salesInRange)
                {
                    foreach (var detail in sale.Details)
                    {
                        var medicine = allMedicines.FirstOrDefault(m => m.MedicineID == detail.MedicineID);
                        if (medicine == null) continue;

                        var totalCost = detail.QuantityPacks * medicine.PurchasePricePerPack;
                        var totalRevenue = detail.QuantityPacks * detail.SalePrice;
                        var profit = totalRevenue - totalCost;
                        var profitPercentage = totalCost > 0 ? (profit / totalCost) * 100 : 0;

                        // Apply profit range filter
                        if (profit < profitFrom || profit > profitTo) continue;

                        // Apply name filter if provided
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!((medicine.MedicineName ?? "").ToLowerInvariant().Contains(name) ||
                                  (medicine.GenericName ?? "").ToLowerInvariant().Contains(name) ||
                                  (medicine.Company ?? "").ToLowerInvariant().Contains(name) ||
                                  (medicine.BatchNo ?? "").ToLowerInvariant().Contains(name)))
                                continue;
                        }

                        profitData.Add(new
                        {
                            InvoiceNo = sale.InvoiceNo,
                            SaleDate = sale.SaleDate.ToString("yyyy-MM-dd HH:mm"),
                            MedicineID = medicine.MedicineID,
                            MedicineName = medicine.MedicineName,
                            GenericName = medicine.GenericName,
                            Company = medicine.Company,
                            BatchNo = medicine.BatchNo,
                            QuantityPacks = detail.QuantityPacks,
                            CostPricePerPack = medicine.PurchasePricePerPack,
                            SalePricePerPack = detail.SalePrice,
                            TotalCost = totalCost,
                            TotalRevenue = totalRevenue,
                            Profit = profit,
                            ProfitPercentage = Math.Round(profitPercentage, 2),
                            Status = GetProfitStatus(profit, (double)profitPercentage)
                        });
                    }
                }

                // Sort by profit (highest first)
                var data = profitData.OrderByDescending(item => ((dynamic)item).Profit).ToList();

                // Update form title to show counts
                this.Text = $"Profit Report - {data.Count()} transactions from {dateFrom:yyyy-MM-dd} to {dateTo:yyyy-MM-dd}";

                if (dgv != null)
                {
                    dgv.DataSource = null;
                    dgv.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying profit filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetProfitStatus(decimal profit, double profitPercentage)
        {
            if (profit < 0) return "Loss";
            if (profit == 0) return "Break Even";
            if (profitPercentage < 5) return "Low Margin";
            if (profitPercentage < 15) return "Good Margin";
            if (profitPercentage < 30) return "High Margin";
            
            return "Excellent Margin";
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyProfitFilter();
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
                
                // Clear other filters
                if (txtFilterName != null)
                    txtFilterName.Text = string.Empty;
                if (numMinProfitFrom != null)
                    numMinProfitFrom.Value = 0;
                if (numMinProfitTo != null)
                    numMinProfitTo.Value = 10000;
                
                ApplyProfitFilter();
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
                    Title = "Export Profit Report",
                    FileName = $"ProfitReport_{DateTime.Now:yyyy-MM-dd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var csv = new System.Text.StringBuilder();
                    
                    // Add headers
                    csv.AppendLine("Invoice No,Sale Date,Medicine ID,Medicine Name,Generic Name,Company,Batch No,Quantity Packs,Cost Price Per Pack,Sale Price Per Pack,Total Cost,Total Revenue,Profit,Profit %,Status");
                    
                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        
                        var values = new string[]
                        {
                            row.Cells["InvoiceNo"]?.Value?.ToString() ?? "",
                            row.Cells["SaleDate"]?.Value?.ToString() ?? "",
                            row.Cells["MedicineID"]?.Value?.ToString() ?? "0",
                            $"\"{row.Cells["MedicineName"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["GenericName"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["Company"]?.Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["BatchNo"]?.Value?.ToString() ?? string.Empty}\"",
                            row.Cells["QuantityPacks"]?.Value?.ToString() ?? "0",
                            row.Cells["CostPricePerPack"]?.Value?.ToString() ?? "0",
                            row.Cells["SalePricePerPack"]?.Value?.ToString() ?? "0",
                            row.Cells["TotalCost"]?.Value?.ToString() ?? "0",
                            row.Cells["TotalRevenue"]?.Value?.ToString() ?? "0",
                            row.Cells["Profit"]?.Value?.ToString() ?? "0",
                            row.Cells["ProfitPercentage"]?.Value?.ToString() ?? "0",
                            $"\"{row.Cells["Status"]?.Value?.ToString() ?? string.Empty}\""
                        };
                        
                        csv.AppendLine(string.Join(",", values));
                    }
                    
                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                    MessageBox.Show("Profit report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

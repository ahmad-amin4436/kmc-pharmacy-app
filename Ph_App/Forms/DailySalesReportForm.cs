using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Text;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class DailySalesReportForm : ResponsiveForm
    {
        public DailySalesReportForm()
        {
            InitializeComponent();
            dtReportDate.Value = DateTime.Today;
            // load logo if present
            try
            {
                var logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", "black logo correct address.jpeg");
                var img = Ph_App.Utils.LogoHelper.LoadLogoWithTransparentBackground(logoPath);
                if (img != null && this.Controls.Contains(pbLogo))
                {
                    // Use high-quality scaled image for PictureBox
                    var scaledImg = Ph_App.Utils.LogoHelper.LoadLogoForPictureBox(logoPath, pbLogo.Size);
                    pbLogo.Image = scaledImg ?? img;
                    pbLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    pbLogo.BackColor = System.Drawing.Color.Transparent;
                    pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch { }

            LoadSalesReport();
        }

        private void LoadSalesReport()
        {
            var date = dtReportDate.Value.Date;
            var salesForDay = PharmacyDBContext.Sales.GetByDateRange(date, date).ToList();

            var rows = salesForDay.SelectMany(sale =>
                sale.Details.Select(detail =>
                {
                    var medicine = PharmacyDBContext.Medicines.GetById(detail.MedicineID);
                    return new
                    {
                        sale.InvoiceNo,
                        SaleDate = sale.SaleDate.ToString("yyyy-MM-dd HH:mm"),
                        Medicine = medicine?.MedicineName ?? "?",
                        detail.QuantityPacks,
                        detail.QuantityStrips,
                        detail.QuantityTablets,
                        detail.SalePrice,
                        detail.Total,
                        SaleTotal = sale.TotalAmount
                    };
                })
            ).ToList(); 

            dgv.DataSource = null;
            dgv.DataSource = rows;
            
            // Add Print button column if not already present
            if (!dgv.Columns.Contains("Print"))
            {
                dgv.Columns.Add(btnPrintColumn);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgv.Columns["Print"].Index && e.RowIndex >= 0)
            {
                var invoiceNo = dgv.Rows[e.RowIndex].Cells["InvoiceNo"].Value?.ToString();
                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    GenerateInvoicePDF(invoiceNo);
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void GenerateInvoicePDF(string invoiceNo)
        {
            try
            {
                var sale = PharmacyDBContext.Sales.GetByInvoiceNo(invoiceNo);
                if (sale == null)
                {
                    MessageBox.Show($"Invoice {invoiceNo} not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    Title = $"Save Invoice {invoiceNo}",
                    FileName = $"Invoice_{invoiceNo}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Generate simple HTML invoice
                    var htmlContent = GenerateInvoiceHTML(sale);
                    
                    // For now, save as HTML file (can be converted to PDF later)
                    File.WriteAllText(saveFileDialog.FileName.Replace(".pdf", ".html"), htmlContent);
                    
                    MessageBox.Show($"Invoice {invoiceNo} saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateInvoiceHTML(Sale sale)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<title>Invoice " + sale.InvoiceNo + "</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine(".invoice-info { margin-bottom: 20px; }");
            html.AppendLine("table { border-collapse: collapse; width: 100%; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine(".total { text-align: right; font-weight: bold; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            html.AppendLine("<div class='header'>");
            // include embedded logo if present
            try
            {
                var logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", "black logo correct address.jpeg");
                if (System.IO.File.Exists(logoPath))
                {
                    var bytes = System.IO.File.ReadAllBytes(logoPath);
                    var base64 = Convert.ToBase64String(bytes);
                    var ext = System.IO.Path.GetExtension(logoPath).TrimStart('.').ToLower();
                    var mime = ext == "jpeg" ? "image/jpeg" : "image/x-icon";
                    html.AppendLine($"<img src=\"data:{mime};base64,{base64}\" style=\"max-height:80px;\" />");
                }
            }
            catch { }
            html.AppendLine("<h1>PHARMACY INVOICE</h1>");
            html.AppendLine("</div>");
            
            html.AppendLine("<div class='invoice-info'>");
            html.AppendLine($"<p><strong>Invoice No:</strong> {sale.InvoiceNo}</p>");
            html.AppendLine($"<p><strong>Date:</strong> {sale.SaleDate:yyyy-MM-dd HH:mm}</p>");
            html.AppendLine($"<p><strong>Payment Method:</strong> {sale.PaymentMethod}</p>");
            html.AppendLine("</div>");
            
            html.AppendLine("<table>");
            html.AppendLine("<tr><th>Medicine</th><th>Packs</th><th>Strips</th><th>Tablets</th><th>Price</th><th>Total</th></tr>");
            
            foreach (var detail in sale.Details)
            {
                var medicine = PharmacyDBContext.Medicines.GetById(detail.MedicineID);
                html.AppendLine("<tr>");
                html.AppendLine($"<td>{medicine?.MedicineName ?? "Unknown"}</td>");
                html.AppendLine($"<td>{detail.QuantityPacks}</td>");
                html.AppendLine($"<td>{detail.QuantityStrips}</td>");
                html.AppendLine($"<td>{detail.QuantityTablets}</td>");
                // html.AppendLine($"<td>{detail.SalePrice:C}</td>"); // System currency format commented
                html.AppendLine($"<td>Rs {detail.SalePrice:F2}</td>"); // PKR format
                // html.AppendLine($"<td>{detail.Total:C}</td>"); // System currency format commented
                html.AppendLine($"<td>Rs {detail.Total:F2}</td>"); // PKR format
                html.AppendLine("</tr>");
            }
            
            html.AppendLine("</table>");
            
            html.AppendLine("<div class='total'>");
            // html.AppendLine($"<p><strong>Subtotal:</strong> {sale.TotalAmount:C}</p>"); // System currency format commented
            html.AppendLine($"<p><strong>Subtotal:</strong> Rs {sale.TotalAmount:F2}</p>"); // PKR format
            // html.AppendLine($"<p><strong>Discount:</strong> {sale.Discount:C}</p>"); // System currency format commented
            html.AppendLine($"<p><strong>Discount:</strong> Rs {sale.Discount:F2}</p>"); // PKR format
            // html.AppendLine($"<p><strong>Net Amount:</strong> {sale.NetAmount:C}</p>"); // System currency format commented
            html.AppendLine($"<p><strong>Net Amount:</strong> Rs {sale.NetAmount:F2}</p>"); // PKR format
            // html.AppendLine($"<p><strong>Paid Amount:</strong> {sale.PaidAmount:C}</p>"); // System currency format commented
            html.AppendLine($"<p><strong>Paid Amount:</strong> Rs {sale.PaidAmount:F2}</p>"); // PKR format
            // html.AppendLine($"<p><strong>Change:</strong> {sale.ChangeAmount:C}</p>"); // System currency format commented
            html.AppendLine($"<p><strong>Change:</strong> Rs {sale.ChangeAmount:F2}</p>"); // PKR format
            html.AppendLine("</div>");
            
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            return html.ToString();
        }

        private void ExportToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.csv)|*.csv",
                    Title = "Export Daily Sales Report",
                    FileName = $"DailySales_{dtReportDate.Value:yyyy-MM-dd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var csv = new StringBuilder();
                    
                    // Add headers
                    csv.AppendLine("Invoice No,Sale Date,Medicine,Packs,Strips,Tablets,Sale Price,Total,Sale Total");
                    
                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        
                        var values = new string[]
                        {
                            $"\"{row.Cells["InvoiceNo"].Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["SaleDate"].Value?.ToString() ?? string.Empty}\"",
                            $"\"{row.Cells["Medicine"].Value?.ToString() ?? string.Empty}\"",
                            row.Cells["QuantityPacks"].Value?.ToString() ?? "0",
                            row.Cells["QuantityStrips"].Value?.ToString() ?? "0",
                            row.Cells["QuantityTablets"].Value?.ToString() ?? "0",
                            row.Cells["SalePrice"].Value?.ToString() ?? "0",
                            row.Cells["Total"].Value?.ToString() ?? "0",
                            row.Cells["SaleTotal"].Value?.ToString() ?? "0"
                        };
                        
                        csv.AppendLine(string.Join(",", values));
                    }
                    
                    File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                    MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

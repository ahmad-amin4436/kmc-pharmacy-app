using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ph_App.Forms
{
    public partial class DashboardForm : ResponsiveForm
    {
        public DashboardForm()
        {
            InitializeComponent();
            // Ensure roles/rights service is loaded and defaulted
            Ph_App.BLL.RolesRightsService.InitializeIfNeeded();
            // Load logo into picture box if present
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
            PopulateModules();
        }

        private void PopulateModules()
        {
            void AddModule(string key, string text, EventHandler onClick)
            {
                var btn = new Button
                {
                    Text = text,
                    Width = 220,
                    Height = 80,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Margin = new Padding(15)
                };
                btn.Click += onClick;
                // Set visibility based on current user role and configured rights
                try
                {
                    var role = Ph_App.Database.PharmacyDBContext.CurrentUser?.Role ?? "";
                    btn.Visible = Ph_App.BLL.RolesRightsService.IsAllowed(role, key);
                }
                catch { /* ignore and show by default */ }

                flowPanel.Controls.Add(btn);
            }
            AddModule("medicine_management", "Medicine Management", (s, e) => OpenForm(new MedicineForm()));
            AddModule("stock_management", "Stock Management", (s, e) => OpenForm(new StockManagementForm()));
            AddModule("purchase_entry", "Purchase Entry", (s, e) => OpenForm(new PurchaseEntryForm()));
            AddModule("sales_pos", "Sales / POS", (s, e) => OpenForm(new SalesForm()));
            AddModule("daily_sales_report", "Daily Sales Report", (s, e) => OpenForm(new DailySalesReportForm()));
            AddModule("expiry_report", "Expiry Report", (s, e) => OpenForm(new ExpiryReportForm()));
            AddModule("low_stock_report", "Low Stock Report", (s, e) => OpenForm(new LowStockReportForm()));
            AddModule("profit_report", "Profit Report", (s, e) => OpenForm(new ProfitReportForm()));
            AddModule("user_management", "User Management", (s, e) => OpenForm(new UserManagementForm()));
            AddModule("audit_logs_viewer", "Audit Logs Viewer", (s, e) => OpenForm(new AuditLogsViewerForm()));
            // Roles & Rights management - only visible if allowed (typically Admin)
            AddModule("roles_rights", "Roles & Rights", (s, e) => OpenForm(new RolesRightsForm()));
        }

        private void OpenForm(Form f)
        {
            // Log module open
            try
            {
                Ph_App.Database.PharmacyDBContext.AuditLogs.LogUserAction(Ph_App.Database.PharmacyDBContext.CurrentUser?.UserID, "OPEN", "Forms", f.GetType().Name, "", $"User opened {f.GetType().Name}");
            }
            catch { }

            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            f.Dispose();
        }
    }
}

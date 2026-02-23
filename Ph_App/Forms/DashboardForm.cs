using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ph_App.Forms
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            PopulateModules();
        }

        private void PopulateModules()
        {
            void AddModule(string text, EventHandler onClick)
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
                flowPanel.Controls.Add(btn);
            }

            AddModule("Medicine Management", (s, e) => OpenForm(new MedicineForm()));
            AddModule("Stock Management", (s, e) => OpenForm(new StockManagementForm()));
            AddModule("Purchase Report", (s, e) => OpenForm(new PurchaseEntryForm()));
            AddModule("Sales / POS", (s, e) => OpenForm(new SalesForm()));
            AddModule("Daily Sales Report", (s, e) => OpenForm(new DailySalesReportForm()));
            AddModule("Expiry Report", (s, e) => OpenForm(new ExpiryReportForm()));
            AddModule("Low Stock Report", (s, e) => OpenForm(new LowStockReportForm()));
            AddModule("Profit Report", (s, e) => OpenForm(new ProfitReportForm()));
            AddModule("User Management", (s, e) => OpenForm(new UserManagementForm()));
            AddModule("Audit Logs Viewer", (s, e) => OpenForm(new AuditLogsViewerForm()));
        }

        private void OpenForm(Form f)
        {
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
            f.Dispose();
        }
    }
}

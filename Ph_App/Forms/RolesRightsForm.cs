using System;
using System.Linq;
using System.Windows.Forms;

namespace Ph_App.Forms
{
    public partial class RolesRightsForm : Form
    {
        private readonly string[] _allModules = new[]
        {
            "medicine_management",
            "stock_management",
            "purchase_entry",
            "sales_pos",
            "daily_sales_report",
            "expiry_report",
            "low_stock_report",
            "profit_report",
            "user_management",
            "audit_logs_viewer",
            "roles_rights"
        };

        public RolesRightsForm()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            comboRoles.Items.Clear();
            var all = Ph_App.BLL.RolesRightsService.GetAll();
            foreach (var r in all.Keys) comboRoles.Items.Add(r);
            // If no roles present, seed defaults
            if (comboRoles.Items.Count == 0)
            {
                comboRoles.Items.Add("Admin");
                comboRoles.Items.Add("Cashier");
            }
            if (comboRoles.Items.Count > 0) comboRoles.SelectedIndex = 0;
        }

        private void comboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var role = comboRoles.SelectedItem?.ToString();
            checkedListModules.Items.Clear();
            foreach (var m in _allModules)
            {
                var label = m; // show key; could be mapped to friendly text
                var idx = checkedListModules.Items.Add(label);
                var allowed = Ph_App.BLL.RolesRightsService.IsAllowed(role, m);
                checkedListModules.SetItemChecked(idx, allowed);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var role = comboRoles.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(role)) return;
            var allowed = checkedListModules.CheckedItems.Cast<string>().ToList();
            Ph_App.BLL.RolesRightsService.SetRights(role, allowed);
            MessageBox.Show("Rights saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // If current user updated their own role, refresh UI
            this.Close();
        }
    }
}

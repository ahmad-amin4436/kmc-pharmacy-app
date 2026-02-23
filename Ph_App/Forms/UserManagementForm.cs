using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Models;
using Ph_App.Database;
using Ph_App.DAL;

namespace Ph_App.Forms
{
    public partial class UserManagementForm : ResponsiveForm
    {
        public UserManagementForm()
        {
            InitializeComponent();
            // Set default date range to current month
            dtDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtDateTo.Value = DateTime.Now;
            LoadUsers();
            
            // Log form access
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "VIEW", "Forms", "UserManagementForm", "", "User accessed User Management");
        }

        private void LoadUsers()
        {
            try
            {
                var username = (txtFilterName?.Text ?? string.Empty).Trim().ToLowerInvariant();
                var dateFrom = dtDateFrom?.Value.Date ?? DateTime.Now.Date;
                var dateTo = dtDateTo?.Value.Date ?? DateTime.Now.Date;

                // Get users from database
                var users = PharmacyDBContext.Users.GetAll().ToList();

                // Filter by username if provided
                if (!string.IsNullOrEmpty(username))
                {
                    users = users.Where(u => (u.Username ?? "").ToLowerInvariant().Contains(username)).ToList();
                }

                // Filter by created date range
                users = users.Where(u => u.CreatedDate.Date >= dateFrom && u.CreatedDate.Date <= dateTo).ToList();

                var data = users.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.Role,
                    u.IsDeleted,
                    CreatedDate = u.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                    Status = GetUserStatus(u.IsDeleted)
                }).OrderBy(u => u.Username).ToList();

                // Update form title to show counts
                this.Text = $"User Management - {data.Count()} users found";

                if (dgv != null)
                {
                    dgv.DataSource = null;
                    dgv.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUserStatus(bool isDeleted)
        {
            return isDeleted ? "Inactive" : "Active";
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear username filter
                if (txtFilterName != null)
                    txtFilterName.Text = string.Empty;
                
                // Reset date range to current month
                if (dtDateFrom != null)
                    dtDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (dtDateTo != null)
                    dtDateTo.Value = DateTime.Now;
                
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "UserManagementForm.btnAdd", "", "User clicked Add User button");
            
            using (var f = new AddUserForm())
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    var u = f.Result;
                    u.CreatedDate = DateTime.Now;
                    var addedUser = PharmacyDBContext.Users.Add(u);
                    PharmacyDBContext.AuditLogs.LogUserAction(addedUser.UserID, "CREATE", "Users", addedUser.UserID.ToString(), "", $"Username: {addedUser.Username}, Role: {addedUser.Role}");
                    LoadUsers();
                    MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private User GetSelectedUser()
        {
            if (dgv.SelectedRows.Count == 0) return null;
            var id = Convert.ToInt32(dgv.SelectedRows[0].Cells["UserID"].Value);
            return PharmacyDBContext.Users.GetById(id);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "UserManagementForm.btnEdit", "", "User clicked Edit User button");
            
            var sel = GetSelectedUser();
            if (sel == null) 
            { 
                MessageBox.Show("Select a user first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                return; 
            }
            
            using (var f = new AddUserForm(sel))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    var updated = f.Result;
                    var oldValue = $"Username: {sel.Username}, Role: {sel.Role}";
                    var newValue = $"Username: {updated.Username}, Role: {updated.Role}";
                    sel.Username = updated.Username;
                    sel.Role = updated.Role;
                    sel.ModifiedDate = DateTime.Now;
                    PharmacyDBContext.Users.Update(sel);
                    PharmacyDBContext.AuditLogs.LogUserAction(sel.UserID, "UPDATE", "Users", sel.UserID.ToString(), oldValue, newValue);
                    LoadUsers();
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}

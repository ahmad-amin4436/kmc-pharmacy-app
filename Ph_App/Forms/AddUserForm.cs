using System;
using System.Windows.Forms;
using Ph_App.Models;
using Ph_App.BLL;

namespace Ph_App.Forms
{
    public partial class AddUserForm : Form
    {
        public User Result { get; private set; }
        private User editing;

        public AddUserForm() : this(null) { }

        public AddUserForm(User u)
        {
            editing = u;
            InitializeComponent();
            this.Text = editing == null ? "Add User" : "Edit User";
            if (u != null)
            {
                txtUsername.Text = u.Username;
                cbRole.SelectedItem = u.Role;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { MessageBox.Show("Username required"); return; }
            if (editing == null && string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("Password required"); return; }
            var user = new User { Username = txtUsername.Text.Trim(), Role = cbRole.SelectedItem.ToString(), CreatedDate = DateTime.Now };
            if (!string.IsNullOrWhiteSpace(txtPassword.Text)) user.PasswordHash = AuthService.HashPassword(txtPassword.Text);
            Result = user;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ph_App.BLL;
using Ph_App.Forms;

namespace Ph_App
{
    public partial class Form1 : Form
    {
        private readonly AuthService _auth = new AuthService();
        public Form1()
        {
            InitializeComponent();
            InitializeLoginControls();
        }
        
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;
        
        private void InitializeLoginControls()
        {
            this.Text = "Pharmacy Management - Login";
            this.Width = 400;
            this.Height = 250;
            
            txtUsername = new TextBox { Left = 120, Top = 40, Width = 180 };
            txtPassword = new TextBox { Left = 120, Top = 80, Width = 180, UseSystemPasswordChar = true };
            btnLogin = new Button { Left = 120, Top = 120, Width = 100, Text = "Login" };
            lblStatus = new Label { Left = 120, Top = 160, Width = 300 };
            
            var lblUser = new Label { Left = 40, Top = 40, Text = "Username:" };
            var lblPass = new Label { Left = 40, Top = 80, Text = "Password:" };
            
            btnLogin.Click += BtnLogin_Click;
            
            this.Controls.Add(lblUser);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblStatus);
        }
        
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            lblStatus.Text = "Authenticating...";
            try
            {
                var user = await _auth.AuthenticateAsync(txtUsername.Text.Trim(), txtPassword.Text);
                if (user != null)
                {
                    lblStatus.Text = $"Welcome {user.Username} ({user.Role})";
                    // Open Dashboard
                    this.Hide();
                    using (var dash = new DashboardForm())
                    {
                        dash.ShowDialog(this);
                    }
                    this.Show();
                }
                else
                {
                    lblStatus.Text = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}

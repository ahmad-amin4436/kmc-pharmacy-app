using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ph_App.BLL;
using Ph_App.Forms;
using System.IO;
using System.Drawing;
using System.Linq;

namespace Ph_App
{
    public partial class Form1 : Form
    {
        private readonly AuthService _auth = new AuthService();
        public Form1()
        {
            InitializeComponent();
            // load cleaned transparent logo into picture box (designer places pbLogo)
            try
            {
                var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", "black logo correct address.jpg.jpeg");
                var img = Ph_App.Utils.LogoHelper.LoadLogoWithTransparentBackground(logoPath);
                if (img != null)
                {
                    var objs = this.Controls.Find("pbLogo", true);
                    if (objs != null && objs.Length > 0 && objs[0] is PictureBox pb)
                    {
                        pb.Image = img;
                        pb.BackColor = System.Drawing.Color.Transparent;
                    }
                }
            }
            catch { }
        }
        
        
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var btn = this.Controls.Find("btnLogin", true).FirstOrDefault() as Button;
            var lbl = this.Controls.Find("lblStatus", true).FirstOrDefault() as Label;
            var txtUser = this.Controls.Find("txtUsername", true).FirstOrDefault() as TextBox;
            var txtPass = this.Controls.Find("txtPassword", true).FirstOrDefault() as TextBox;
            if (btn != null) btn.Enabled = false;
            if (lbl != null) lbl.Text = "Authenticating...";
            try
            {
                var username = txtUser?.Text?.Trim() ?? string.Empty;
                var password = txtPass?.Text ?? string.Empty;
                var user = await _auth.AuthenticateAsync(username, password);
                if (user != null)
                {
                    if (lbl != null) lbl.Text = $"Welcome {user.Username} ({user.Role})";
                    // set current user in DB context for audit logging
                    Ph_App.Database.PharmacyDBContext.CurrentUser = user;
                    // Log user login
                    try
                    {
                        Ph_App.Database.PharmacyDBContext.AuditLogs.LogUserLogin(user.UserID, user.Username);
                    }
                    catch { }
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
                    if (lbl != null) lbl.Text = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                if (lbl != null) lbl.Text = "Error: " + ex.Message;
            }
            finally
            {
                if (btn != null) btn.Enabled = true;
            }
        }
    }
}

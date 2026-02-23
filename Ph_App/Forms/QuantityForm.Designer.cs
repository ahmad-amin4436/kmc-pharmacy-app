namespace Ph_App.Forms
{
    partial class QuantityForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPacks;
        private System.Windows.Forms.Label lblStrips;
        private System.Windows.Forms.Label lblTablets;
        private System.Windows.Forms.NumericUpDown nudPacks;
        private System.Windows.Forms.NumericUpDown nudStrips;
        private System.Windows.Forms.NumericUpDown nudTablets;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPacks = new System.Windows.Forms.Label();
            this.lblStrips = new System.Windows.Forms.Label();
            this.lblTablets = new System.Windows.Forms.Label();
            this.nudPacks = new System.Windows.Forms.NumericUpDown();
            this.nudStrips = new System.Windows.Forms.NumericUpDown();
            this.nudTablets = new System.Windows.Forms.NumericUpDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudPacks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTablets)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(110, 19);
            this.lblTitle.Text = "Enter Quantity";
            // 
            // lblPacks
            // 
            this.lblPacks.Location = new System.Drawing.Point(20, 52);
            this.lblPacks.Name = "lblPacks";
            this.lblPacks.Size = new System.Drawing.Size(95, 20);
            this.lblPacks.Text = "Packs:";
            this.lblPacks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStrips
            // 
            this.lblStrips.Location = new System.Drawing.Point(20, 92);
            this.lblStrips.Name = "lblStrips";
            this.lblStrips.Size = new System.Drawing.Size(95, 20);
            this.lblStrips.Text = "Strips:";
            this.lblStrips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTablets
            // 
            this.lblTablets.Location = new System.Drawing.Point(20, 132);
            this.lblTablets.Name = "lblTablets";
            this.lblTablets.Size = new System.Drawing.Size(95, 20);
            this.lblTablets.Text = "Tablets:";
            this.lblTablets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudPacks
            // 
            this.nudPacks.Location = new System.Drawing.Point(120, 50);
            this.nudPacks.Name = "nudPacks";
            this.nudPacks.Size = new System.Drawing.Size(150, 20);
            this.nudPacks.Minimum = 0;
            this.nudPacks.Maximum = 100000;
            // 
            // nudStrips
            // 
            this.nudStrips.Location = new System.Drawing.Point(120, 90);
            this.nudStrips.Name = "nudStrips";
            this.nudStrips.Size = new System.Drawing.Size(150, 20);
            this.nudStrips.Minimum = 0;
            this.nudStrips.Maximum = 100000;
            // 
            // nudTablets
            // 
            this.nudTablets.Location = new System.Drawing.Point(120, 130);
            this.nudTablets.Name = "nudTablets";
            this.nudTablets.Size = new System.Drawing.Size(150, 20);
            this.nudTablets.Minimum = 0;
            this.nudTablets.Maximum = 100000;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(80, 170);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.Text = "OK";
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // QuantityForm
            // 
            this.ClientSize = new System.Drawing.Size(350, 220);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPacks);
            this.Controls.Add(this.lblStrips);
            this.Controls.Add(this.lblTablets);
            this.Controls.Add(this.nudPacks);
            this.Controls.Add(this.nudStrips);
            this.Controls.Add(this.nudTablets);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "QuantityForm";
            this.Text = "Enter Quantity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.nudPacks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTablets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

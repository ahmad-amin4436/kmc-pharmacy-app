namespace Ph_App.Forms
{
    partial class StockManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Label lblExpiryFrom;
        private System.Windows.Forms.Label lblExpiryTo;
        private System.Windows.Forms.Label lblCreatedFrom;
        private System.Windows.Forms.Label lblCreatedTo;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.DateTimePicker dtExpiryFrom;
        private System.Windows.Forms.DateTimePicker dtExpiryTo;
        private System.Windows.Forms.DateTimePicker dtCreatedFrom;
        private System.Windows.Forms.DateTimePicker dtCreatedTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgv;

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
            this.lblSearchName = new System.Windows.Forms.Label();
            this.lblExpiryFrom = new System.Windows.Forms.Label();
            this.lblExpiryTo = new System.Windows.Forms.Label();
            this.lblCreatedFrom = new System.Windows.Forms.Label();
            this.lblCreatedTo = new System.Windows.Forms.Label();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.dtExpiryFrom = new System.Windows.Forms.DateTimePicker();
            this.dtExpiryTo = new System.Windows.Forms.DateTimePicker();
            this.dtCreatedFrom = new System.Windows.Forms.DateTimePicker();
            this.dtCreatedTo = new System.Windows.Forms.DateTimePicker();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(145, 21);
            this.lblTitle.Text = "Stock Management";
            // 
            // lblSearchName
            // 
            this.lblSearchName.Location = new System.Drawing.Point(20, 48);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(90, 20);
            this.lblSearchName.Text = "Search by Name:";
            this.lblSearchName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFilterName
            // 
            this.txtFilterName.Location = new System.Drawing.Point(115, 46);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(200, 20);
            this.txtFilterName.TextChanged += new System.EventHandler((s, e) => ApplyFilters());
            // 
            // lblExpiryFrom
            // 
            this.lblExpiryFrom.Location = new System.Drawing.Point(340, 48);
            this.lblExpiryFrom.Name = "lblExpiryFrom";
            this.lblExpiryFrom.Size = new System.Drawing.Size(70, 20);
            this.lblExpiryFrom.Text = "Expiry From:";
            this.lblExpiryFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtExpiryFrom
            // 
            this.dtExpiryFrom.Location = new System.Drawing.Point(415, 46);
            this.dtExpiryFrom.Name = "dtExpiryFrom";
            this.dtExpiryFrom.Size = new System.Drawing.Size(130, 20);
            this.dtExpiryFrom.ValueChanged += new System.EventHandler((s, e) => ApplyFilters());
            // 
            // lblExpiryTo
            // 
            this.lblExpiryTo.Location = new System.Drawing.Point(555, 48);
            this.lblExpiryTo.Name = "lblExpiryTo";
            this.lblExpiryTo.Size = new System.Drawing.Size(55, 20);
            this.lblExpiryTo.Text = "Expiry To:";
            this.lblExpiryTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtExpiryTo
            // 
            this.dtExpiryTo.Location = new System.Drawing.Point(615, 46);
            this.dtExpiryTo.Name = "dtExpiryTo";
            this.dtExpiryTo.Size = new System.Drawing.Size(130, 20);
            this.dtExpiryTo.ValueChanged += new System.EventHandler((s, e) => ApplyFilters());
            // 
            // lblCreatedFrom
            // 
            this.lblCreatedFrom.Location = new System.Drawing.Point(20, 78);
            this.lblCreatedFrom.Name = "lblCreatedFrom";
            this.lblCreatedFrom.Size = new System.Drawing.Size(90, 20);
            this.lblCreatedFrom.Text = "Created From:";
            this.lblCreatedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtCreatedFrom
            // 
            this.dtCreatedFrom.Location = new System.Drawing.Point(115, 76);
            this.dtCreatedFrom.Name = "dtCreatedFrom";
            this.dtCreatedFrom.Size = new System.Drawing.Size(200, 20);
            this.dtCreatedFrom.ValueChanged += new System.EventHandler((s, e) => ApplyFilters());
            // 
            // lblCreatedTo
            // 
            this.lblCreatedTo.Location = new System.Drawing.Point(340, 78);
            this.lblCreatedTo.Name = "lblCreatedTo";
            this.lblCreatedTo.Size = new System.Drawing.Size(70, 20);
            this.lblCreatedTo.Text = "Created To:";
            this.lblCreatedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtCreatedTo
            // 
            this.dtCreatedTo.Location = new System.Drawing.Point(415, 76);
            this.dtCreatedTo.Name = "dtCreatedTo";
            this.dtCreatedTo.Size = new System.Drawing.Size(200, 20);
            this.dtCreatedTo.ValueChanged += new System.EventHandler((s, e) => ApplyFilters());
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(635, 74);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(100, 24);
            this.btnFilter.Text = "Apply Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Location = new System.Drawing.Point(745, 74);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(90, 24);
            this.btnClearFilters.Text = "Clear";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.BtnClearFilters_Click);
            // 
            // dgv
            // 
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.Location = new System.Drawing.Point(20, 112);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(860, 465);
            this.dgv.ReadOnly = true;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.MultiSelect = false;
            this.dgv.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(20, 590);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler((s, e) => this.Close());
            // 
            // StockManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 640);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.lblExpiryFrom);
            this.Controls.Add(this.dtExpiryFrom);
            this.Controls.Add(this.lblExpiryTo);
            this.Controls.Add(this.dtExpiryTo);
            this.Controls.Add(this.lblCreatedFrom);
            this.Controls.Add(this.dtCreatedFrom);
            this.Controls.Add(this.lblCreatedTo);
            this.Controls.Add(this.dtCreatedTo);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnClose);
            this.Name = "StockManagementForm";
            this.Text = "Stock Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

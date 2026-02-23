namespace Ph_App.Forms
{
    partial class ProfitReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Label lblMinProfitFrom;
        private System.Windows.Forms.Label lblMinProfitTo;
        private System.Windows.Forms.DateTimePicker dtDateFrom;
        private System.Windows.Forms.DateTimePicker dtDateTo;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.NumericUpDown numMinProfitFrom;
        private System.Windows.Forms.NumericUpDown numMinProfitTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnClose;

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
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.lblMinProfitFrom = new System.Windows.Forms.Label();
            this.lblMinProfitTo = new System.Windows.Forms.Label();
            this.dtDateFrom = new System.Windows.Forms.DateTimePicker();
            this.dtDateTo = new System.Windows.Forms.DateTimePicker();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.numMinProfitFrom = new System.Windows.Forms.NumericUpDown();
            this.numMinProfitTo = new System.Windows.Forms.NumericUpDown();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 21);
            this.lblTitle.Text = "Profit Report";
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.Location = new System.Drawing.Point(20, 48);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(75, 20);
            this.lblDateFrom.Text = "Date From:";
            this.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtDateFrom
            // 
            this.dtDateFrom.Location = new System.Drawing.Point(100, 46);
            this.dtDateFrom.Name = "dtDateFrom";
            this.dtDateFrom.Size = new System.Drawing.Size(120, 20);
            // 
            // lblDateTo
            // 
            this.lblDateTo.Location = new System.Drawing.Point(230, 48);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(60, 20);
            this.lblDateTo.Text = "Date To:";
            this.lblDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtDateTo
            // 
            this.dtDateTo.Location = new System.Drawing.Point(295, 46);
            this.dtDateTo.Name = "dtDateTo";
            this.dtDateTo.Size = new System.Drawing.Size(120, 20);
            // 
            // lblSearchName
            // 
            this.lblSearchName.Location = new System.Drawing.Point(425, 48);
            this.lblSearchName.Name = "lblSearchName";
            this.lblSearchName.Size = new System.Drawing.Size(90, 20);
            this.lblSearchName.Text = "Search by Name:";
            this.lblSearchName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFilterName
            // 
            this.txtFilterName.Location = new System.Drawing.Point(520, 46);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(150, 20);
            // 
            // lblMinProfitFrom
            // 
            this.lblMinProfitFrom.Location = new System.Drawing.Point(20, 78);
            this.lblMinProfitFrom.Name = "lblMinProfitFrom";
            this.lblMinProfitFrom.Size = new System.Drawing.Size(90, 20);
            this.lblMinProfitFrom.Text = "Profit From:";
            this.lblMinProfitFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMinProfitFrom
            // 
            this.numMinProfitFrom.Location = new System.Drawing.Point(115, 76);
            this.numMinProfitFrom.Name = "numMinProfitFrom";
            this.numMinProfitFrom.Size = new System.Drawing.Size(80, 20);
            this.numMinProfitFrom.Minimum = 0;
            this.numMinProfitFrom.Maximum = 100000;
            this.numMinProfitFrom.Value = 0;
            // 
            // lblMinProfitTo
            // 
            this.lblMinProfitTo.Location = new System.Drawing.Point(210, 78);
            this.lblMinProfitTo.Name = "lblMinProfitTo";
            this.lblMinProfitTo.Size = new System.Drawing.Size(75, 20);
            this.lblMinProfitTo.Text = "Profit To:";
            this.lblMinProfitTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMinProfitTo
            // 
            this.numMinProfitTo.Location = new System.Drawing.Point(290, 76);
            this.numMinProfitTo.Name = "numMinProfitTo";
            this.numMinProfitTo.Size = new System.Drawing.Size(80, 20);
            this.numMinProfitTo.Minimum = 0;
            this.numMinProfitTo.Maximum = 100000;
            this.numMinProfitTo.Value = 10000;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(680, 44);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 24);
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Location = new System.Drawing.Point(760, 44);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(80, 24);
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.BtnClearFilters_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(130, 525);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 28);
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // dgv
            // 
            this.dgv.Location = new System.Drawing.Point(20, 110);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(860, 400);
            this.dgv.ReadOnly = true;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(20, 525);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // ProfitReportForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDateFrom);
            this.Controls.Add(this.dtDateFrom);
            this.Controls.Add(this.lblDateTo);
            this.Controls.Add(this.dtDateTo);
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.lblMinProfitFrom);
            this.Controls.Add(this.numMinProfitFrom);
            this.Controls.Add(this.lblMinProfitTo);
            this.Controls.Add(this.numMinProfitTo);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnClose);
            this.Name = "ProfitReportForm";
            this.Text = "Profit Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

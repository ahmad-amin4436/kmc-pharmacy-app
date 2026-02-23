namespace Ph_App.Forms
{
    partial class LowStockReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Label lblMinStockFrom;
        private System.Windows.Forms.Label lblMinStockTo;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.NumericUpDown numMinStockFrom;
        private System.Windows.Forms.NumericUpDown numMinStockTo;
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
            this.lblSearchName = new System.Windows.Forms.Label();
            this.lblMinStockFrom = new System.Windows.Forms.Label();
            this.lblMinStockTo = new System.Windows.Forms.Label();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.numMinStockFrom = new System.Windows.Forms.NumericUpDown();
            this.numMinStockTo = new System.Windows.Forms.NumericUpDown();
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
            this.lblTitle.Size = new System.Drawing.Size(130, 21);
            this.lblTitle.Text = "Low Stock Report";
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
            // 
            // lblMinStockFrom
            // 
            this.lblMinStockFrom.Location = new System.Drawing.Point(330, 48);
            this.lblMinStockFrom.Name = "lblMinStockFrom";
            this.lblMinStockFrom.Size = new System.Drawing.Size(85, 20);
            this.lblMinStockFrom.Text = "Min Stock From:";
            this.lblMinStockFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMinStockFrom
            // 
            this.numMinStockFrom.Location = new System.Drawing.Point(420, 46);
            this.numMinStockFrom.Name = "numMinStockFrom";
            this.numMinStockFrom.Size = new System.Drawing.Size(80, 20);
            this.numMinStockFrom.Minimum = 0;
            this.numMinStockFrom.Value = 0;
            // 
            // lblMinStockTo
            // 
            this.lblMinStockTo.Location = new System.Drawing.Point(510, 48);
            this.lblMinStockTo.Name = "lblMinStockTo";
            this.lblMinStockTo.Size = new System.Drawing.Size(75, 20);
            this.lblMinStockTo.Text = "Min Stock To:";
            this.lblMinStockTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMinStockTo
            // 
            this.numMinStockTo.Location = new System.Drawing.Point(590, 46);
            this.numMinStockTo.Name = "numMinStockTo";
            this.numMinStockTo.Size = new System.Drawing.Size(80, 20);
            this.numMinStockTo.Minimum = 0;
            this.numMinStockTo.Value = 100;
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
            this.dgv.Location = new System.Drawing.Point(20, 80);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(860, 430);
            this.dgv.ReadOnly = true;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.ScrollBars = System.Windows.Forms.ScrollBars.Both;
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
            // LowStockReportForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.lblMinStockFrom);
            this.Controls.Add(this.numMinStockFrom);
            this.Controls.Add(this.lblMinStockTo);
            this.Controls.Add(this.numMinStockTo);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnClose);
            this.Name = "LowStockReportForm";
            this.Text = "Low Stock Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

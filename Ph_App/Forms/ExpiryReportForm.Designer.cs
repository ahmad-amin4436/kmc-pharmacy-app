namespace Ph_App.Forms
{
    partial class ExpiryReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.Label lblExpiryFrom;
        private System.Windows.Forms.Label lblExpiryTo;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.DateTimePicker dtExpiryFrom;
        private System.Windows.Forms.DateTimePicker dtExpiryTo;
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
            this.lblExpiryFrom = new System.Windows.Forms.Label();
            this.lblExpiryTo = new System.Windows.Forms.Label();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.dtExpiryFrom = new System.Windows.Forms.DateTimePicker();
            this.dtExpiryTo = new System.Windows.Forms.DateTimePicker();
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
            this.lblTitle.Size = new System.Drawing.Size(105, 21);
            this.lblTitle.Text = "Expiry Report";
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
            this.txtFilterName.TextChanged += new System.EventHandler((s, e) => this.ApplyExpiredMedicinesFilter());
            // 
            // lblExpiryFrom
            // 
            this.lblExpiryFrom.Location = new System.Drawing.Point(330, 48);
            this.lblExpiryFrom.Name = "lblExpiryFrom";
            this.lblExpiryFrom.Size = new System.Drawing.Size(70, 20);
            this.lblExpiryFrom.Text = "Expiry From:";
            this.lblExpiryFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtExpiryFrom
            // 
            this.dtExpiryFrom.Location = new System.Drawing.Point(405, 46);
            this.dtExpiryFrom.Name = "dtExpiryFrom";
            this.dtExpiryFrom.Size = new System.Drawing.Size(120, 20);
            this.dtExpiryFrom.ValueChanged += new System.EventHandler((s, e) => this.ApplyExpiredMedicinesFilter());
            // 
            // lblExpiryTo
            // 
            this.lblExpiryTo.Location = new System.Drawing.Point(533, 48);
            this.lblExpiryTo.Name = "lblExpiryTo";
            this.lblExpiryTo.Size = new System.Drawing.Size(55, 20);
            this.lblExpiryTo.Text = "Expiry To:";
            this.lblExpiryTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtExpiryTo
            // 
            this.dtExpiryTo.Location = new System.Drawing.Point(593, 46);
            this.dtExpiryTo.Name = "dtExpiryTo";
            this.dtExpiryTo.Size = new System.Drawing.Size(120, 20);
            this.dtExpiryTo.ValueChanged += new System.EventHandler((s, e) => this.ApplyExpiredMedicinesFilter());
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(720, 44);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 24);
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Location = new System.Drawing.Point(800, 44);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(80, 24);
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.BtnClearFilters_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(130, 495);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 28);
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // dgv
            // 
            this.dgv.Location = new System.Drawing.Point(20, 82);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(860, 398);
            this.dgv.ReadOnly = true;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(20, 495);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // ExpiryReportForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.lblExpiryFrom);
            this.Controls.Add(this.dtExpiryFrom);
            this.Controls.Add(this.lblExpiryTo);
            this.Controls.Add(this.dtExpiryTo);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnClearFilters);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnClose);
            this.Name = "ExpiryReportForm";
            this.Text = "Expiry Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

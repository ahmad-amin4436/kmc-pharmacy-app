namespace Ph_App.Forms
{
    partial class MedicineEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblGeneric;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.Label lblExpiry;
        private System.Windows.Forms.Label lblPackQty;
        private System.Windows.Forms.Label lblStripQty;
        private System.Windows.Forms.Label lblMinStock;
        private System.Windows.Forms.Label lblPurchasePricePack;
        private System.Windows.Forms.Label lblPurchasePriceStrip;
        private System.Windows.Forms.Label lblPurchasePriceTablet;
        private System.Windows.Forms.Label lblSalePricePack;
        private System.Windows.Forms.Label lblSalePriceStrip;
        private System.Windows.Forms.Label lblSalePriceTablet;
        private System.Windows.Forms.Label lblCurrentPacks;
        private System.Windows.Forms.Label lblCurrentStrips;
        private System.Windows.Forms.Label lblCurrentTablets;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtGeneric;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.DateTimePicker dtExpiry;
        private System.Windows.Forms.TextBox txtPackQty;
        private System.Windows.Forms.TextBox txtStripQty;
        private System.Windows.Forms.TextBox txtMinStock;
        private System.Windows.Forms.TextBox txtPurchasePricePack;
        private System.Windows.Forms.TextBox txtPurchasePriceStrip;
        private System.Windows.Forms.TextBox txtPurchasePriceTablet;
        private System.Windows.Forms.TextBox txtSalePricePack;
        private System.Windows.Forms.TextBox txtSalePriceStrip;
        private System.Windows.Forms.TextBox txtSalePriceTablet;
        private System.Windows.Forms.TextBox txtCurrentPacks;
        private System.Windows.Forms.TextBox txtCurrentStrips;
        private System.Windows.Forms.TextBox txtCurrentTablets;
        private System.Windows.Forms.Button btnSave;
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblGeneric = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblBatch = new System.Windows.Forms.Label();
            this.lblExpiry = new System.Windows.Forms.Label();
            this.lblPackQty = new System.Windows.Forms.Label();
            this.lblStripQty = new System.Windows.Forms.Label();
            this.lblMinStock = new System.Windows.Forms.Label();
            this.lblPurchasePricePack = new System.Windows.Forms.Label();
            this.lblPurchasePriceStrip = new System.Windows.Forms.Label();
            this.lblPurchasePriceTablet = new System.Windows.Forms.Label();
            this.lblSalePricePack = new System.Windows.Forms.Label();
            this.lblSalePriceStrip = new System.Windows.Forms.Label();
            this.lblSalePriceTablet = new System.Windows.Forms.Label();
            this.lblCurrentPacks = new System.Windows.Forms.Label();
            this.lblCurrentStrips = new System.Windows.Forms.Label();
            this.lblCurrentTablets = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtGeneric = new System.Windows.Forms.TextBox();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.dtExpiry = new System.Windows.Forms.DateTimePicker();
            this.txtPackQty = new System.Windows.Forms.TextBox();
            this.txtStripQty = new System.Windows.Forms.TextBox();
            this.txtMinStock = new System.Windows.Forms.TextBox();
            this.txtPurchasePricePack = new System.Windows.Forms.TextBox();
            this.txtPurchasePriceStrip = new System.Windows.Forms.TextBox();
            this.txtPurchasePriceTablet = new System.Windows.Forms.TextBox();
            this.txtSalePricePack = new System.Windows.Forms.TextBox();
            this.txtSalePriceStrip = new System.Windows.Forms.TextBox();
            this.txtSalePriceTablet = new System.Windows.Forms.TextBox();
            this.txtCurrentPacks = new System.Windows.Forms.TextBox();
            this.txtCurrentStrips = new System.Windows.Forms.TextBox();
            this.txtCurrentTablets = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 21);
            this.lblTitle.Text = "Add / Edit Medicine";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(20, 47);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(120, 20);
            this.lblName.Text = "Medicine Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGeneric
            // 
            this.lblGeneric.Location = new System.Drawing.Point(20, 87);
            this.lblGeneric.Name = "lblGeneric";
            this.lblGeneric.Size = new System.Drawing.Size(120, 20);
            this.lblGeneric.Text = "Generic Name:";
            this.lblGeneric.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(20, 127);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(120, 20);
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBatch
            // 
            this.lblBatch.Location = new System.Drawing.Point(20, 167);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(120, 20);
            this.lblBatch.Text = "Batch No:";
            this.lblBatch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExpiry
            // 
            this.lblExpiry.Location = new System.Drawing.Point(20, 207);
            this.lblExpiry.Name = "lblExpiry";
            this.lblExpiry.Size = new System.Drawing.Size(120, 20);
            this.lblExpiry.Text = "Expiry Date:";
            this.lblExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPackQty
            // 
            this.lblPackQty.Location = new System.Drawing.Point(20, 247);
            this.lblPackQty.Name = "lblPackQty";
            this.lblPackQty.Size = new System.Drawing.Size(120, 20);
            this.lblPackQty.Text = "Pack Qty:";
            this.lblPackQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStripQty
            // 
            this.lblStripQty.Location = new System.Drawing.Point(20, 287);
            this.lblStripQty.Name = "lblStripQty";
            this.lblStripQty.Size = new System.Drawing.Size(120, 20);
            this.lblStripQty.Text = "Strip Qty:";
            this.lblStripQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMinStock
            // 
            this.lblMinStock.Location = new System.Drawing.Point(20, 327);
            this.lblMinStock.Name = "lblMinStock";
            this.lblMinStock.Size = new System.Drawing.Size(120, 20);
            this.lblMinStock.Text = "Min Stock Level:";
            this.lblMinStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurchasePricePack
            // 
            this.lblPurchasePricePack.Location = new System.Drawing.Point(20, 367);
            this.lblPurchasePricePack.Name = "lblPurchasePricePack";
            this.lblPurchasePricePack.Size = new System.Drawing.Size(120, 20);
            this.lblPurchasePricePack.Text = "Purchase (Pack):";
            this.lblPurchasePricePack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurchasePriceStrip
            // 
            this.lblPurchasePriceStrip.Location = new System.Drawing.Point(20, 407);
            this.lblPurchasePriceStrip.Name = "lblPurchasePriceStrip";
            this.lblPurchasePriceStrip.Size = new System.Drawing.Size(120, 20);
            this.lblPurchasePriceStrip.Text = "Purchase (Strip):";
            this.lblPurchasePriceStrip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurchasePriceTablet
            // 
            this.lblPurchasePriceTablet.Location = new System.Drawing.Point(20, 447);
            this.lblPurchasePriceTablet.Name = "lblPurchasePriceTablet";
            this.lblPurchasePriceTablet.Size = new System.Drawing.Size(120, 20);
            this.lblPurchasePriceTablet.Text = "Purchase (Tablet):";
            this.lblPurchasePriceTablet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalePricePack
            // 
            this.lblSalePricePack.Location = new System.Drawing.Point(350, 367);
            this.lblSalePricePack.Name = "lblSalePricePack";
            this.lblSalePricePack.Size = new System.Drawing.Size(65, 20);
            this.lblSalePricePack.Text = "Sale (Pack):";
            this.lblSalePricePack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalePriceStrip
            // 
            this.lblSalePriceStrip.Location = new System.Drawing.Point(350, 407);
            this.lblSalePriceStrip.Name = "lblSalePriceStrip";
            this.lblSalePriceStrip.Size = new System.Drawing.Size(65, 20);
            this.lblSalePriceStrip.Text = "Sale (Strip):";
            this.lblSalePriceStrip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalePriceTablet
            // 
            this.lblSalePriceTablet.Location = new System.Drawing.Point(350, 447);
            this.lblSalePriceTablet.Name = "lblSalePriceTablet";
            this.lblSalePriceTablet.Size = new System.Drawing.Size(65, 20);
            this.lblSalePriceTablet.Text = "Sale (Tablet):";
            this.lblSalePriceTablet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentPacks
            // 
            this.lblCurrentPacks.Location = new System.Drawing.Point(20, 487);
            this.lblCurrentPacks.Name = "lblCurrentPacks";
            this.lblCurrentPacks.Size = new System.Drawing.Size(120, 20);
            this.lblCurrentPacks.Text = "Current Packs:";
            this.lblCurrentPacks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentStrips
            // 
            this.lblCurrentStrips.Location = new System.Drawing.Point(350, 487);
            this.lblCurrentStrips.Name = "lblCurrentStrips";
            this.lblCurrentStrips.Size = new System.Drawing.Size(65, 20);
            this.lblCurrentStrips.Text = "Current Strips:";
            this.lblCurrentStrips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentTablets
            // 
            this.lblCurrentTablets.Location = new System.Drawing.Point(620, 487);
            this.lblCurrentTablets.Name = "lblCurrentTablets";
            this.lblCurrentTablets.Size = new System.Drawing.Size(65, 20);
            this.lblCurrentTablets.Text = "Current Tablets:";
            this.lblCurrentTablets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Controls layout
            // 
            this.txtName.Location = new System.Drawing.Point(150, 45);
            this.txtName.Size = new System.Drawing.Size(250, 20);
            this.txtGeneric.Location = new System.Drawing.Point(150, 85);
            this.txtGeneric.Size = new System.Drawing.Size(250, 20);
            this.txtCompany.Location = new System.Drawing.Point(150, 125);
            this.txtCompany.Size = new System.Drawing.Size(250, 20);
            this.txtBatch.Location = new System.Drawing.Point(150, 165);
            this.txtBatch.Size = new System.Drawing.Size(250, 20);
            this.dtExpiry.Location = new System.Drawing.Point(150, 205);
            this.dtExpiry.Size = new System.Drawing.Size(200, 20);
            this.txtPackQty.Location = new System.Drawing.Point(150, 245);
            this.txtPackQty.Size = new System.Drawing.Size(100, 20);
            this.txtStripQty.Location = new System.Drawing.Point(150, 285);
            this.txtStripQty.Size = new System.Drawing.Size(100, 20);
            this.txtMinStock.Location = new System.Drawing.Point(150, 325);
            this.txtMinStock.Size = new System.Drawing.Size(100, 20);
            this.txtPurchasePricePack.Location = new System.Drawing.Point(150, 365);
            this.txtPurchasePricePack.Size = new System.Drawing.Size(100, 20);
            this.txtPurchasePriceStrip.Location = new System.Drawing.Point(150, 405);
            this.txtPurchasePriceStrip.Size = new System.Drawing.Size(100, 20);
            this.txtPurchasePriceTablet.Location = new System.Drawing.Point(150, 445);
            this.txtPurchasePriceTablet.Size = new System.Drawing.Size(100, 20);
            this.txtSalePricePack.Location = new System.Drawing.Point(420, 365);
            this.txtSalePricePack.Size = new System.Drawing.Size(100, 20);
            this.txtSalePriceStrip.Location = new System.Drawing.Point(420, 405);
            this.txtSalePriceStrip.Size = new System.Drawing.Size(100, 20);
            this.txtSalePriceTablet.Location = new System.Drawing.Point(420, 445);
            this.txtSalePriceTablet.Size = new System.Drawing.Size(100, 20);
            this.txtCurrentPacks.Location = new System.Drawing.Point(150, 485);
            this.txtCurrentPacks.Size = new System.Drawing.Size(100, 20);
            this.txtCurrentStrips.Location = new System.Drawing.Point(420, 485);
            this.txtCurrentStrips.Size = new System.Drawing.Size(100, 20);
            this.txtCurrentTablets.Location = new System.Drawing.Point(690, 485);
            this.txtCurrentTablets.Size = new System.Drawing.Size(100, 20);
            this.btnSave.Location = new System.Drawing.Point(300, 525);
            this.btnCancel.Location = new System.Drawing.Point(420, 525);
            this.btnSave.Text = "Save";
            this.btnCancel.Text = "Cancel";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.btnCancel.Click += new System.EventHandler((s, e) => { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; this.Close(); });

            // 
            // MedicineEditForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblGeneric);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.lblBatch);
            this.Controls.Add(this.lblExpiry);
            this.Controls.Add(this.lblPackQty);
            this.Controls.Add(this.lblStripQty);
            this.Controls.Add(this.lblMinStock);
            this.Controls.Add(this.lblPurchasePricePack);
            this.Controls.Add(this.lblPurchasePriceStrip);
            this.Controls.Add(this.lblPurchasePriceTablet);
            this.Controls.Add(this.lblSalePricePack);
            this.Controls.Add(this.lblSalePriceStrip);
            this.Controls.Add(this.lblSalePriceTablet);
            this.Controls.Add(this.lblCurrentPacks);
            this.Controls.Add(this.lblCurrentStrips);
            this.Controls.Add(this.lblCurrentTablets);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtGeneric);
            this.Controls.Add(this.txtCompany);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.dtExpiry);
            this.Controls.Add(this.txtPackQty);
            this.Controls.Add(this.txtStripQty);
            this.Controls.Add(this.txtMinStock);
            this.Controls.Add(this.txtPurchasePricePack);
            this.Controls.Add(this.txtPurchasePriceStrip);
            this.Controls.Add(this.txtPurchasePriceTablet);
            this.Controls.Add(this.txtSalePricePack);
            this.Controls.Add(this.txtSalePriceStrip);
            this.Controls.Add(this.txtSalePriceTablet);
            this.Controls.Add(this.txtCurrentPacks);
            this.Controls.Add(this.txtCurrentStrips);
            this.Controls.Add(this.txtCurrentTablets);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "MedicineEditForm";
            this.Text = "Add / Edit Medicine";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}

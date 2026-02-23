using System;
using System.Windows.Forms;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class MedicineEditForm : Form
    {
        public MedicineEditForm() : this(null) { }

        // New constructor to allow demo prefill
        public MedicineEditForm(bool useDemo) : this(null)
        {
            if (useDemo)
            {
                PopulateDemo();
            }
        }

        public Medicine Result { get; private set; }

        public MedicineEditForm(Medicine m)
        {
            InitializeComponent();

            if (m != null)
            {
                // populate fields
                txtName.Text = m.MedicineName;
                txtGeneric.Text = m.GenericName;
                txtCompany.Text = m.Company;
                txtBatch.Text = m.BatchNo;
                dtExpiry.Value = m.ExpiryDate ?? DateTime.Now;
                txtPackQty.Text = m.PackQuantity.ToString();
                txtStripQty.Text = m.StripQuantity.ToString();
                txtMinStock.Text = m.MinimumStockLevel.ToString();
                txtPurchasePricePack.Text = m.PurchasePricePerPack.ToString("F2");
                txtPurchasePriceStrip.Text = m.PurchasePricePerStrip.ToString("F2");
                txtPurchasePriceTablet.Text = m.PurchasePricePerTablet.ToString("F2");
                txtSalePricePack.Text = m.SalePricePerPack.ToString("F2");
                txtSalePriceStrip.Text = m.SalePricePerStrip.ToString("F2");
                txtSalePriceTablet.Text = m.SalePricePerTablet.ToString("F2");
                txtCurrentPacks.Text = m.CurrentStockPacks.ToString();
                txtCurrentStrips.Text = m.CurrentStockStrips.ToString();
                txtCurrentTablets.Text = m.CurrentStockTablets.ToString();
                Result = new Medicine { MedicineID = m.MedicineID };
            }
        }

        private void PopulateDemo()
        {
            try
            {
                txtName.Text = "Ibuprofen200mg";
                txtGeneric.Text = "Ibuprofen";
                txtCompany.Text = "DemoPharma";
                txtBatch.Text = "D-001";
                dtExpiry.Value = DateTime.Now.AddYears(2);
                txtPackQty.Text = "10";
                txtStripQty.Text = "10";
                txtMinStock.Text = "5";
                txtPurchasePricePack.Text = "100.00";
                txtPurchasePriceStrip.Text = "10.00";
                txtPurchasePriceTablet.Text = "1.00";
                txtSalePricePack.Text = "130.00";
                txtSalePriceStrip.Text = "13.00";
                txtSalePriceTablet.Text = "1.30";
                txtCurrentPacks.Text = "50";
                txtCurrentStrips.Text = "0";
                txtCurrentTablets.Text = "0";
            }
            catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Name required"); return; }
            int packQty = 0, stripQty = 0, minStock = 0;
            int.TryParse(txtPackQty.Text, out packQty);
            int.TryParse(txtStripQty.Text, out stripQty);
            int.TryParse(txtMinStock.Text, out minStock);

            decimal pPricePack = 0, pPriceStrip = 0, pPriceTablet = 0;
            decimal sPricePack = 0, sPriceStrip = 0, sPriceTablet = 0;
            decimal.TryParse(txtPurchasePricePack.Text, out pPricePack);
            decimal.TryParse(txtPurchasePriceStrip.Text, out pPriceStrip);
            decimal.TryParse(txtPurchasePriceTablet.Text, out pPriceTablet);
            decimal.TryParse(txtSalePricePack.Text, out sPricePack);
            decimal.TryParse(txtSalePriceStrip.Text, out sPriceStrip);
            decimal.TryParse(txtSalePriceTablet.Text, out sPriceTablet);

            int curPacks = 0, curStrips = 0, curTablets = 0;
            int.TryParse(txtCurrentPacks.Text, out curPacks);
            int.TryParse(txtCurrentStrips.Text, out curStrips);
            int.TryParse(txtCurrentTablets.Text, out curTablets);

            Result = new Medicine
            {
                MedicineName = txtName.Text.Trim(),
                GenericName = txtGeneric.Text.Trim(),
                Company = txtCompany.Text.Trim(),
                BatchNo = txtBatch.Text.Trim(),
                ExpiryDate = dtExpiry.Value,
                PackQuantity = packQty,
                StripQuantity = stripQty,
                MinimumStockLevel = minStock,
                PurchasePricePerPack = pPricePack,
                PurchasePricePerStrip = pPriceStrip,
                PurchasePricePerTablet = pPriceTablet,
                SalePricePerPack = sPricePack,
                SalePricePerStrip = sPriceStrip,
                SalePricePerTablet = sPriceTablet,
                CurrentStockPacks = curPacks,
                CurrentStockStrips = curStrips,
                CurrentStockTablets = curTablets,
                CreatedDate = DateTime.Now
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

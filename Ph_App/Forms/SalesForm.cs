using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Models;
using Ph_App.Database;
using Ph_App.DAL;
using Ph_App.Utils;

namespace Ph_App.Forms
{
    public partial class SalesForm : ResponsiveForm
    {
        public SalesForm()
        {
            InitializeComponent();
            LoadMedicineSearch();
            txtSearch.TextChanged += TxtSearch_TextChanged;
            
            // Log form access
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "VIEW", "Forms", "SalesForm", "", "User accessed Sales/POS form");
        }

        private List<SaleLine> Cart = new List<SaleLine>();

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var q = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
            var list = PharmacyDBContext.Medicines.GetAll().Where(m => !m.IsDeleted && ((m.MedicineName ?? string.Empty).ToLowerInvariant().Contains(q) || (m.GenericName ?? string.Empty).ToLowerInvariant().Contains(q))).Select(m => new { m.MedicineID, m.MedicineName, m.GenericName, m.CurrentStockPacks, Expiry = m.ExpiryDate?.ToString("yyyy-MM-dd") }).ToList();
            dgvSearch.DataSource = list;
        }

        private void LoadMedicineSearch()
        {
            var list = PharmacyDBContext.Medicines.GetAll().Where(m => !m.IsDeleted).Select(m => new { m.MedicineID, m.MedicineName, m.GenericName, m.CurrentStockPacks, Expiry = m.ExpiryDate?.ToString("yyyy-MM-dd") }).ToList();
            dgvSearch.DataSource = list;
        }

        private void RefreshCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = Cart.Select(c => new { c.MedicineID, c.Name, c.Packs, c.Strips, c.Tablets, c.UnitPricePack, c.UnitPriceStrip, c.UnitPriceTablet, c.Total }).ToList();
            //lblTotal.Text = "Total: " + Cart.Sum(x => x.Total).ToString("C2");
            lblTotal.Text = "Total: Rs " + Cart.Sum(x => x.Total).ToString("N2");
        }

        private int GetReservedPacksInCart(int medicineId, SaleLine exclude = null)
        {
            return Cart.Where(c => c.MedicineID == medicineId && c != exclude).Sum(c => c.Packs);
        }
        private int GetReservedStripsInCart(int medicineId, SaleLine exclude = null)
        {
            return Cart.Where(c => c.MedicineID == medicineId && c != exclude).Sum(c => c.Strips);
        }
        private int GetReservedTabletsInCart(int medicineId, SaleLine exclude = null)
        {
            return Cart.Where(c => c.MedicineID == medicineId && c != exclude).Sum(c => c.Tablets);
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvSearch.SelectedRows.Count == 0) { MessageBox.Show("Select a medicine first."); return; }
            var id = Convert.ToInt32(dgvSearch.SelectedRows[0].Cells["MedicineID"].Value);
            var med = PharmacyDBContext.Medicines.GetById(id);
            if (med == null) return;

            using (var qf = new QuantityForm())
            {
                if (qf.ShowDialog(this) != DialogResult.OK) return;
                int packs = qf.Packs;
                int strips = qf.Strips;
                int tablets = qf.Tablets;

                var reservedPacks = GetReservedPacksInCart(med.MedicineID);
                var reservedStrips = GetReservedStripsInCart(med.MedicineID);
                var reservedTablets = GetReservedTabletsInCart(med.MedicineID);
                if (packs + reservedPacks > med.CurrentStockPacks) { MessageBox.Show("Not enough packs in stock."); return; }
                if (strips + reservedStrips > med.CurrentStockStrips) { MessageBox.Show("Not enough strips in stock."); return; }
                if (tablets + reservedTablets > med.CurrentStockTablets) { MessageBox.Show("Not enough tablets in stock."); return; }

                decimal unitPack = med.SalePricePerPack;
                decimal unitStrip = med.SalePricePerStrip;
                decimal unitTablet = med.SalePricePerTablet;

                decimal total = packs * unitPack + strips * unitStrip + tablets * unitTablet;
                var line = new SaleLine { MedicineID = med.MedicineID, Name = med.MedicineName, Packs = packs, Strips = strips, Tablets = tablets, UnitPricePack = unitPack, UnitPriceStrip = unitStrip, UnitPriceTablet = unitTablet, Total = total };
                Cart.Add(line);
                RefreshCart();
                try { PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CREATE", "Cart", line.MedicineID.ToString(), "", $"Added to cart: {line.Name} Packs:{line.Packs} Strips:{line.Strips} Tablets:{line.Tablets}"); } catch { }
            }
        }

        private void BtnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0) { MessageBox.Show("Select a cart item first."); return; }
            var medId = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["MedicineID"].Value);
            var line = Cart.FirstOrDefault(c => c.MedicineID == medId && c.Packs == Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Packs"].Value) && c.Strips == Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Strips"].Value) && c.Tablets == Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Tablets"].Value));
            if (line == null) { MessageBox.Show("Selected cart item not found."); return; }
            var med = PharmacyDBContext.Medicines.GetById(line.MedicineID);
            if (med == null) return;

            using (var qf = new QuantityForm(line.Packs, line.Strips, line.Tablets))
            {
                if (qf.ShowDialog(this) == DialogResult.OK)
                {
                    line.Packs = qf.Packs;
                    line.Strips = qf.Strips;
                    line.Tablets = qf.Tablets;
                    line.Total = qf.Packs * med.SalePricePerPack + qf.Strips * med.SalePricePerStrip + qf.Tablets * med.SalePricePerTablet;
                    RefreshCart();
                    try { PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "UPDATE", "Cart", line.MedicineID.ToString(), "", $"Updated cart line: {line.Name} Packs:{line.Packs} Strips:{line.Strips} Tablets:{line.Tablets}"); } catch { }
                }
            }
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "SalesForm.btnCheckout", "", "User clicked Complete Sale button");
            
            if (!Cart.Any()) { MessageBox.Show("Cart is empty."); return; }
            var sale = new Sale { 
                SaleDate = DateTime.Now, 
                TotalAmount = Cart.Sum(c => c.Total), 
                NetAmount = Cart.Sum(c => c.Total), 
                PaidAmount = Cart.Sum(c => c.Total), 
                PaymentMethod = "Cash", 
                InvoiceNo = "S" + DateTime.Now.ToString("yyyyMMddHHmmss"), 
                Notes = "",
                CustomerID = null,
                UserID = PharmacyDBContext.CurrentUser?.UserID ?? 0,
                CreatedDate = DateTime.Now
            };
            
            foreach (var c in Cart)
            {
                var sd = new SaleDetail { 
                    MedicineID = c.MedicineID, 
                    QuantityPacks = c.Packs, 
                    QuantityStrips = c.Strips, 
                    QuantityTablets = c.Tablets, 
                    SalePrice = c.UnitPricePack, 
                    Total = c.Total,
                    CreatedDate = DateTime.Now
                };
                sale.Details.Add(sd);
                var med = PharmacyDBContext.Medicines.GetById(c.MedicineID);
                if (med != null)
                {
                    med.CurrentStockPacks -= c.Packs;
                    med.CurrentStockStrips -= c.Strips;
                    med.CurrentStockTablets -= c.Tablets;
                    med.ModifiedDate = DateTime.Now;
                    PharmacyDBContext.Medicines.Update(med);
                }
            }
            
            var addedSale = PharmacyDBContext.Sales.Add(sale);
            PharmacyDBContext.AuditLogs.LogUserAction(1, "CREATE", "Sales", addedSale.SaleID.ToString(), "", $"Invoice: {addedSale.InvoiceNo}, Total: {addedSale.TotalAmount:C2}");
            MessageBox.Show($"Sale completed. Invoice: {addedSale.InvoiceNo} Total: {addedSale.TotalAmount:C2}");
            Cart.Clear();
            RefreshCart();
            LoadMedicineSearch();
        }

        private void BtnEditItem_Click(object sender, EventArgs e)
        {
            // Edit selected cart item quantities (reuse existing quantity flow)
            if (dgvCart.SelectedRows.Count == 0) { MessageBox.Show("Select a cart item first."); return; }
            // Reuse the existing logic that opens QuantityForm and updates the cart line
            BtnRemoveFromCart_Click(sender, e);
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            // Remove the selected cart item from the cart
            if (dgvCart.SelectedRows.Count == 0) { MessageBox.Show("Select a cart item first."); return; }
            var medId = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["MedicineID"].Value);
            var packs = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Packs"].Value);
            var strips = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Strips"].Value);
            var tablets = Convert.ToInt32(dgvCart.SelectedRows[0].Cells["Tablets"].Value);
            var line = Cart.FirstOrDefault(c => c.MedicineID == medId && c.Packs == packs && c.Strips == strips && c.Tablets == tablets);
            if (line == null) { MessageBox.Show("Selected cart item not found."); return; }
            var dlg = MessageBox.Show($"Remove {line.Name} from cart?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                Cart.Remove(line);
                RefreshCart();
            }
        }

        private class SaleLine
        {
            public int MedicineID { get; set; }
            public string Name { get; set; }
            public int Packs { get; set; }
            public int Strips { get; set; }
            public int Tablets { get; set; }
            public decimal UnitPricePack { get; set; }
            public decimal UnitPriceStrip { get; set; }
            public decimal UnitPriceTablet { get; set; }
            public decimal Total { get; set; }
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Models;
using Ph_App.Database;
using Ph_App.DAL;

namespace Ph_App.Forms
{
    public partial class MedicineForm : MedicineBaseForm
    {
        public MedicineForm()
        {
            InitializeComponent();
            LoadMedicinesGrid();

            // Log form access
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "VIEW", "Forms", "MedicineForm", "", "User accessed Medicine Management");
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try { PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "MedicineForm.btnFilter", "", "User clicked Filter in MedicineForm"); } catch { }
            ApplyQuickFilter();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            try { PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "MedicineForm.btnClearFilters", "", "User clicked Clear filter in MedicineForm"); } catch { }
            // clear textbox if present
            var tb = this.Controls.Find("txtFilter", true);
            if (tb != null && tb.Length > 0 && tb[0] is TextBox t)
            {
                t.Text = string.Empty;
            }
            ApplyQuickFilter();
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyQuickFilter();
        }

        private void ApplyQuickFilter()
        {
            // find txtFilter control if present
            var txts = this.Controls.Find("txtFilter", true);
            var filter = string.Empty;
            if (txts != null && txts.Length > 0 && txts[0] is TextBox tx)
            {
                filter = (tx.Text ?? string.Empty).Trim().ToLowerInvariant();
            }

            // get all medicines and apply quick search across several fields
            var list = PharmacyDBContext.Medicines.GetAll().Where(m => !m.IsDeleted).ToList();
            if (!string.IsNullOrEmpty(filter))
            {
                list = list.Where(m => ((m.MedicineName ?? string.Empty).ToLowerInvariant().Contains(filter)
                    || (m.GenericName ?? string.Empty).ToLowerInvariant().Contains(filter)
                    || (m.Company ?? string.Empty).ToLowerInvariant().Contains(filter)
                    || (m.BatchNo ?? string.Empty).ToLowerInvariant().Contains(filter))).ToList();
            }

            dgv.DataSource = null;
            dgv.DataSource = list.Select(m => new
            {
                m.MedicineID,
                m.MedicineName,
                m.GenericName,
                m.Company,
                m.BatchNo,
                Expiry = m.ExpiryDate?.ToString("yyyy-MM-dd"),
                m.PackQuantity,
                m.StripQuantity,
                m.TabletQuantity,
                m.CurrentStockPacks,
                m.CurrentStockStrips,
                m.CurrentStockTablets,
                m.MinimumStockLevel,
                Created = m.CreatedDate.ToString("yyyy-MM-dd")
            }).ToList();
        }

        private void BtnAddDemo_Click(object sender, EventArgs e)
        {
            using (var form = new MedicineEditForm(true))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    var m = form.Result;
                    m.CreatedDate = DateTime.Now;
                    var addedMedicine = PharmacyDBContext.Medicines.Add(m);
                    PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CREATE", "Medicines", addedMedicine.MedicineID.ToString(), "", $"Medicine: {addedMedicine.MedicineName}");
                    LoadMedicinesGrid();
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "MedicineForm.btnAdd", "", "User clicked Add Medicine button");

            using (var form = new MedicineEditForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    var m = form.Result;
                    m.CreatedDate = DateTime.Now;
                    var addedMedicine = PharmacyDBContext.Medicines.Add(m);
                    PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CREATE", "Medicines", addedMedicine.MedicineID.ToString(), "", $"Medicine: {addedMedicine.MedicineName}");
                    LoadMedicinesGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "MedicineForm.btnEdit", "", "User clicked Edit Medicine button");

            var sel = GetSelectedMedicine();
            if (sel == null)
            {
                MessageBox.Show("Select a medicine first.");
                return;
            }

            using (var form = new MedicineEditForm(sel))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    var updated = form.Result;
                    var oldValue = $"MedicineName: {sel.MedicineName}, GenericName: {sel.GenericName}";
                    var newValue = $"MedicineName: {updated.MedicineName}, GenericName: {updated.GenericName}";
                    sel.MedicineName = updated.MedicineName;
                    sel.GenericName = updated.GenericName;
                    sel.Company = updated.Company;
                    sel.BatchNo = updated.BatchNo;
                    sel.ExpiryDate = updated.ExpiryDate;
                    sel.PackQuantity = updated.PackQuantity;
                    sel.StripQuantity = updated.StripQuantity;
                    sel.TabletQuantity = updated.TabletQuantity;
                    sel.MinimumStockLevel = updated.MinimumStockLevel;
                    sel.PurchasePricePerPack = updated.PurchasePricePerPack;
                    sel.PurchasePricePerStrip = updated.PurchasePricePerStrip;
                    sel.PurchasePricePerTablet = updated.PurchasePricePerTablet;
                    sel.SalePricePerPack = updated.SalePricePerPack;
                    sel.SalePricePerStrip = updated.SalePricePerStrip;
                    sel.SalePricePerTablet = updated.SalePricePerTablet;
                    sel.CurrentStockPacks = updated.CurrentStockPacks;
                    sel.CurrentStockStrips = updated.CurrentStockStrips;
                    sel.CurrentStockTablets = updated.CurrentStockTablets;
                    sel.SupplierID = updated.SupplierID;
                    sel.ModifiedDate = DateTime.Now;
                    PharmacyDBContext.Medicines.Update(sel);
                    PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "UPDATE", "Medicines", sel.MedicineID.ToString(), oldValue, newValue);
                    LoadMedicinesGrid();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var sel = GetSelectedMedicine();
            if (sel == null)
            {
                MessageBox.Show("Select a medicine first.");
                return;
            }
            var ok = MessageBox.Show("Soft delete this medicine?", "Confirm", MessageBoxButtons.YesNo);
            if (ok == DialogResult.Yes)
            {
                // Log button click and delete action
                PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "CLICK", "Forms", "MedicineForm.btnDelete", "", "User clicked Delete Medicine button");
                PharmacyDBContext.AuditLogs.LogUserAction(PharmacyDBContext.CurrentUser?.UserID, "DELETE", "Medicines", sel.MedicineID.ToString(), $"Medicine: {sel.MedicineName}", "");
                sel.IsDeleted = true;
                sel.ModifiedDate = DateTime.Now;
                PharmacyDBContext.Medicines.Update(sel);
                LoadMedicinesGrid();
            }
        }
    }
}

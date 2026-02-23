using System;
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
            PharmacyDBContext.AuditLogs.LogUserAction(null, "VIEW", "Forms", "MedicineForm", "", "User accessed Medicine Management");
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
                    PharmacyDBContext.AuditLogs.LogUserAction(null, "CREATE", "Medicines", addedMedicine.MedicineID.ToString(), "", $"Medicine: {addedMedicine.MedicineName}");
                    LoadMedicinesGrid();
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(null, "CLICK", "Forms", "MedicineForm.btnAdd", "", "User clicked Add Medicine button");
            
            using (var form = new MedicineEditForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    var m = form.Result;
                    m.CreatedDate = DateTime.Now;
                    var addedMedicine = PharmacyDBContext.Medicines.Add(m);
                    PharmacyDBContext.AuditLogs.LogUserAction(null, "CREATE", "Medicines", addedMedicine.MedicineID.ToString(), "", $"Medicine: {addedMedicine.MedicineName}");
                    LoadMedicinesGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // Log button click
            PharmacyDBContext.AuditLogs.LogUserAction(null, "CLICK", "Forms", "MedicineForm.btnEdit", "", "User clicked Edit Medicine button");
            
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
                    PharmacyDBContext.AuditLogs.LogUserAction(null, "UPDATE", "Medicines", sel.MedicineID.ToString(), oldValue, newValue);
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
                PharmacyDBContext.AuditLogs.LogUserAction(null, "CLICK", "Forms", "MedicineForm.btnDelete", "", "User clicked Delete Medicine button");
                PharmacyDBContext.AuditLogs.LogUserAction(null, "DELETE", "Medicines", sel.MedicineID.ToString(), $"Medicine: {sel.MedicineName}", "");
                sel.IsDeleted = true;
                sel.ModifiedDate = DateTime.Now;
                PharmacyDBContext.Medicines.Update(sel);
                LoadMedicinesGrid();
            }
        }
    }
}

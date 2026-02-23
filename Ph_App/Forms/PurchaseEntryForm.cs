using System;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class PurchaseEntryForm : Form
    {
        public PurchaseEntryForm()
        {
            InitializeComponent();
            dtExpiryFrom.Value = DateTime.Now.Date;
            dtExpiryTo.Value = DateTime.Now.AddYears(2).Date;
            dtCreatedFrom.Value = DateTime.Now.AddYears(-10).Date;
            dtCreatedTo.Value = DateTime.Now.Date;
            ApplyMedicinesFilter();
        }

        private void ApplyMedicinesFilter()
        {
            var name = (txtFilterName?.Text ?? string.Empty).Trim().ToLowerInvariant();
            var expiryFrom = dtExpiryFrom.Value.Date;
            var expiryTo = dtExpiryTo.Value.Date;
            var createdFrom = dtCreatedFrom.Value.Date;
            var createdTo = dtCreatedTo.Value.Date;

            // Get all medicines from database
            var list = PharmacyDBContext.Medicines.GetAll().Where(m => !m.IsDeleted).ToList();
            
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(m => ((m.MedicineName ?? "").ToLowerInvariant().Contains(name) || (m.GenericName ?? "").ToLowerInvariant().Contains(name) || (m.Company ?? "").ToLowerInvariant().Contains(name) || (m.BatchNo ?? "").ToLowerInvariant().Contains(name))).ToList();
            }
            list = list.Where(m => !m.ExpiryDate.HasValue || (m.ExpiryDate.Value.Date >= expiryFrom && m.ExpiryDate.Value.Date <= expiryTo)).ToList();
            list = list.Where(m => m.CreatedDate.Date >= createdFrom && m.CreatedDate.Date <= createdTo).ToList();

            var data = list.Select(m => new
            {
                m.MedicineID,
                m.MedicineName,
                m.GenericName,
                m.BatchNo,
                CostPrice = m.PurchasePricePerPack,
                Quantity = m.CurrentStockPacks,
                TotalCost = m.PurchasePricePerPack * m.CurrentStockPacks,
                Expiry = m.ExpiryDate?.ToString("yyyy-MM-dd"),
                Created = m.CreatedDate.ToString("yyyy-MM-dd")
            }).ToList();

            dgv.DataSource = null;
            dgv.DataSource = data;
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyMedicinesFilter();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtFilterName.Text = string.Empty;
            dtExpiryFrom.Value = DateTime.Now.Date;
            dtExpiryTo.Value = DateTime.Now.AddYears(2).Date;
            dtCreatedFrom.Value = DateTime.Now.AddYears(-10).Date;
            dtCreatedTo.Value = DateTime.Now.Date;
            ApplyMedicinesFilter();
        }
    }
}

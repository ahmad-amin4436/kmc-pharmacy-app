using System;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Database;
using Ph_App.Models;

namespace Ph_App.Forms
{
    public partial class StockManagementForm : Form
    {
        public StockManagementForm()
        {
            InitializeComponent();
            dtExpiryFrom.Value = DateTime.Now.Date;
            dtExpiryTo.Value = DateTime.Now.AddYears(2).Date;
            dtCreatedFrom.Value = DateTime.Now.AddYears(-10).Date;
            dtCreatedTo.Value = DateTime.Now.Date;
            LoadMedicinesGrid();
        }

        private void LoadMedicinesGrid()
        {
            ApplyFilters();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtFilterName.Text = string.Empty;
            dtExpiryFrom.Value = DateTime.Now.AddYears(-10);
            dtExpiryTo.Value = DateTime.Now.AddYears(10);
            dtCreatedFrom.Value = DateTime.Now.AddYears(-10);
            dtCreatedTo.Value = DateTime.Now.AddYears(10);
            ApplyFilters();
        }

        private void ApplyFilters()
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
                list = list.Where(m => ((m.MedicineName ?? string.Empty).ToLowerInvariant().Contains(name) || (m.GenericName ?? string.Empty).ToLowerInvariant().Contains(name) || (m.Company ?? string.Empty).ToLowerInvariant().Contains(name) || (m.BatchNo ?? string.Empty).ToLowerInvariant().Contains(name))).ToList();
            }
            // filter by expiry between from and to if expiry has value
            list = list.Where(m => !m.ExpiryDate.HasValue || (m.ExpiryDate.Value.Date >= expiryFrom && m.ExpiryDate.Value.Date <= expiryTo)).ToList();
            // filter by created date range
            list = list.Where(m => m.CreatedDate.Date >= createdFrom && m.CreatedDate.Date <= createdTo).ToList();

            dgv.DataSource = null;
            dgv.DataSource = list.Select(m => new {
                m.MedicineID,
                m.MedicineName,
                m.GenericName,
                m.Company,
                m.BatchNo,
                Expiry = m.ExpiryDate?.ToString("yyyy-MM-dd"),
                m.PackQuantity,
                m.StripQuantity,
                m.CurrentStockPacks,
                m.CurrentStockStrips,
                m.CurrentStockTablets,
                m.MinimumStockLevel,
                Created = m.CreatedDate.ToString("yyyy-MM-dd")
            }).ToList();
        }
    }
}

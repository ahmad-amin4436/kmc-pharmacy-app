using System;
using System.Linq;
using System.Windows.Forms;
using Ph_App.Models;
using Ph_App.Database;
using Ph_App.DAL;

namespace Ph_App.Forms
{
    /// <summary>
    /// Base form that provides a medicine data grid and common helpers used by medicine-related forms.
    /// </summary>
    public class MedicineBaseForm : Form
    {
        protected DataGridView dgv;

        public MedicineBaseForm()
        {
            InitializeBaseGrid();
        }

        /// <summary>
        /// Initialize shared data grid for medicines.
        /// </summary>
        protected void InitializeBaseGrid()
        {
            this.dgv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 420,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            this.Controls.Add(dgv);
        }

        /// <summary>
        /// Load medicines into the base grid from database.
        /// </summary>
        protected void LoadMedicinesGrid()
        {
            if (dgv == null)
                InitializeBaseGrid();

            dgv.DataSource = null;

            var medicineRepo = PharmacyDBContext.Medicines;
            var list = medicineRepo.GetAll()
            .Where(m => !m.IsDeleted)
            .Select(m => new
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
            })
            .ToList();

            dgv.DataSource = list;
        }

        protected Medicine GetSelectedMedicine()
        {
            if (dgv == null || dgv.SelectedRows.Count == 0) return null;
            if (dgv == null || dgv.SelectedRows.Count == 0)
                return null;

            var id = Convert.ToInt32(dgv.SelectedRows[0].Cells["MedicineID"].Value);
            var medicineRepo = PharmacyDBContext.Medicines;
            return medicineRepo.GetById(id);
        }
    }
}

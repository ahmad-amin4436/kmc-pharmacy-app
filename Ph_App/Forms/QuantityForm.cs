using System;
using System.Windows.Forms;

namespace Ph_App.Forms
{
    public partial class QuantityForm : ResponsiveForm
    {
        public int Packs { get; private set; }
        public int Strips { get; private set; }
        public int Tablets { get; private set; }

        public QuantityForm(int defaultPacks = 0, int defaultStrips = 0, int defaultTablets = 0)
        {
            InitializeComponent();
            nudPacks.Value = defaultPacks;
            nudStrips.Value = defaultStrips;
            nudTablets.Value = defaultTablets;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Packs = (int)nudPacks.Value;
            Strips = (int)nudStrips.Value;
            Tablets = (int)nudTablets.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

namespace Ph_App.Forms
{
    partial class RolesRightsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox comboRoles;
        private System.Windows.Forms.CheckedListBox checkedListModules;
        private System.Windows.Forms.Button btnSave;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboRoles = new System.Windows.Forms.ComboBox();
            this.checkedListModules = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboRoles
            // 
            this.comboRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRoles.FormattingEnabled = true;
            this.comboRoles.Location = new System.Drawing.Point(12, 12);
            this.comboRoles.Name = "comboRoles";
            this.comboRoles.Size = new System.Drawing.Size(360, 21);
            this.comboRoles.TabIndex = 0;
            this.comboRoles.SelectedIndexChanged += new System.EventHandler(this.comboRoles_SelectedIndexChanged);
            // 
            // checkedListModules
            // 
            this.checkedListModules.CheckOnClick = true;
            this.checkedListModules.FormattingEnabled = true;
            this.checkedListModules.Location = new System.Drawing.Point(12, 45);
            this.checkedListModules.Name = "checkedListModules";
            this.checkedListModules.Size = new System.Drawing.Size(360, 199);
            this.checkedListModules.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(297, 260);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // RolesRightsForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 295);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.checkedListModules);
            this.Controls.Add(this.comboRoles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RolesRightsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Roles & Rights";
            this.ResumeLayout(false);

        }

        #endregion
    }
}

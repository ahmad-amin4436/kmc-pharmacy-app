using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ph_App.Utils
{
 public static class InputDialog
 {
 public static string Show(string prompt, string title, string defaultValue = "")
 {
 using (var form = new Form())
 {
 form.Width =400;
 form.Height =150;
 form.Text = title;
 form.StartPosition = FormStartPosition.CenterParent;
 var lbl = new Label() { Left =10, Top =10, Text = prompt, Width =360 };
 var txt = new TextBox() { Left =10, Top =35, Width =360, Text = defaultValue };
 var btnOk = new Button() { Text = "OK", Left =200, Width =80, Top =70, DialogResult = DialogResult.OK };
 var btnCancel = new Button() { Text = "Cancel", Left =290, Width =80, Top =70, DialogResult = DialogResult.Cancel };
 form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
 form.AcceptButton = btnOk;
 form.CancelButton = btnCancel;
 var dr = form.ShowDialog();
 return dr == DialogResult.OK ? txt.Text : null;
 }
 }
 }
}

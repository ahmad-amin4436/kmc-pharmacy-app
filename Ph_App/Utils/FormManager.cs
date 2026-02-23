using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ph_App.Utils
{
    public static class FormManager
    {
        private static readonly Dictionary<Type, Form> _openForms = new Dictionary<Type, Form>();
        private static readonly object _lock = new object();

        public static T ShowForm<T>() where T : Form, new()
        {
            lock (_lock)
            {
                var formType = typeof(T);
                
                if (_openForms.ContainsKey(formType))
                {
                    var existingForm = _openForms[formType];
                    if (!existingForm.IsDisposed)
                    {
                        existingForm.BringToFront();
                        existingForm.WindowState = FormWindowState.Normal;
                        return (T)existingForm;
                    }
                    else
                    {
                        _openForms.Remove(formType);
                    }
                }

                var newForm = new T();
                newForm.FormClosed += (s, e) => {
                    lock (_lock)
                    {
                        _openForms.Remove(formType);
                    }
                };
                
                _openForms[formType] = newForm;
                newForm.Show();
                return newForm;
            }
        }

        public static void CloseForm<T>() where T : Form
        {
            lock (_lock)
            {
                var formType = typeof(T);
                if (_openForms.ContainsKey(formType))
                {
                    var form = _openForms[formType];
                    if (!form.IsDisposed)
                    {
                        form.Close();
                    }
                    _openForms.Remove(formType);
                }
            }
        }

        public static void CloseAllForms()
        {
            lock (_lock)
            {
                var formsToClose = new List<Form>(_openForms.Values);
                foreach (var form in formsToClose)
                {
                    if (!form.IsDisposed)
                    {
                        form.Close();
                    }
                }
                _openForms.Clear();
            }
        }

        public static bool IsFormOpen<T>() where T : Form
        {
            lock (_lock)
            {
                var formType = typeof(T);
                return _openForms.ContainsKey(formType) && !_openForms[formType].IsDisposed;
            }
        }

        public static T GetOpenForm<T>() where T : Form
        {
            lock (_lock)
            {
                var formType = typeof(T);
                if (_openForms.ContainsKey(formType) && !_openForms[formType].IsDisposed)
                {
                    return (T)_openForms[formType];
                }
                return null;
            }
        }

        public static void ShowFormDialog<T>() where T : Form, new()
        {
            using (var form = new T())
            {
                form.ShowDialog();
            }
        }
    }
}

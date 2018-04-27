// Stephen Toub

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PreviewHandlerEditor
{
    public partial class MainForm : Form
    {
        private bool _loading = false;
        private RegistrationData _regData;

        public MainForm() { InitializeComponent(); }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            _loading = true;
            handlerComboBox.Enabled = false;
            extensionsListBox.Enabled = false;

            ThreadPool.QueueUserWorkItem(delegate
            {
                _regData = LoadRegistrationInformation();
                Invoke((MethodInvoker)delegate
                {
                    handlerComboBox.Items.AddRange(_regData.Handlers.ToArray());
                    extensionsListBox.Enabled = true;
                    handlerComboBox.Enabled = true;
                    _loading = false;
                    UseWaitCursor = false;
                });
            }, null);
        }

        private void handlerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading) ConfigureExtensionsList();
        }

        private void ConfigureExtensionsList()
        {
            PreviewHandlerInfo handler = handlerComboBox.SelectedItem as PreviewHandlerInfo;
            if (handler != null)
            {
                UseWaitCursor = true;
                _loading = true;
                handlerComboBox.Enabled = false;
                extensionsListBox.Enabled = false;

                extensionsListBox.SuspendLayout();
                extensionsListBox.Items.Clear();
                foreach (ExtensionInfo info in _regData.Extensions)
                {
                    bool isChecked = info.Handler == handler;
                    if (isChecked || !showOnlyRegisteredCheckBox.Checked)
                    {
                        extensionsListBox.Items.Add(info, isChecked);
                    }
                }
                extensionsListBox.ResumeLayout();

                extensionsListBox.Enabled = true;
                handlerComboBox.Enabled = true;
                _loading = false;
                UseWaitCursor = false;
            }
        }

        private RegistrationData LoadRegistrationInformation()
        {
            // Load and sort all preview handler information from registry
            List<PreviewHandlerInfo> handlers = new List<PreviewHandlerInfo>();
            using (RegistryKey handlersKey = Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", true))
            {
                foreach (string id in handlersKey.GetValueNames())
                {
                    PreviewHandlerInfo handler = new PreviewHandlerInfo();
                    handler.ID = id;
                    handler.Name = handlersKey.GetValue(id, null) as string;
                    handlers.Add(handler);
                }
            }
            handlers.Sort(delegate(PreviewHandlerInfo first, PreviewHandlerInfo second)
            {
                if (first.Name == null) return 1;
                else if (second.Name == null) return -1;
                else return first.Name.CompareTo(second.Name);
            });

            // Create a lookup table of preview handler ID -> PreviewHandlerInfo
            Dictionary<string, PreviewHandlerInfo> handlerMapping = new Dictionary<string, PreviewHandlerInfo>(handlers.Count);
            foreach (PreviewHandlerInfo handler in handlers)
            {
                handlerMapping.Add(handler.ID, handler);
            }

            // Get all classes/extensions from registry
            string[] extensions = Registry.ClassesRoot.GetSubKeyNames();

            // Find out what each extension is registered to be previewed with
            List<ExtensionInfo> extensionInfos = new List<ExtensionInfo>(extensions.Length);
            foreach (string extension in extensions)
            {
                ExtensionInfo info = new ExtensionInfo();
                info.Extension = extension;

                string id = Registry.GetValue(
                    string.Format(@"HKEY_CLASSES_ROOT\{0}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", extension),
                    null, null) as string;
                PreviewHandlerInfo mappedHandler;
                if (id != null && handlerMapping.TryGetValue(id, out mappedHandler)) info.Handler = mappedHandler;

                extensionInfos.Add(info);
            }

            // Return the information
            RegistrationData data = new RegistrationData();
            data.Handlers = handlers;
            data.Extensions = extensionInfos;
            return data;
        }

        private static void RegisterHandlerForExtension(string extension, PreviewHandlerInfo handler)
        {
            using (RegistryKey extensionKey = Registry.ClassesRoot.CreateSubKey(extension))
            using (RegistryKey shellexKey = extensionKey.CreateSubKey("shellex"))
            using (RegistryKey previewKey = shellexKey.CreateSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}"))
            {
                previewKey.SetValue(null, handler.ID, RegistryValueKind.String);
            }
        }

        private static void UnregisterHandlerForExtension(string extension)
        {
            using (RegistryKey extensionKey = Registry.ClassesRoot.OpenSubKey(extension, true))
            {
                if (extensionKey != null)
                {
                    using (RegistryKey shellexKey = extensionKey.OpenSubKey("shellex", true))
                    {
                        if (shellexKey != null)
                        {
                            shellexKey.DeleteSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}");
                        }
                    }
                }
            }
        }

        private void extensionsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_loading)
            {
                if (handlerComboBox.SelectedItem != null)
                {
                    PreviewHandlerInfo handlerInfo = handlerComboBox.SelectedItem as PreviewHandlerInfo;
                    ExtensionInfo extensionInfo = (ExtensionInfo)extensionsListBox.Items[e.Index];
                    if (e.CurrentValue == CheckState.Unchecked)
                    {
                        RegisterHandlerForExtension(extensionInfo.Extension, handlerInfo);
                        extensionInfo.Handler = handlerInfo;
                    }
                    else
                    {
                        UnregisterHandlerForExtension(extensionInfo.Extension);
                        extensionInfo.Handler = null;
                    }
                }
            }
        }

        private string _searchString = string.Empty;

        private void extensionsListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
            {
                _searchString += e.KeyChar;
                keyTimer.Stop();
                keyTimer.Start();

                for (int i = 0; i < extensionsListBox.Items.Count; i++)
                {
                    if (((ExtensionInfo)extensionsListBox.Items[i]).Extension.StartsWith(_searchString))
                    {
                        extensionsListBox.SelectedIndex = i;
                        break;
                    }
                }
                e.Handled = true;
            }
        }

        private void keyTimer_Tick(object sender, EventArgs e)
        {
            _searchString = string.Empty;
            keyTimer.Stop();
        }

        private void showOnlyRegisteredCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ConfigureExtensionsList();
        }

        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
            using(AboutBox box = new AboutBox()) box.ShowDialog(this);
        }

        private class RegistrationData
        {
            public List<PreviewHandlerInfo> Handlers;
            public List<ExtensionInfo> Extensions;
        }

        private class PreviewHandlerInfo
        {
            public string Name;
            public string ID;
            public override string ToString()
            {
                if (string.IsNullOrEmpty(Name)) return ID;
                else return Name;
            }
        }

        private class ExtensionInfo
        {
            public string Extension;
            public PreviewHandlerInfo Handler;
            public override string ToString() { return Extension; }
        }
    }
}
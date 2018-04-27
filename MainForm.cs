// Stephen Toub

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
                    comboBox1.Items.AddRange(_regData.Handlers.ToArray());
                    extensionsListBox.Enabled = true;
                    handlerComboBox.Enabled = true;
                    comboBox1.Enabled = false;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
          var  handler = handlerComboBox.SelectedItem as PreviewHandlerInfo;
          var extension = new ExtensionInfo() {
              Extension = txtExtension.Text,
              Handler = handler
          };

          using (var extensionKey = Registry.ClassesRoot.CreateSubKey(txtExtension.Text))
            if (handler != null)
            {
              using (var shellexKey = extensionKey.CreateSubKey("shellex"))
                using (var previewKey = shellexKey.CreateSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}"))
                {
                    previewKey.SetValue(null, handler.ID, RegistryValueKind.String);
                        slStatus.Text = "Key for extension created, and linked to Handler: " + handler.Name ;
                    }
                }
            slStatus.Text = "Key for extension created (without linked PV-handler)"; 
        }

        private void txtExtension_TextChanged(object sender, EventArgs e)
        {
            slStatus.Text = "";
            comboBox1.Enabled = txtExtension.Text.Trim().Length > 0;
            lblDirectExtensionInfo.Text = getExtensionInfoText(txtExtension.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtExtension.Text.Trim() != "") {
                RegisterHandlerForExtension(txtExtension.Text, ((ComboBox)sender).SelectedItem as PreviewHandlerInfo);
                slStatus.Text = $"Linked handler {comboBox1.SelectedText} to extension {txtExtension.Text}";
            }
        }

        private string getExtensionInfoText(string extension)
        {
            var regLinks = new List<string>();

            /*
             Possible scenarios:
               - 1. Extension does not exist
               - 2. Extension exists, <yes/no> Parent, Bound to own PVH.
               - 2. Extension exists, No parent, Unbound
               - 3. Extension exists, Has Parent, Unbound
               - 4. Extension exists, Has Parent, Bound via Parent                              
             */
            if (!Registry.ClassesRoot.GetSubKeyNames().Any((skn) => skn.Equals(extension)))
            {
                // Extension does not exist:
                llExtension.Enabled = false;
                llHandler.Enabled = false;
                llParent.Enabled = false;
                llParentHandler.Enabled = false;

                return $"Extension {extension} not found...";
            }
            else
            {
                var extKey = Registry.ClassesRoot.OpenSubKey(extension);
                var sbRet = new StringBuilder();
                sbRet.Append($"Extension {extension} exists, ");
                llExtension.Text = $@"HKEY_CLASSES_ROOT\{ extension}";
                llExtension.Links.Clear();
                llExtension.Links.Add(0,llExtension.Text.Length, $@"reg://HKEY_CLASSES_ROOT\{ extension}");
                llExtension.Enabled = true;

                var pvHandlerName = "";
                var pvHandlerGuidValue = handlerGuidFromExtension(extension, false);

                if (pvHandlerGuidValue == null)
                {
                    var parentName = labelAndParentExtensionNameFromExtensionName(extension).Item1;
                    if (parentName != null && parentName.Length>0)
                    {
                        sbRet.Append($"has Parent {parentName}, ");

                        llParent.Text = $@"HKEY_CLASSES_ROOT\{parentName}";
                        llParent.Links.Clear();
                        llParent.Links.Add(0, llParent.Text.Length, $@"reg://HKEY_CLASSES_ROOT\{parentName}");
                        llParent.Enabled = true;
                        
                        // Has Valid Parent, No (own) Handler
                        pvHandlerGuidValue = handlerGuidFromExtension(parentName, false);
                        if (pvHandlerGuidValue == null)
                        {
                            // Parent, no own handler, nor Parent Handler
                            sbRet.Append($"and no handler exists for Parent nor Extension itself");
                        }
                        else
                        {
                            // Parent, no (own) handler, but some GUID as Parent HAndler
                            pvHandlerName = handlerNameFromGuid(pvHandlerGuidValue);
                            if (pvHandlerName == null)
                            {
                                // Parent has some HAndler-GUID, but invalid:
                                sbRet.Append($"which is bound to an (invalid?) Handler-GUID: {pvHandlerGuidValue}");

                            }
                            else
                            {
                                // Parent had valid Handler: 
                                sbRet.Append($"which is bound to registered Handler: {pvHandlerName}");
                            }
                        }
                    }
                    else
                    {
                        // No Parent, No Handler
                        sbRet.Append("has no Parent - Link, nor Handler");
                    }
                }
                else
                {
                    // regLinks.Add($@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}");

                    llHandler.Links.Clear();  
                    llHandler.Links.Add(0,llHandler.Text.Length, $@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}");
                    llHandler.Enabled = true;
                    pvHandlerName = handlerNameFromGuid(pvHandlerGuidValue);
                    if (pvHandlerName == null)
                    {
                        // Handler, but invalid
                        sbRet.Append($"is bound to an (invalid?) handler with GUID: {pvHandlerGuidValue}");
                    }
                    else
                    {
                        // Named Handler
                        sbRet.Append($"and is handled by Handler: {pvHandlerName}");
                    }
                }

                //Registry.GetValue($@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", "", "") as string;
                //if (pvHandlerGuidValue == null || pvHandlerGuidValue == "") {
                //    var extensionParent = Registry.GetValue($@"HKEY_CLASSES_ROOT\{extension}", "", "") as string;
                //    if (extensionParent != null && extensionParent.Trim().Length > 0) {
                //        pvHandlerGuidValue = Registry.GetValue($@"HKEY_CLASSES_ROOT\{extensionParent}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", "", "") as string;
                //        if (pvHandlerGuidValue == null || pvHandlerGuidValue == "")
                //        {
                //            return $"Extension {extension} exists, has Parent {extensionParent}, and is NOT bound to a PV-Handler";
                //        }
                //        else {
                //            var pvhName = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", pvHandlerGuidValue, "") as string;
                //            if (pvhName == null || pvhName == "")
                //            { return $"Extension {extension} exists, has Parent {extensionParent}, and is bound to an Unknown / Erroneous PV-Handler"; }
                //            else
                //            {
                //                return $"Extension {extension} exists, has Parent {extensionParent}, and is bound to PV-Handler {pvhName}";
                //            }
                //        }
                //    }
                //    //if (   ) { } else {
                //        return $"Extension {extension} exists, and has no PV-handler"; }
                //}
                //else {
                //    var pvhName = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", pvHAndlerGuidValue, "") as string;
                //    return $"Extension {extension} exists and handled by: " + pvhName;
                //}
                return sbRet.ToString(); 
            }             
        }

        private string handlerNameFromGuid(string handlerGuid) {
            var ret = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", handlerGuid, "") as string;
            if (ret == "" || ret == null)
                return null;
            return ret;
        }

        private string handlerGuidFromExtension(string ext, bool bRecurseParents) {
            var ret = Registry.GetValue($@"HKEY_CLASSES_ROOT\{ext}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", "", "") as string;
            if (ret == "" || ret == null)
                return null;
            return ret;
        }

        private Tuple<string, string> labelAndParentExtensionNameFromExtensionName(string ext) {
            var ret = Tuple.Create<string, string>("", "");

            // Is a Name present:
            ret = Tuple.Create(Registry.GetValue($@"HKEY_CLASSES_ROOT\{ext}", "", "") as string, ret.Item2);

            // Ret = Parent as labeled @ Extension. ==> Needs to be under HKCR as extension. If this is true, Parent is valid.


            try
            {
                var parentKey = Registry.ClassesRoot.OpenSubKey(ret.Item1, false);
                var parentLabel = parentKey.GetValue($@"HKEY_CLASSES_ROOT\{ret.Item1}", "") as string;
                // If parentLabel does not exist, Parent can still be valid. (since it has its entry under HKCR);


                // Is this name a valid extension itself?
                ret = Tuple.Create(ret.Item1, parentLabel);

                if (parentKey.Handle == null)
                    return null;
                return ret;
            }catch (Exception ex) {
                return null;
            }
        }

        private void ll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start( "c:\\windows\\system32\\cmd.exe", " /c start " +    e.Link.LinkData); 
        }
    }
    }

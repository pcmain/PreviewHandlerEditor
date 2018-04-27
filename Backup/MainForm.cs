// Stephen Toub

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Linq;

namespace PreviewHandlerEditor
{
    public partial class MainForm : Form
    {
        private bool _loading = false;
        private RegistrationData _regData = Program.AllRegData;
        //private static string[] _restrictToExtensionsList = null;

        public bool ShowOnlyRegistered {
            get {
                return showOnlyRegisteredCheckBox.Checked; }
            set {
                if (_loading) return;
                if (showOnlyRegisteredCheckBox.Checked != value)
                    showOnlyRegisteredCheckBox.Checked = value;
            }
        }

        //public static string[] RestrictToExtensionsList {
        //    get {
        //        return _restrictToExtensionsList;
        //    }
        //    set {
        //        _restrictToExtensionsList = value;
        //        //_regData = LoadRegistrationInformation(loadExtensionsRegistrationInformation(null, _restrictToExtensionsList));
        //    }
        //}

        public MainForm() {
            _loading = true;
            InitializeComponent();
            _loading = false;
        }

            // With files passed as Parameters, we do two things:
            // 1. Register the extension, prompting the user for an (optional) description / filetype
            // 2. Register a selectable Preview-Hanlder to the Extension. When more files where passed, with 
            //    different extensions, we'll set the (same) PreviewHandler to each one of them.

        private void MainForm_Load(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            _loading = true;
            handlerComboBox.Enabled = false;
            extensionsListBox.Enabled = false;

            ThreadPool.QueueUserWorkItem(delegate
            {
                _regData = Program.AllRegData;//LoadRegistrationInformation();
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
            if (!_loading)
                ConfigureExtensionsList();
        }

        private void ConfigureExtensionsList()
        {
            var handler = handlerComboBox.SelectedItem as PreviewHandlerInfo;

            if ((handler != null) || (handler == null && !ShowOnlyRegistered))
            {
                UseWaitCursor = true;
                _loading = true;
                handlerComboBox.Enabled = false;    
                extensionsListBox.Enabled = false;

                extensionsListBox.SuspendLayout();
                extensionsListBox.Items.Clear();

               // if (_regData == null) {
               //     _regData = LoadRegistrationInformation();
               // }
               foreach (var info in (_regData??Program.AllRegData).Extensions)
                    {
                        bool isChecked = (info.Handler!=PreviewHandlerInfo.Unspecified) && info.Handler == handler;
                        if (isChecked || !ShowOnlyRegistered)
                            extensionsListBox.Items.Add(info, isChecked);
                    }                
                extensionsListBox.ResumeLayout();

                extensionsListBox.Enabled = true;
                handlerComboBox.Enabled = true;
                _loading = false;
                UseWaitCursor = false;
            }
        }

        //private RegistrationData LoadRegistrationInformation()
        //{
        //    // Load and sort all preview handler information from registry
        //    var handlers = new List<PreviewHandlerInfo>() {
        //        PreviewHandlerInfo.Unspecified
        //    };

        //    using (RegistryKey handlersKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", false ))
        //    {
        //        foreach (var id in handlersKey.GetValueNames())
        //        {
        //            var handler = new PreviewHandlerInfo{
        //                ID = id,
        //                Name = handlersKey.GetValue(id, null) as string
        //            };
        //            handlers.Add(handler);
        //        }
        //    }

        //    handlers.Sort(delegate(PreviewHandlerInfo first, PreviewHandlerInfo second)
        //    {
        //        // Unspecified always to Top:
        //        if (first.ID == "-") return -1;
        //        if (second.ID == "-") return 1;

        //        if (first.Name == null) return 1;
        //        else if (second.Name == null) return -1;
        //        else return first.Name.CompareTo(second.Name);
        //    });

        //    // Create a lookup table of preview handler ID -> PreviewHandlerInfo
        //    var handlerMapping = new Dictionary<string, PreviewHandlerInfo>(handlers.Count);
        //    foreach (var handler in handlers)
        //        handlerMapping.Add(handler.ID, handler);

        //    // Get all classes/extensions from registry
        //    var extensions = Registry.ClassesRoot.GetSubKeyNames();

        //    if (_restrictToExtensionsList != null && _restrictToExtensionsList.Length>0)
        //    {
        //        // Limit list to only these from param:
        //        extensions = extensions.Where((e) => _restrictToExtensionsList.Any((re) => re.ToLowerInvariant().Equals(e.ToLowerInvariant()))).ToArray();
        //    }

        //    //// Find out what each extension is registered to be previewed with
        //    var extensionInfos = new List<ExtensionInfo>(extensions.Length);
        //    foreach (var extension in extensions)
        //    {
        //        var info = new ExtensionInfo
        //        {
        //            Extension = extension
        //        };

        //        var id = Registry.GetValue($@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string;
        //        if (id != null && handlerMapping.TryGetValue(id, out PreviewHandlerInfo mappedHandler))
        //            info.Handler = mappedHandler;

        //        extensionInfos.Add(info);
        //    }
        //    // var extensionInfos = loadExtensionsRegistrationInformation(handlerMapping);

        //    // Return the information
        //    var data = new RegistrationData()
        //    {
        //        Handlers = handlers,
        //        Extensions = extensionInfos
        //    };

        //    return data;
        //}

        private List<ExtensionInfo> loadExtensionsRegistrationInformation(Dictionary<string,PreviewHandlerInfo> handlerMapping = null, string[] limitToExtensions = null)  {

            // Get all classes/extensions from registry
            var extensions = Registry.ClassesRoot.GetSubKeyNames();

            if (limitToExtensions != null) {
                // Limit list to only these from param:
                extensions = extensions.Where((e) => limitToExtensions.Any((le) => le.ToLowerInvariant().Equals(e.ToLowerInvariant()))).ToArray();
            }

            // Find out what each extension is registered to be previewed with
            var extensionInfos = new List<ExtensionInfo>(extensions.Length);
            foreach (var extension in extensions)
            {
                var info = new ExtensionInfo{
                    Extension = extension
                };

                // Check for Handler:
                var id = Registry.GetValue($@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string;
                if (handlerMapping != null)
                    if (id != null && handlerMapping.TryGetValue(id, out PreviewHandlerInfo mappedHandler))
                        if(limitToExtensions!=null)     // If LimitToExtensions is specified, this is for addind a common handler to all of them. Therefore, we'll ignore existing handler.
                            info.Handler = mappedHandler;
                
                extensionInfos.Add(info);
            }
            return extensionInfos;
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
    }
}
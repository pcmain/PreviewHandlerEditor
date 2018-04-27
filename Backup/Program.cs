using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Security.Principal;

namespace PreviewHandlerEditor
{
    static class Program
    {
        public enum RegKeys {
            None=0,
            Extensions=1,
            PVHandlers = 2,            
        }

        public static Dictionary<RegKeys, RegistryKey> RegKeysDict = new Dictionary<RegKeys, RegistryKey> {
            { RegKeys.Extensions, Registry.ClassesRoot},
            { RegKeys.PVHandlers, Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", false)}
        };

        public static RegistrationData AllRegData = null;
        public static List<string> ArgsExtensions = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            if (args != null) {
                chkUserIsAdmin("To run PreviewHandlerEditor.exe with arguments, elevation is needed.");
                ensureFileExtensions(args);
            }

            ArgsExtensions = args.Select((p) => Path.GetExtension(p).ToLowerInvariant()).ToList();

            AllRegData = new RegistrationData()
            {
                Extensions = RegKeysDict[RegKeys.Extensions].GetSubKeyNames()
                    .Where ((skn)=>ArgsExtensions==null || ArgsExtensions.Count==0 || ArgsExtensions.Any((a)=>a.Equals(skn.ToLowerInvariant())))
                    .Select((skn) => new ExtensionInfo(skn)).ToList(),

                Handlers = RegKeysDict[RegKeys.PVHandlers].GetValueNames().Select((vn) =>
                    new PreviewHandlerInfo
                    {
                        ID = vn,
                        Name = RegKeysDict[RegKeys.PVHandlers].GetValue(vn, null) as string
                    })
                    .Union(new PreviewHandlerInfo[] {
                        PreviewHandlerInfo.Unspecified
                    }).ToList()
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //MainForm.RestrictToExtensionsList = args;
            Application.Run(new MainForm() {
                //RestrictToExtensionsList = args,
                ShowOnlyRegistered = (args==null)                
            });
        }

        /// <summary>
        /// Given a number of Files, makes sure that each file's extension exists under HKCR.
        /// </summary>
        /// <param name="filePaths"></param>
        private static void ensureFileExtensions(string[] filePaths)
        {
            if (filePaths == null)
                return;

            // Distinct paths and distinct extension:
            foreach (var ext in filePaths.Distinct().Where((p) => Path.HasExtension(p)).Select((p) => Path.GetExtension(p)).OrderBy((p) => p).Distinct())
                ensureExtension(ext);
        }

        /// <summary>
        /// Given an extension, makes sure that extension exists under HKCR
        /// </summary>
        /// <param name="singleExtension"></param>
        private static void ensureExtension(string singleExtension)
        {
            if (string.IsNullOrEmpty(singleExtension))
                return;

            // Extension includes the dot "."
            if (!singleExtension.StartsWith(".")) singleExtension = "." + singleExtension;

            MessageBox.Show(singleExtension);
            
            using (RegistryKey extensionsKey = Registry.ClassesRoot.OpenSubKey("",true))
            {
                if (!extensionsKey.GetSubKeyNames().Any((k) => k.ToLowerInvariant().Equals(singleExtension.ToLowerInvariant())))
                    MessageBox.Show(extensionsKey.CreateSubKey($@"{singleExtension.ToLowerInvariant()}\shellEx", true).Name); 

                var test = extensionsKey.GetSubKeyNames().First((k) => k.ToLowerInvariant().Equals(singleExtension.ToLowerInvariant()));
                if (test == null)
                    throw new KeyNotFoundException($"Failed to Ensure extension {singleExtension}");
            }
        }

        private static void chkUserIsAdmin(string msgOnFail = "") {
            var bRet = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (!bRet)
                throw new UnauthorizedAccessException($"User No Admin!\n{msgOnFail}");
        }
        
    }
}
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHE2
{    
    static class Program
    {
        public static PreviewHandlersDictionary PreviewHandlers;
        public static ExtensionsDictionary Extensions;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            if (args != null && args.Length > 0)
            {
                Form1.ArgsExtensions = args.Select<string, string>((a) => Path.GetExtension(a).ToLowerInvariant()).Distinct().ToList();
            }

            if (PreviewHandlers == null) {PreviewHandlers = new PreviewHandlersDictionary();}
            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers").GetValueNames()
                .Select((vn) => new PreviewHandlerInfo(vn) {
                    Name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", vn, null) as string }
                ).ToList()
                .ForEach((pvh) => {
                    if(!PreviewHandlers.ContainsKey(pvh.GUID))
                        PreviewHandlers.Add(pvh.GUID, pvh);
                });
            Console.WriteLine($"Added {PreviewHandlers.Count} PVHs");

            if (Extensions == null)
                Extensions = new ExtensionsDictionary();

            Registry.ClassesRoot.GetSubKeyNames()
                .Select((skn) => new ExtensionInfo(skn))
                .ToList()
                .ForEach((e) => {
                    Extensions.Add(e.Ext, e);
                });
            Console.WriteLine($"Added {ExtensionsDictionary.ExtList.Count} Extensions");
            Extensions[".bat"].Load(); 
            Console.WriteLine($"Bat hasAlias: {Extensions[".bat"].HasAlias} ");
            Console.WriteLine($"Bat PHV: {Extensions[".bat"].PreviewHandlerGuid} ");
            if (Extensions[".bat"].PreviewHandlerGuid != null)
            {
                Console.WriteLine($"Bat PHV: {Extensions[".bat"].PreviewHandler.Name} ");
            }

            Console.WriteLine($"Added {Extensions.Count((e) => e.Value.Ext.StartsWith("."))} Dotted Extensions ");


            Console.WriteLine($"NoAalias {ExtensionsDictionary.HasNoAliasExtList.Count} ");

            //            Extensions.Where((e) => e.Value.HasPhv).ToList().ForEach((e)=>{ Console.WriteLine(e.ToString()); });

            Console.WriteLine("Coutning done");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1() { });
        }
    }
}

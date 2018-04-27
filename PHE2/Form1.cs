using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHE2
{
   
    public partial class Form1 : Form
    {
        public static Dictionary<string, TreeNode> WKTreeNodes = new Dictionary<string, TreeNode>();
        public static List<string> ArgsExtensions;
        private Dictionary<string, string> _allExtensions = null;
        private Dictionary<string, string> _dotExtensions = null;

        public DataTable DT_Extension = new DataTable("DT_Extension");
        public DataTable DT_Group = new DataTable();
        public DataTable DT_Handler= new DataTable();



        public Dictionary<string, string> AllExtensions
        {
            get
            {
                if (_allExtensions == null)
                {
                    _allExtensions = GetAllExtensions();
                }
                return _allExtensions;
            }
        }

        public Dictionary<string, string> DotExtensions
        {
            get
            {
                if (_dotExtensions == null)
                {
                    _dotExtensions = new Dictionary<string, string>();
                    GetAllExtensions().Where((e) => e.Key.StartsWith(".")).Select((e) => e.Key).ToList().ForEach((e) => {
                        _dotExtensions.Add(e, AllExtensions[e]);
                    });
                }
                return _dotExtensions;
            }
        }

        public Dictionary<string, string> TheExtensions = null;

        public Form1()
        {
            TheExtensions = DotExtensions;
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DT_Handler.Columns.AddRange(new DataColumn[] {
                    new DataColumn("Id", typeof(int)){ AutoIncrement=true, AutoIncrementSeed=1, AutoIncrementStep=1, Unique=true,AllowDBNull=false},
                    new DataColumn("GUID", typeof(string)){ MaxLength=128, Unique=true, AllowDBNull=false},
                    new DataColumn("Name", typeof(string)){ MaxLength=128, Unique=true, AllowDBNull=false}
            });

            DT_Group.Columns.AddRange(new DataColumn[] {
                    new DataColumn("Id", typeof(int)){ AutoIncrement=true, AutoIncrementSeed=1, AutoIncrementStep=1, Unique=true,AllowDBNull=false},
                    new DataColumn("Ext", typeof(string)){ MaxLength=128, Unique=true, AllowDBNull=false},
                    new DataColumn("Name", typeof(string)){ MaxLength=128, Unique=true, AllowDBNull=false},
                    new DataColumn("HandlerId", typeof(int)){AllowDBNull=true},
                    new DataColumn("GroupId", typeof(int)){ AllowDBNull=true}
            });
            
            DT_Extension.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("Id", typeof(int)){ AutoIncrement=true, AutoIncrementSeed=1, AutoIncrementStep=1, Unique=true,AllowDBNull=false},
                    new DataColumn("Ext", typeof(string)){ MaxLength=128, Unique=true, AllowDBNull=false},
                    new DataColumn("Name", typeof(string)){ MaxLength=128, AllowDBNull=true},
                    new DataColumn("IsDotted", typeof(bool)){ DefaultValue=false},
                    new DataColumn("HandlerId", typeof(int)){AllowDBNull=true},
                    new DataColumn("GroupId", typeof(int)){ AllowDBNull=true}
                });

            DT_Extension.PrimaryKey = new DataColumn[] { DT_Extension.Columns["Id"] };

            //Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", false).GetValueNames().Select((vn) => new
            //{
            //    GUID = vn,
            //    Name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", vn, null) as string
            //}).Where((h)=>h.Name).ToList()
            //.ForEach((h) => {
            //    var drh = DT_Handler.NewRow();
            //    drh["GUID"] = h.GUID;
            //    drh["Name"] = h.Name;
            //    DT_Handler.Rows.Add(drh);
            //});
            //DT_Handler.AcceptChanges();

            //Registry.ClassesRoot.GetSubKeyNames().Select((skn) => 
            //{
            //    GUID = vn,
            //    Name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", vn, null) as string
            //}).ToList().ForEach((h) => {
            //    var drh = DT_Handler.NewRow();
            //    drh["GUID"] = h.GUID;
            //    drh["Name"] = h.Name;
            //    DT_Handler.Rows.Add(drh);
            //});
            //DT_Handler.AcceptChanges();

            //foreach (string kn in Registry.ClassesRoot.GetSubKeyNames()){
            //    var rk = Registry.ClassesRoot.OpenSubKey(kn, false);
            //    DataRow drNew = DT_Extension.NewRow();
            //    drNew["Ext"] = kn;
            //    drNew["Name"] = rk.GetValue("@") as string;
            //    drNew["IsDotted"] = kn.StartsWith(".");
            //    drNew["HandlerId"] = DT_Handler.Rows.Cast<DataRow>().Where((dr) => dr["GUID"].Equals(Registry.GetValue($@"HKEY_CLASSES_ROOT\{e}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string)).First()["Id"];
            //    DT_Handler.Select("[GUID]=''");
            //}; 

            WKTreeNodes.Add("WH", tvMain.Nodes.Add("WithHandler", "With Handler"));

            WKTreeNodes.Add("WnoH", tvMain.Nodes.Add("WithoutHandler", "Without Handler"));

            WKTreeNodes.Add("NE", tvMain.Nodes.Add("NewExtensions", "New Extensions"));

            WKTreeNodes.Add("A", tvMain.Nodes.Add("ExtensionsFromArgs", "Extensions from Arguments"));
            if (ArgsExtensions != null)
                WKTreeNodes["A"].Nodes.AddRange(ArgsExtensions.Select((ae) => new TreeNode {
                    Text = ae,
                    Name = ae
                }).ToArray());

            WKTreeNodes.Add("CFDE", tvMain.Nodes.Add("ClassFulDotExtensions", "Classful Dot Extensions"));

            GetHandlers();
            GetNoHandled();
            GetClassFulDots();
        }

        public void GetHandlers()
        {
            tvMain.Nodes["WithHandler"].Nodes.AddRange(
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", false).GetValueNames().Select((vn) =>
                     new TreeNode
                     {
                         Name = vn,
                         Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers", vn, null) as string
                     })
                     .Select((tn) =>
                     {
                         tn.Nodes.AddRange(
                             TheExtensions.Where((kvp) => (kvp.Value ?? "").ToLowerInvariant().Equals(tn.Name.ToLowerInvariant())).Cast<KeyValuePair<string, string>>()
                             .Select((kvp) => { return new TreeNode {
                                 Name = kvp.Key,
                                 Text = kvp.Key
                             };
                             }).ToArray());
                         return tn;
                     }).ToArray());
        }

        public void GetNoHandled()
        {
            tvMain.Nodes["WithoutHandler"].Nodes.AddRange(

                // With no ShellEx - SubKey
                TheExtensions
                    .Select((e) => e.Key)
                    .Where((e) => Registry.GetValue($@"HKEY_CLASSES_ROOT\{e}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string == null)
                    //.Where((e) => Registry.ClassesRoot.OpenSubKey(e, false).GetSubKeyNames().Any((skn) => skn.ToLowerInvariant().Equals("shellex")) == false)
                    .Select((e) => new TreeNode() { Name = e, Text = e })
                    .ToArray()
                );
        }

        public void GetClassFulDots() {
            tvMain.Nodes["ClassFulDotExtensions"].Nodes.AddRange(
                DotExtensions.Where((e) => Registry.ClassesRoot.GetSubKeyNames().Any((skn) => skn.ToLowerInvariant().Equals(((Registry.GetValue($@"HKEY_CLASSES_ROOT\{e.Key}", null, null) as string)??"").ToLowerInvariant())))
                .Select((e) => new TreeNode { Name = e.Key, Text = e.Key }).ToArray()
                );            
        }

        public Dictionary<string, string> GetAllExtensions() {
            var dictTmp = new Dictionary<string, string>();

            Registry.ClassesRoot.GetSubKeyNames().ToList().ForEach((k) => {
                dictTmp.Add(k, Registry.GetValue($@"HKEY_CLASSES_ROOT\{k}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string);
            });

            return dictTmp;
        }

        public Dictionary<string, string> GetDotExtensions(){
            var dictTmp = new Dictionary<string, string>();
            GetAllExtensions().ToList().ForEach((kvp) => {
                dictTmp.Add(kvp.Key, kvp.Value);
            });
            return dictTmp;
        }
        
        public string GetExtensionHandlerId(string Extension){
            return ""; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).GetSubKeyNames();
             
        }

        public void WR(RegistryKey regKey = null) {
            
            if (regKey == null) regKey = Registry.ClassesRoot.OpenSubKey("");  //Registry.LocalMachine;// RegistryKey().OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);


            foreach (var keyName in regKey.GetSubKeyNames()) {
                try
                {
                    var rk = regKey.OpenSubKey(keyName);

                    if (rk.Name.Contains("{8895B1C6-B41F-4C1C-A562-0D564250836F}"))
                        Console.WriteLine(  rk.Name);
                    WR(rk);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

        }
    }
}

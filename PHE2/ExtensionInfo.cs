using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHE2
{
    public class ExtensionsDictionary : SortedDictionary<string, ExtensionInfo>
    {
        public static RegistryKey ExtensionsKey = Registry.ClassesRoot;

        private static List<string> _extList = null;

        public static List<string> ExtList { get { if (_extList == null) { _extList = Registry.ClassesRoot.GetSubKeyNames().ToList(); }
                return _extList; }
        }

        public static List<string> DottedExtList { get { return ExtList.Where((i) => i.StartsWith(".")).ToList(); } }

        // HKCR\.bat\batFile
        // HKCR\barFile
        public static List<string> HasAliasExtList { get { return DottedExtList.Where((i) => Registry.GetValue($@"HKEY_CLASSES_ROOT\{Registry.ClassesRoot.OpenSubKey(i).GetValue(null, null) as string}", null, null) as string != null).ToList();
            } }

        public static List<string> HasNoAliasExtList { get { return ExtList.Except(HasAliasExtList).ToList(); } }

        public ExtensionsDictionary() { }

        public new ExtensionInfo this[string ext] {
            get { return ExtensionInfo.FromExt(ext); }
        }

        private List<ExtensionInfo> _handled=new List<ExtensionInfo>();
        public List<ExtensionInfo> Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }

        public void Populate() {

            ExtensionsKey.GetSubKeyNames()                
                .Where( (k)=>!this.ContainsKey(k) )
                .ToList()
                .ForEach((k)=>{
                    this.Add(k, new ExtensionInfo(k));
            });
        }

        public ExtensionsDictionary(IDictionary<string, ExtensionInfo> dictionary) : base(dictionary)
        {        }

        public ExtensionsDictionary(IComparer<string> comparer) : base(comparer)
        {        }

        public ExtensionsDictionary(IDictionary<string, ExtensionInfo> dictionary, IComparer<string> comparer) : base(dictionary, comparer)
        {        }

        
    }

    public class ExtensionInfo : IComparable
    {

        private string _previewHandlerGuid = null;
        private string _ext = null;
        private RegistryKey _extRegKey = null;
        private RegistryKey _defRegKey = null;
        private string _default = null;
        private bool? _hasAlias = null;
        private bool _fullLoaded = false;
        private bool _hasPhv = false;


        public static ExtensionInfo FromExt(string ext) => new ExtensionInfo(ext);


        public string Ext => _ext;

        public bool HasPhv {
            get {
                if (!_fullLoaded) Load();
                return _hasPhv;
            }
        }

        public void Load() {

            if (_fullLoaded) return;

            _extRegKey = Registry.ClassesRoot.OpenSubKey($@"{_ext}");

            _default = _extRegKey.GetValue(null, null) as string;
            
            _defRegKey = Registry.ClassesRoot.OpenSubKey($@"{_default}");

            _previewHandlerGuid =            Registry.GetValue($@"{Registry.ClassesRoot.Name}\{ (HasAlias ? _default : _ext)}\shellEx\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string;
            //_previewHandlerGuid = (HasAlias?_defRegKey: _extRegKey).OpenSubKey(@"shellEx\{8895b1c6-b41f-4c1c-a562-0d564250836f}").GetValue(null, null) as string;
            if (_previewHandlerGuid != null)
            {
                if (!Program.PreviewHandlers.ContainsKey(_previewHandlerGuid))
                {
                    Program.PreviewHandlers.Add(_previewHandlerGuid, PreviewHandlerInfo.FromGuid(_previewHandlerGuid));

                };
                _hasPhv = true;
            }
            _fullLoaded = true;

        }

        public string PreviewHandlerGuid {

            get {
                if (!_fullLoaded) { Load(); }

//                    if (HasAlias) {
  //                      _previewHandlerGuid = DefRegKey.OpenSubKey(@"shellEx\{8895b1c6-b41f-4c1c-a562-0d564250836f}").GetValue(null, null) as string;
    //                
      //              else
        //                _previewHandlerGuid = ExtRegKey.OpenSubKey(@"shellEx\{8895b1c6-b41f-4c1c-a562-0d564250836f}").GetValue(null, null) as string;                    

                    if (_previewHandlerGuid != null){
                        if (!Program.PreviewHandlers.ContainsKey(_previewHandlerGuid))
                            Program.PreviewHandlers.Add(_previewHandlerGuid, new PreviewHandlerInfo(_previewHandlerGuid));
                    
                        if (!Program.PreviewHandlers[_previewHandlerGuid].HandlingExtensions.Contains(this.Ext))
                            Program.PreviewHandlers[_previewHandlerGuid].HandlingExtensions.Add(Ext);
                    Program.Extensions.Handled.Add(this);
                }                

                return _previewHandlerGuid;
            }

            set {
                if (value != _previewHandlerGuid) {
                    _previewHandlerGuid = value;
                }
                if (_previewHandlerGuid != null && Program.PreviewHandlers.ContainsKey(_previewHandlerGuid))
                {
                    if (!Program.PreviewHandlers[_previewHandlerGuid].HandlingExtensions.Contains(this.Ext))
                        Program.PreviewHandlers[_previewHandlerGuid].HandlingExtensions.Add(Ext);
                }
            }
        }


        public PreviewHandlerInfo PreviewHandler
        {
            get
            {
                if (!_fullLoaded)
                    Load();

                return _previewHandlerGuid == null ? null : Program.PreviewHandlers[_previewHandlerGuid];                
            }
        }

        public RegistryKey ExtRegKey {
            get { if (_extRegKey == null) {
                    _extRegKey = Registry.ClassesRoot.OpenSubKey($@"{_ext}"); }
                return _extRegKey;
            }
        }
        public RegistryKey DefRegKey {
            get {
                if (_defRegKey == null) {
                    _defRegKey = Registry.ClassesRoot.OpenSubKey(Default); }
                return _defRegKey;    }
            }
        

        public string Default { get { if (_default == null) { _default = ExtRegKey.GetValue(null, null) as string; } return _default; } }

        public bool HasAlias {
            get
            {
                return ExtensionsDictionary.HasAliasExtList.Contains(_ext, StringComparer.InvariantCultureIgnoreCase);// Any((e) => e.Equals(_ext, StringComparison.InvariantCultureIgnoreCase));

                if (!_fullLoaded) { Load(); }
                
                if (!_hasAlias.HasValue) {
                    _hasAlias= ExtensionsDictionary.ExtList.Any((s) => s.Equals(Default, StringComparison.InvariantCultureIgnoreCase));

                    if (_hasAlias.Value.Equals(true))
                    {
//                        _extRegKey = Registry.ClassesRoot.OpenSubKey(Default);
                        _previewHandlerGuid = ExtRegKey.GetValue(@"shellEx\{8895b1c6-b41f-4c1c-a562-0d564250836f}", null) as string;
                    }

                }
                return _hasAlias.Value;
            }
        }
        
        public ExtensionInfo(string ext) {
            _ext = ext;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is ExtensionInfo))
                throw new InvalidCastException("Object no ExtensionInfo!");
            var ei = obj as ExtensionInfo;            
            return this.Ext.ToLowerInvariant().CompareTo(ei.Ext.ToLowerInvariant());
        }

        public override string ToString()=> $"{Ext}\t{HasAlias}\t{PreviewHandlerGuid}\t{PreviewHandler.Name}";
        
        public bool IsDotted=>Ext.StartsWith(".");
    }
}

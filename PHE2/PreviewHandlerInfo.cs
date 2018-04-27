using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHE2
{
  
    public class PreviewHandlersDictionary : SortedDictionary<string, PreviewHandlerInfo>
    {
        private static List<string> _guidList = null;

        public static RegistryKey PvhKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers");

        public static List<string> GuidList {
            get {
                if (_guidList == null) _guidList = PvhKey.GetValueNames().ToList();
                    return _guidList; }
        }

        public PreviewHandlerInfo this[string guid]=>new PreviewHandlerInfo(guid);

    }

    public class PreviewHandlerInfo
    {
        private string _guid = null;
        private string _name = null;
        private List<string> _handlingExtensions = null;

        public string GUID { get {return _guid; }}

        public string Name { get{ if (_name == null) _name = PreviewHandlersDictionary.PvhKey.GetValue(_guid) as string;
                return _name;
            } set { if (value != _name) { _name = value; }; } }

        public List<string> HandlingExtensions {
            get
            {
                if (_handlingExtensions == null)
                    _handlingExtensions = new List<string>();
                return _handlingExtensions;
            }
        }

        public static PreviewHandlerInfo FromGuid(string guid) {
            return new PreviewHandlerInfo(guid, true);
        }

        public PreviewHandlerInfo(string guid) { _guid = guid; }
        public PreviewHandlerInfo(string guid, bool fullLoad = false) {
            _guid = guid;
            if (fullLoad) {
                var he = HandlingExtensions;
                var n = Name;

            }
        }


    }
}

// Stephen Toub


using Microsoft.Win32;

namespace PreviewHandlerEditor
{
   
public class ExtensionInfo{

        public ExtensionInfo() {

        }
        public ExtensionInfo(string extension) {
            var hId = Registry.GetValue($@"HKEY_CLASSES_ROOT\{extension}\shellex\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", null, null) as string;

            Handler = new PreviewHandlerInfo{
                ID = hId,
                Name = Registry.LocalMachine.GetValue($@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers\\{hId}") as string
            };
        }
            public string Extension;
            public PreviewHandlerInfo Handler;
            public override string ToString() => Extension; 
        }
    }

// Stephen Toub


namespace PreviewHandlerEditor
{
        public class PreviewHandlerInfo
        {
            public string ID;
            public string Name;
            public override string ToString() =>string.IsNullOrEmpty(Name)?ID: Name;
            public static PreviewHandlerInfo Unspecified => new PreviewHandlerInfo {
                ID = "-",
                Name = "<Unspecified>"
            };  
        }
    }
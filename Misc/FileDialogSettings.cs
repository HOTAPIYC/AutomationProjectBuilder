using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public class FileDialogSettings
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Filter { get; set; }
        public bool CheckFileExists { get; set; }
        public bool CheckPathExists { get; set; }
    }
}

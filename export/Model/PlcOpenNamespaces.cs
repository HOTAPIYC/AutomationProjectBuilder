using System.Xml.Linq;

namespace AutomationProjectBuilder.Export.Model
{
    public static class PlcOpenNamespaces
    {
        public static XNamespace Ns { get; } = "http://www.plcopen.org/xml/tc6_0200";
        public static XNamespace Xhtmlns { get; } = "http://www.w3.org/1999/xhtml";
    }
}

using CimTools.v2.Attributes;
using System.Collections.Generic;

namespace CimTools.v2.Data
{
    [XmlOptions("OptionPanel")]
    public class OptionPanelSavedData
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
    }
}

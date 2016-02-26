using System;

namespace CimTools.v2.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XmlOptionsAttribute : Attribute
    {
        public string Key = null;
    }
}

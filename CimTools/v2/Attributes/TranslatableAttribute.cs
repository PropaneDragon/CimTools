using System;

namespace CimTools.v2.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TranslatableAttribute : Attribute
    {
        public string identifier = "";
    }
}

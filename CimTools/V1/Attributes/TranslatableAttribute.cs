using System;

namespace CimTools.V1.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TranslatableAttribute : Attribute
    {
        public string identifier = "";
    }
}

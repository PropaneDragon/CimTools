using System.Reflection;

namespace CimTools.v2.Data
{
    public class Strings
    {
        private CimToolBase _toolBase = null;

        public Strings(CimToolBase toolBase)
        {
            _toolBase = toolBase;

            SetUpStrings();
        }

        private void SetUpStrings()
        {
            VERSION = string.Format("V{0}", Assembly.GetAssembly(typeof(CimToolBase)).GetName().Version.Major);
        }

        /// <summary>
        /// Current version of CimTools. Returns the letter V followed by the version number
        /// </summary>
        public string VERSION = "V2";

        /// <summary>
        /// The group name by which save files are searched for using the XmlOptionsAttribute
        /// </summary>
        public readonly string SAVE_FILE_GROUP_NAME = "SaveFile";
    }
}

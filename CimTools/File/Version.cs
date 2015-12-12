using System;
using System.Reflection;

namespace CimTools.File
{
    /// <summary>
    /// Handles everything to do with the mod version number
    /// </summary>
    public class Version
    {
        public enum Limit { Major, Minor, Build, Revision };

        /// <summary>
        /// Major version ([1].2.35.623)
        /// </summary>
        /// <param name="thisAssembly">The assembly to get the verison number of</param>
        /// <returns>The major number</returns>
        public static int Major(Assembly thisAssembly)
        {
            return thisAssembly.GetName().Version.Major;
        }

        /// <summary>
        /// Minor version (1.[2].35.623)
        /// </summary>
        /// <param name="thisAssembly">The assembly to get the verison number of</param>
        /// <returns>The minor number</returns>
        public static int Minor(Assembly thisAssembly)
        {
            return thisAssembly.GetName().Version.Minor;
        }

        /// <summary>
        /// Build version (1.2.[35].623)
        /// </summary>
        /// <param name="thisAssembly">The assembly to get the verison number of</param>
        /// <returns>The build number</returns>
        public static int Build(Assembly thisAssembly)
        {
            return thisAssembly.GetName().Version.Build;
        }

        /// <summary>
        /// Revision version (1.2.35.[623])
        /// </summary>
        /// <param name="thisAssembly">The assembly to get the verison number of</param>
        /// <returns>The revision number</returns>
        public static int Revision(Assembly thisAssembly)
        {
            return thisAssembly.GetName().Version.Revision;
        }

        /// <summary>
        /// Returns a delimited version string
        /// </summary>
        /// <param name="delimiter">The delimiter to place between version numbers</param>
        /// <param name="upTo">Return a version number up to this limit</param>
        /// <returns>A delimited version string</returns>
        public static string Delimited(Assembly thisAssembly, Limit upTo = Limit.Build, string delimiter = ".")
        {
            string returnVersion = Major(thisAssembly).ToString();

            if (upTo >= Limit.Minor) { returnVersion += delimiter + Minor(thisAssembly).ToString(); }
            if (upTo >= Limit.Build) { returnVersion += delimiter + Build(thisAssembly).ToString(); }
            if (upTo >= Limit.Revision) { returnVersion += delimiter + Revision(thisAssembly).ToString(); }

            return returnVersion;
        }
    }
}

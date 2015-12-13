using System;
using System.Reflection;

namespace CimTools.File
{
    /// <summary>
    /// Handles everything to do with the mod version number
    /// </summary>
    public class Version
    {
        /// <summary>
        /// How far to limit the version numbering to.
        /// </summary>
        public enum Limit
        {
            /// <summary>
            /// Major version ([1].2.35.623)
            /// </summary>
            Major,

            /// <summary>
            /// Minor version (1.[2].35.623)
            /// </summary>
            Minor,

            /// <summary>
            /// Build version (1.2.[35].623)
            /// </summary>
            Build,

            /// <summary>
            /// Revision version (1.2.35.[623])
            /// </summary>
            Revision
        };

        /// <summary>
        /// Major version ([1].2.35.623)
        /// </summary>
        /// <returns>The major number</returns>
        public static int Major()
        {
            return Settings.ModAssembly != null ? Settings.ModAssembly.GetName().Version.Major : -1;
        }

        /// <summary>
        /// Minor version (1.[2].35.623)
        /// </summary>
        /// <returns>The minor number</returns>
        public static int Minor()
        {
            return Settings.ModAssembly != null ? Settings.ModAssembly.GetName().Version.Minor : -1;
        }

        /// <summary>
        /// Build version (1.2.[35].623)
        /// </summary>
        /// <returns>The build number</returns>
        public static int Build()
        {
            return Settings.ModAssembly != null ? Settings.ModAssembly.GetName().Version.Build : -1;
        }

        /// <summary>
        /// Revision version (1.2.35.[623])
        /// </summary>
        /// <returns>The revision number</returns>
        public static int Revision()
        {
            return Settings.ModAssembly != null ? Settings.ModAssembly.GetName().Version.Revision : -1;
        }

        /// <summary>
        /// Returns a delimited version string
        /// </summary>
        /// <param name="delimiter">The delimiter to place between version numbers.</param>
        /// <param name="upTo">Return a version number up to this limit.</param>
        /// <returns>A delimited version string</returns>
        public static string Delimited(Limit upTo = Limit.Build, string delimiter = ".")
        {
            string returnVersion = Major().ToString();

            if (upTo >= Limit.Minor) { returnVersion += delimiter + Minor().ToString(); }
            if (upTo >= Limit.Build) { returnVersion += delimiter + Build().ToString(); }
            if (upTo >= Limit.Revision) { returnVersion += delimiter + Revision().ToString(); }

            return returnVersion;
        }
    }
}

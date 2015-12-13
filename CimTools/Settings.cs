using System.Reflection;

namespace CimTools
{
    /// <summary>
    /// Global settings for CimTools. Set these up before doing anything
    /// with the tools, or elements may not work properly.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The name of the mod you wish to use.
        /// <para>
        /// Required for: Path, PersistentOptions
        /// </para>
        /// </summary>
        public static string ModName = null;

        /// <summary>
        /// The assembly to get the version name from. Use <code>Assembly.GetExecutingAssembly()</code> if unsure.
        /// <para>
        /// Required for: Version.
        /// </para>
        /// </summary>
        public static Assembly ModAssembly = null;

        /// <summary>
        /// The workshop ID of the mod.
        /// <para>
        /// Required for: Changelog.
        /// </para>
        /// </summary>
        public static ulong? WorkshopID = null;
    }
}

namespace CimTools.File
{
    class InternalOptions : ExportOptionBase
    {
        private static InternalOptions instance = null;

        /// <summary>
        /// Gets an instance of the SavedOptions
        /// </summary>
        /// <returns>An instance of SavedOptions</returns>
        public static InternalOptions Instance()
        {
            if (instance == null)
            {
                instance = new InternalOptions();
            }

            return instance;
        }

        /// <summary>
        /// Change the instance used for the options.
        /// </summary>
        /// <param name="optionManager">The SavedOptions to replace the existing instance</param>
        public static void SetInstance(InternalOptions optionManager)
        {
            if (optionManager != null)
            {
                instance = optionManager;
            }
        }
    }
}

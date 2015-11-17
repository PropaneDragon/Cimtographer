namespace Mapper.CustomUI
{
    /// <summary>
    /// Option storage
    /// </summary>
    class OptionItem
    {
        /// <summary>
        /// The option value
        /// </summary>
        public bool value = true;

        /// <summary>
        /// Whether the option can be edited
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// A unique identifier for finding the option later
        /// </summary>
        public string id = "";

        /// <summary>
        /// The readable string to be printed out
        /// </summary>
        public string readableLabel = "";
    }
}

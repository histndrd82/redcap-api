namespace VCU.Redcap.Models
{
    /// <summary>
    /// <list type="none">
    ///     <item>normal - blank/empty values will be ignored [default]</item>
    ///     <item>overwrite - blank/empty values are valid and will overwrite data</item>
    /// </list>
    /// </summary>
    public enum OverwriteBehavior
    {
        /// <summary>
        /// blank/empty values will be ignored [default]
        /// </summary>
        normal = 0,
        /// <summary>
        /// blank/empty values are valid and will overwrite data
        /// </summary>
        overwrite = 1
    }
}

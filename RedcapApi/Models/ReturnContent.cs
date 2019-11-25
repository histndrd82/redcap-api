namespace VCU.Redcap.Models
{
    /// <summary>
    /// The content that the response object should contain from Redcap API.
    /// <list type="none">
    ///     <item>ReturnContent => 0 = ids</item>
    ///     <item>ReturnContent => 1 = count</item>
    /// </list>
    /// </summary>
    public enum ReturnContent
    {
        /// <summary>
        /// ids - a list of all record IDs that were imported
        /// </summary>
        ids = 0,
        /// <summary>
        ///  count [default] - the number of records imported
        /// </summary>
        count = 1
    }
}

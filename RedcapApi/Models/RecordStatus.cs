using System.ComponentModel.DataAnnotations;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// Represents the status of the record in redcap
    /// <list type="none">
    ///     <item>Incomplete = 0</item>
    ///     <item>Unverified = 1</item>
    ///     <item>Complete = 2</item>
    /// </list>
    /// </summary>
    public enum RedcapStatus
    {
        /// <summary>
        /// Instrument is incomplete for the record
        /// </summary>
        [Display(Name ="Incomplete")]
        Incomplete = 0,
        /// <summary>
        /// Instrument is unverified for the record
        /// </summary>
        ///
        [Display(Name ="Unverified")]
        Unverified = 1,

        /// <summary>
        /// Instrument is complete for the record
        /// </summary>
        /// 
        [Display(Name ="Complete")]
        Complete = 2
    }
}
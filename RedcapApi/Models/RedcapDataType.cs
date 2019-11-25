using System.ComponentModel.DataAnnotations;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// The data format which the Redcap API endpoint should receive.
    /// <list type="none">
    ///     <item>RedcapDataType => 0 = flat</item>
    ///     <item>RedcapDataType => 1 = eav</item>
    ///     <item>RedcapDataType => 2 = nonlogitudinal</item>
    ///     <item>RedcapDataType => 3 = longitudinal</item>
    /// </list>
    /// </summary>
    public enum RedcapDataType
    {
        /// <summary>
        /// output as one record per row [default]
        /// </summary>
        /// 
        [Display(Name = "flat")]
        flat = 0,
        /// <summary>
        /// input as one data point per row
        /// </summary>
        /// 
        [Display(Name = "eav")]
        eav = 1,
        /// <summary>
        /// EAV: Non-longitudinal: Will have the fields - record*, field_name, value
        /// </summary>
        /// 
        [Display(Name = "nonLongitudinal")]
        nonlongitudinal = 2,
        /// <summary>
        /// EAV: Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name
        /// </summary>
        /// 
        [Display(Name = "longitudinal")]
        longitudinal = 3
    }
}

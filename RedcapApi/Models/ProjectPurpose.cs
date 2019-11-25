using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// This is a direct translation of redcap purpose.
    /// <list type="none">
    ///     <item>raw [default]</item>
    ///     <item>label - export the raw coded values or labels for the options of multiple choice fields</item>
    /// </list> 
    /// </summary>
    public enum ProjectPurpose
    {
        /// <summary>
        /// Pratice 
        /// </summary>
        /// 
        [Display(Name = "Pratice or For Fun")]
        [Description("Pratice or For Fun")]
        PraticeForFun = 0,

        /// <summary>
        /// Other
        /// </summary>
        /// 
        [Display(Name = "Other")]
        [Description("Other")]
        Other = 1,

        /// <summary>
        /// Research
        /// </summary>
        /// 
        [Display(Name = "Research")]
        [Description("Research")]
        Research = 2,

        /// <summary>
        /// Quality Improvement
        /// </summary>
        /// 
        [Display(Name = "Quality Improvement")]
        [Description("Quality Improvement")]
        QualityImprovement = 3,

        /// <summary>
        /// Other
        /// </summary>
        /// 
        [Display(Name = "Operational Support")]
        [Description("Operational Support")]
        OperationalSupport = 4
    }
}

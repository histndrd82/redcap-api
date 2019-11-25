using System.ComponentModel.DataAnnotations;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// Redcap API Actions 
    /// </summary>
    public enum RedcapAction
    {
        /// <summary>
        /// Export Action
        /// </summary>
        /// 
        [Display(Name = "export")]
        Export,

        /// <summary>
        /// Import Action
        /// </summary>
        /// 
        [Display(Name = "import")]
        Import,
        /// <summary>
        /// Delete Action
        /// </summary>
        /// 
        [Display(Name = "delete")]
        Delete,
    }
}

using System.ComponentModel.DataAnnotations;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// The format that the response object should be when returned from the http request.
    /// <list type="none">
    ///     <item>Format, 0 = JSON</item>
    ///     <item>Format, 1 = CSV</item>
    ///     <item>Format, 2 = XML</item>
    /// </list>
    /// </summary>
    public enum ReturnFormat
    {
        /// <summary>
        /// Default Javascript Notation
        /// </summary>
        /// 
        [Display(Name = "json")]
        json = 0,
        /// <summary>
        /// Comma Seperated Values
        /// </summary>
        /// 
        [Display(Name = "csv")]
        csv = 1,
        /// <summary>
        /// Extensible Markup Language
        /// </summary>
        /// 
        [Display(Name = "xml")]
        xml = 2,
        /// <summary>
        /// CDISC ODM XML format, specifically ODM version 1.3.1
        /// Only usable on Project Create 
        /// </summary>
        /// 
        [Display(Name = "odm")]
        odm = 3
    }
}

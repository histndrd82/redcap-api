﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// API Action 
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

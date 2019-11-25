using Newtonsoft.Json;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// This class is a template for a redcap project.
    /// Minimum redcap project information when creating a project
    /// 1.  Project Title
    /// 2.  Purpose
    /// 
    /// </summary>
    public class RedcapProject
    {
        /// <summary>
        /// Title of project
        /// </summary>
        /// 
        [JsonProperty("project_title")]
        public string ProjectTitle { get; set; }
        /// <summary>
        /// Purpose, i.e. 0, 1, 2, 3
        /// 0 = Pratice For Fun
        /// 1 = Other
        /// 2 = Research
        /// 3 = Quality Improvement
        /// 4 = Other
        /// </summary>
        /// 
        [JsonProperty("purpose")]
        public ProjectPurpose Purpose { get; set; }
        /// <summary>
        /// Other purpose
        /// </summary>
        /// 
        [JsonProperty("purpose_other")]
        public string PurposeOther { get; set; }
        /// <summary>
        /// Any project notes?
        /// </summary>
        /// 
        [JsonProperty("project_notes")]
        public string ProjectNotes { get; set; }
        /// <summary>
        /// A project that contains instruments or forms that spans over time.
        /// </summary>
        /// 
        [JsonProperty("is_longitudinal")]
        public bool IsLongitudinal { get; set; }
        /// <summary>
        /// Will surveys be enabled for this project?
        /// </summary>
        /// 
        [JsonProperty("surveys_enabled")]
        public bool SurveysEnabled { get; set; }
        /// <summary>
        /// Will auto numbering be enabled for this project?
        /// </summary>
        /// 
        [JsonProperty("record_autonumbering_enabled")]
        public bool RecordAutoNumberingEnabled { get; set; }

    }

}

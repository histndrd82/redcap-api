using Newtonsoft.Json;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// Project details
    /// </summary>
    public class RedcapProjectDetail
    {

        /// <summary>
        /// Project Identifier
        /// </summary>
        /// 
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
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
        /// The preferred language
        /// </summary>
        /// 
        [JsonProperty("project_language")]
        public string ProjectLanguage { get; set; }
        /// <summary>
        /// Custom record label.
        /// </summary>
        /// 
        [JsonProperty("custom_record_label")]
        public string CustomRecordLabel { get; set; }
        /// <summary>
        /// Secondary Unique Field
        /// </summary>
        /// 
        [JsonProperty("secondary_unique_field")]
        public string SecondaryUniqueField { get; set; }
        /// <summary>
        /// Is Longitudinal enabled?
        /// </summary>
        /// 
        [JsonProperty("is_longitudinal")]
        public bool IsLongitudinal { get; set; }
        /// <summary>
        /// Is Surveys enabled?
        /// </summary>
        /// 
        [JsonProperty("surveys_enabled")]
        public bool SurveysEnabled { get; set; }
        /// <summary>
        /// Is Scheduling enabled?
        /// </summary>
        /// 
        [JsonProperty("scheduling_enabled")]
        public bool SchedulingEnabled { get; set; }
        /// <summary>
        /// Enable auto numbering?
        /// </summary>
        /// 
        [JsonProperty("record_autonumbering_enabled")]
        public bool RecordAutonumberingEnabled { get; set; }
        /// <summary>
        /// Enable randomization?
        /// </summary>
        /// 
        [JsonProperty("randomization_enabled")]
        public bool RandomizationEnabled { get; set; }
        /// <summary>
        /// Irb Number
        /// </summary>
        /// 
        [JsonProperty("project_irb_number")]
        public string ProjectIrbNumber { get; set; }
        /// <summary>
        /// Grant Number
        /// </summary>
        /// 
        [JsonProperty("project_grant_number")]
        public string ProjectGrantNumber { get; set; }
        /// <summary>
        /// PI's First Name
        /// </summary>
        /// 
        [JsonProperty("project_pi_firstname")]
        public string ProjectPiFirstName { get; set; }
        /// <summary>
        /// PI's Last Name
        /// </summary>
        /// 
        [JsonProperty("project_pi_lastname")]
        public string ProjectPiLastName { get; set; }
        /// <summary>
        /// Display Today Now Button
        /// </summary>
        /// 
        [JsonProperty("display_today_now_button")]
        public bool DisplayTodayNowButton { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="purposeOther"></param>
        /// <param name="projectNotes"></param>
        /// <param name="projectLanguage"></param>
        /// <param name="customRecordLabel"></param>
        /// <param name="secondaryUniqueField"></param>
        /// <param name="isLongitudinal"></param>
        /// <param name="surveysEnabled"></param>
        /// <param name="schedulingEnabled"></param>
        /// <param name="recordAutonumberingEnabled"></param>
        /// <param name="randomizationEnabled"></param>
        /// <param name="projectIrbNumber"></param>
        /// <param name="projectGrantNumber"></param>
        /// <param name="projectPiFirstName"></param>
        /// <param name="projectPiLastName"></param>
        /// <param name="displayTodayNowButton"></param>
        public RedcapProjectDetail(string projectId, string purposeOther, string projectNotes, string projectLanguage, string customRecordLabel, string secondaryUniqueField,
            bool isLongitudinal, bool surveysEnabled, bool schedulingEnabled, bool recordAutonumberingEnabled, bool randomizationEnabled, string projectIrbNumber,
            string projectGrantNumber, string projectPiFirstName, string projectPiLastName, bool displayTodayNowButton)
        {
            ProjectId = projectId ?? throw new System.ArgumentNullException(nameof(projectId));
            PurposeOther = purposeOther ?? throw new System.ArgumentNullException(nameof(purposeOther));
            ProjectNotes = projectNotes ?? throw new System.ArgumentNullException(nameof(projectNotes));
            ProjectLanguage = projectLanguage ?? throw new System.ArgumentNullException(nameof(projectLanguage));
            CustomRecordLabel = customRecordLabel ?? throw new System.ArgumentNullException(nameof(customRecordLabel));
            SecondaryUniqueField = secondaryUniqueField ?? throw new System.ArgumentNullException(nameof(secondaryUniqueField)); ;
            IsLongitudinal = isLongitudinal;
            SurveysEnabled = surveysEnabled;
            SchedulingEnabled = schedulingEnabled;
            RecordAutonumberingEnabled = recordAutonumberingEnabled;
            RandomizationEnabled = randomizationEnabled;
            ProjectIrbNumber = projectIrbNumber ?? throw new System.ArgumentNullException(nameof(projectIrbNumber));
            ProjectGrantNumber = projectGrantNumber ?? throw new System.ArgumentNullException(nameof(projectGrantNumber));
            ProjectPiFirstName = projectPiFirstName ?? throw new System.ArgumentNullException(nameof(projectPiFirstName));
            ProjectPiLastName = projectPiLastName ?? throw new System.ArgumentNullException(nameof(projectPiLastName));
            DisplayTodayNowButton = displayTodayNowButton;
        }
    }

}

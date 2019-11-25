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
        /// Details related to the project
        /// </summary>
        public RedcapProjectDetail RedcapProjectDetail { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedcapProject()
        {
            ProjectTitle = string.Empty;
            Purpose = ProjectPurpose.PraticeForFun;
        }
        /// <summary>
        /// Constructor overload
        /// </summary>
        /// <param name="projectTitle"></param>
        /// <param name="purpose"></param>
        /// <param name="projectDetail"></param>
        public RedcapProject(string projectTitle, ProjectPurpose purpose, RedcapProjectDetail projectDetail)
        {
            ProjectTitle = projectTitle;
            Purpose = purpose;
            RedcapProjectDetail = projectDetail;
        }
    }

}

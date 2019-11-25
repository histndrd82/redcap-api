using Newtonsoft.Json;

namespace VCU.Redcap.Models
{
    /// <summary>
    /// MetaData, the data dictionary for a particular variable in redcap
    /// </summary>
    public class RedcapMetaData
    {
        /// <summary>
        /// field name
        /// </summary>
        /// 
        [JsonProperty("field_name")]
        public string FieldName { get; set; }
        /// <summary>
        /// the form name / instrument name
        /// </summary>
        /// 
        [JsonProperty("form_name")]
        public string FormName { get; set; }
        /// <summary>
        /// section header of instrument / form
        /// </summary>
        /// 
        [JsonProperty("section_header")]
        public string SectionHeader { get; set; }
        /// <summary>
        /// the type of field, i.e "text", "radio"
        /// </summary>
        /// 
        [JsonProperty("field_type")]
        public string FieldType { get; set; }
        /// <summary>
        /// the label for the field, "first name"
        /// </summary>
        /// 
        [JsonProperty("field_label")]
        public string FieldLabel { get; set; }
        /// <summary>
        /// Choices, Calculations, OR Slider Labels
        /// </summary>
        /// 
        [JsonProperty("select_choices_or_calculations")]
        public string SelectChoicesOrCalculations { get; set; }
        /// <summary>
        /// field not, special instruction for the label
        /// </summary>
        /// 
        [JsonProperty("field_note")]
        public string FieldNote { get; set; }
        /// <summary>
        /// ui field validation
        /// </summary>
        /// 
        [JsonProperty("text_validation_type_or_show_slider_number")]
        public string TextValidationTypeOrShowSliderNumber { get; set; }
        /// <summary>
        /// min value if int/num
        /// </summary>
        /// 
        [JsonProperty("text_validation_min")]
        public string TextValidationMin { get; set; }
        /// <summary>
        /// max value if int/num
        /// </summary>
        /// 
        [JsonProperty("text_validation_max")]
        public string TextValidationMax { get; set; }
        /// <summary>
        /// flag
        /// </summary>
        /// 
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        /// <summary>
        /// the branching logic
        /// </summary>
        /// 
        [JsonProperty("branching_logic")]
        public string BranchingLogic { get; set; }
        /// <summary>
        /// flag
        /// </summary>
        /// 
        [JsonProperty("required_field")]
        public string RequiredField { get; set; }
        /// <summary>
        /// LH, RH etc
        /// </summary>
        /// 
        [JsonProperty("custom_alignment")]
        public string CustomAlignment { get; set; }
        /// <summary>
        /// if numbered
        /// </summary>
        /// 
        [JsonProperty("question_number")]
        public string QuestionNumber { get; set; }
        /// <summary>
        /// is this a matrix group question, what is the name of the group
        /// </summary>
        /// 
        [JsonProperty("matrix_group_name")]
        public string MatrixGroupName { get; set; }
        /// <summary>
        /// the rank
        /// </summary>
        /// 
        [JsonProperty("maxtrix_ranking")]
        public string MatrixRanking { get; set; }
        /// <summary>
        /// field annotation
        /// </summary>
        /// 
        [JsonProperty("field_annotation")]
        public string FieldAnnotation { get; set; }

    }

}

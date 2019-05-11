using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redcap.Models;

namespace Redcap
{
    /// <summary>
    /// The REDCap API is an interface that allows external applications 
    /// to connect to REDCap remotely, and is used for programmatically 
    /// retrieving or modifying data or settings within REDCap, such as performing 
    /// automated data imports/exports for a specified REDCap project. 
    /// Programmers can use the REDCap API to make applications, websites, apps, widgets, 
    /// and other projects that interact with REDCap. 
    /// 
    /// Virginia Commonwealth University
    /// Author: Michael Tran tranpl@outlook.com, tranpl@vcu.edu
    /// </summary>
    public interface IRedcapApi
    {
        /// <summary>
        /// <para>API Version 1.0.0+ </para>
        /// 
        /// <para>From Redcap Version 6.11.0 </para>
        /// <para>Create A New Project</para>
        /// 
        /// <para>This method allows you to create a new REDCap project. A 64-character Super API Token is required for this method (as opposed to project-level API methods that require a regular 32-character token associated with the project-user). </para>
        /// <para>In the API request, you must minimally provide the project attributes 'project_title' and 'purpose' (with numerical value 0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support) when creating a project. </para>
        /// <para>When a project is created with this method, the project will automatically be given all the project-level defaults just as if you created a new empty project via the web user interface, such as a automatically creating a single data collection instrument seeded with a single Record ID field and Form Status field, as well as (for longitudinal projects) one arm with one event.</para> 
        /// <para>And if you intend to create your own arms or events immediately after creating the project, it is recommended that you utilize the override=1 parameter in the 'Import Arms' or 'Import Events' method, respectively, so that the default arm and event are removed when you add your own.Also, the user creating the project will automatically be added to the project as a user with full user privileges and a project-level API token, which could then be used for subsequent project-level API requests.</para>
        /// <para>NOTE: Only users with Super API Tokens can utilize this method.Users can only be granted a super token by a REDCap administrator(using the API Tokens page in the REDCap Control Center). Please be advised that users with a Super API Token can create new REDCap projects via the API without any approval needed by a REDCap administrator.If you are interested in obtaining a super token, please contact your local REDCap administrator.</para>
        /// <para>Permissions Required: To use this method, you must have a Super API Token.</para>
        /// </summary>
        /// 
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The Super API Token specific to a user</param>
        /// <param name="content">project</param>
        /// <param name="format"></param>
        /// <param name="data">
        /// Contains the attributes of the project to be created, in which they are provided in the specified format. While the only required attributes are 'project_title' and 'purpose', the fields listed below are all the possible attributes that can be provided in the 'data' parameter. The 'purpose' attribute must have a numerical value (0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support), in which 'purpose_other' is only required to have a value (as a text string) if purpose=1. The attributes is_longitudinal (0=False, 1=True; Default=0), surveys_enabled (0=False, 1=True; Default=0), and record_autonumbering_enabled (0=False, 1=True; Default=1) are all boolean. Please note that either is_longitudinal=1 or surveys_enabled=1 does not add arms/events or surveys to the project, respectively, but it merely enables those settings which are seen at the top of the project's Project Setup page.
        /// All available attributes:
        /// project_title, purpose, purpose_other, project_notes, is_longitudinal, surveys_enabled, record_autonumbering_enabled
        /// JSON Example:
        /// [{"project_title":"My New REDCap Project","purpose":"0"}]
        /// </param>
        /// <param name="onErrorFormat"></param>
        /// <param name="odm">default: NULL - The 'odm' parameter must be an XML string in CDISC ODM XML format that contains project metadata (fields, forms, events, arms) and might optionally contain data to be imported as well. The XML contained in this parameter can come from a REDCap Project XML export file from REDCap itself, or may come from another system that is capable of exporting projects and data in CDISC ODM format. If the 'odm' parameter is included in the API request, it will use the XML to import its contents into the newly created project. This will allow you not only to create the project with the API request, but also to import all fields, forms, and project attributes (and events and arms, if longitudinal) as well as record data all at the same time.</param>
        /// <returns>
        /// When a project is created, a 32-character project-level API Token is returned (associated with both the project and user creating the project). 
        /// This token could then ostensibly be used to make subsequent API calls to this project, such as for adding new events, fields, records, etc.
        /// </returns>
        Task<string> CreateProjectAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null);
        
        /// <summary>
        /// <para>API Version 1.0.0+ </para>
        /// 
        /// <para>From Redcap Version 6.11.0 </para>
        /// <para>Create A New Project</para>
        /// 
        /// <para>This method allows you to create a new REDCap project. A 64-character Super API Token is required for this method (as opposed to project-level API methods that require a regular 32-character token associated with the project-user). </para>
        /// <para>In the API request, you must minimally provide the project attributes 'project_title' and 'purpose' (with numerical value 0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support) when creating a project. </para>
        /// <para>When a project is created with this method, the project will automatically be given all the project-level defaults just as if you created a new empty project via the web user interface, such as a automatically creating a single data collection instrument seeded with a single Record ID field and Form Status field, as well as (for longitudinal projects) one arm with one event.</para> 
        /// <para>And if you intend to create your own arms or events immediately after creating the project, it is recommended that you utilize the override=1 parameter in the 'Import Arms' or 'Import Events' method, respectively, so that the default arm and event are removed when you add your own.Also, the user creating the project will automatically be added to the project as a user with full user privileges and a project-level API token, which could then be used for subsequent project-level API requests.</para>
        /// <para>NOTE: Only users with Super API Tokens can utilize this method.Users can only be granted a super token by a REDCap administrator(using the API Tokens page in the REDCap Control Center). Please be advised that users with a Super API Token can create new REDCap projects via the API without any approval needed by a REDCap administrator.If you are interested in obtaining a super token, please contact your local REDCap administrator.</para>
        /// <para>Permissions Required: To use this method, you must have a Super API Token.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The Super API Token specific to a user</param>
        /// <param name="format"></param>
        /// <param name="data"></param>
        /// <param name="onErrorFormat"></param>
        /// <param name="odm">default: NULL - The 'odm' parameter must be an XML string in CDISC ODM XML format that contains project metadata (fields, forms, events, arms) and might optionally contain data to be imported as well. The XML contained in this parameter can come from a REDCap Project XML export file from REDCap itself, or may come from another system that is capable of exporting projects and data in CDISC ODM format. If the 'odm' parameter is included in the API request, it will use the XML to import its contents into the newly created project. This will allow you not only to create the project with the API request, but also to import all fields, forms, and project attributes (and events and arms, if longitudinal) as well as record data all at the same time.</param>
        /// <returns>
        /// When a project is created, a 32-character project-level API Token is returned (associated with both the project and user creating the project). 
        /// This token could then ostensibly be used to make subsequent API calls to this project, such as for adding new events, fields, records, etc.
        /// </returns>
        Task<string> CreateProjectAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null);

        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>From Redcap Version 4.7.0</para>
        /// Delete Arms
        /// <para>This method allows you to delete Arms from a project.</para>
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. Additionally, please be aware that deleting an arm also automatically deletes all events that belong to that arm, and will also automatically delete any records/data that have been collected under that arm (this is non-reversible data loss).    
        /// <para>NOTE: This only works for longitudinal projects.</para> 
        /// <para>Permissions Required: To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.</para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">arm</param>
        /// <param name="action">delete</param>
        /// <param name="arms">an array of arm numbers that you wish to delete</param>
        /// <returns>
        /// Number of Arms deleted
        /// </returns>
        Task<string> DeleteArmsAsync(string token, Content content, RedcapAction action, string[] arms);

        /// <summary>
        /// <para>From Redcap Version 4.7.0</para>
        /// Delete Arms
        /// <para>This method allows you to delete Arms from a project.</para>
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. Additionally, please be aware that deleting an arm also automatically deletes all events that belong to that arm, and will also automatically delete any records/data that have been collected under that arm (this is non-reversible data loss).    
        /// <para>NOTE: This only works for longitudinal projects.</para> 
        /// <para>Permissions Required: To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.</para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="arms">an array of arm numbers that you wish to delete</param>
        /// <returns>Number of Events deleted</returns>
        [Obsolete("Please use DeleteArmsAsync with token param")]
        Task<string> DeleteArmsAsync(string token, string[] arms);

        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>From Redcap Version 4.7.0</para>
        /// Delete Arms
        /// <para>This method allows you to delete Arms from a project.</para>
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. Additionally, please be aware that deleting an arm also automatically deletes all events that belong to that arm, and will also automatically delete any records/data that have been collected under that arm (this is non-reversible data loss).    
        /// <para>NOTE: This only works for longitudinal projects.</para> 
        /// <para>Permissions Required: To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>Number of Events deleted</returns>
        Task<string> DeleteArmsAsync<T>(T data);

        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>From Redcap Version 6.11.0</para>
        /// 
        /// <para>Delete Events</para>
        /// <para>This method allows you to delete Events from a project.</para>
        /// <para>Notice: Because of this method's destructive nature, it is only available for use for projects in Development status.</para> 
        /// <para>Additionally, please be aware that deleting an event will automatically delete any records/data that have been collected under that event (this is non-reversible data loss).</para>
        /// <para>NOTE: This only works for longitudinal projects.</para>
        /// <para>
        /// Permissions Required: To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="events">Array of unique event names</param>
        /// <returns>Number of Events deleted</returns>
        Task<string> DeleteEventsAsync(string token, string[] events = null);
        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>From Redcap Version 6.11.0</para>
        /// 
        /// <para>Delete Events</para>
        /// <para>This method allows you to delete Events from a project.</para>
        /// <para>Notice: Because of this method's destructive nature, it is only available for use for projects in Development status.</para> 
        /// <para>Additionally, please be aware that deleting an event will automatically delete any records/data that have been collected under that event (this is non-reversible data loss).</para>
        /// <para>NOTE: This only works for longitudinal projects.</para>
        /// <para>
        /// Permissions Required: To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">event</param>
        /// <param name="action">delete</param>
        /// <param name="events">Array of unique event names</param>
        /// <returns>Number of Events deleted</returns>
        Task<string> DeleteEventsAsync(string token, Content content, RedcapAction action, string[] events = null);
        /// <summary>
        /// <para>API Version 1.0.0+</para> 
        /// <para>Delete a File</para>
        /// <para>This method allows you to remove a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).</para>
        /// <para>
        /// Permissions Required: To use this method, you must have API Import/Update privileges in the project.
        /// </para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">file</param>
        /// <param name="action">delete</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>String</returns>
        Task<string> DeleteFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        /// <summary>
        /// <para>Delete a File</para>
        /// <para>This method allows you to remove a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).</para>
        /// <para>
        /// Permissions Required: To use this method, you must have API Import/Update privileges in the project.
        /// </para>
        /// </summary>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns></returns>
        [Obsolete("Please use DeleteFileAsync with token param ")]
        Task<string> DeleteFileAsync(string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);

        /// <summary>
        /// <para>API Version 1.0.0+</para> 
        /// <para>Delete a File</para>
        /// <para>This method allows you to remove a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).</para>
        /// <para>
        /// Permissions Required: To use this method, you must have API Import/Update privileges in the project.
        /// </para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>String</returns>
        Task<string> DeleteFileAsync(string token, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);

        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>Delete Records</para>
        /// <para>This method allows you to delete one or more records from a project in a single API request.</para>
        /// <para>Note: (This can only be used if the project is longitudinal with more than one arm.) NOTE: If the arm parameter is not provided, the specified records will be deleted from all arms in which they exist. Whereas, if arm is provided, they will only be deleted from the specified arm.</para>
        /// <para>Permissions Required: To use this method, you must have 'Delete Record' user privileges in the project.</para>
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">record</param>
        /// <param name="action">delete</param>
        /// <param name="records">an array of record names specifying specific records you wish to delete</param>
        /// <param name="arm">the arm number of the arm in which the record(s) should be deleted. 
        /// <returns>the number of records deleted.</returns>
        Task<string> DeleteRecordsAsync(string token, Content content, RedcapAction action, string[] records, int? arm);
        /// <summary>
        /// <para>API Version 1.0.0+</para>
        /// <para>Delete Records</para>
        /// <para>This method allows you to delete one or more records from a project in a single API request.</para>
        /// <para>Note: (This can only be used if the project is longitudinal with more than one arm.) NOTE: If the arm parameter is not provided, the specified records will be deleted from all arms in which they exist. Whereas, if arm is provided, they will only be deleted from the specified arm.</para>
        /// <para>Permissions Required: To use this method, you must have 'Delete Record' user privileges in the project.</para>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="records"></param>
        /// <param name="arm"></param>
        /// <returns>the number of records deleted.</returns>
        Task<string> DeleteRecordsAsync(string token, string[] records, int? arm);
        Task<string> ExportArmsAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat);
        Task<string> ExportArmsAsync(string token, ReturnFormat returnFormat = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportArmsAsync(string token, Content content, ReturnFormat returnFormat = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportEventsAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ExportEventsAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat = OnErrorFormat.json, int[] arms = null);
        Task<string> ExportEventsAsync(string token, Content content = Content.Event, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportEventsAsync(string token, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportFieldNamesAsync(string token, Content content = Content.ExportFieldNames, ReturnFormat format = ReturnFormat.json, string field = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportFieldNamesAsync(string token, ReturnFormat format = ReturnFormat.json, string field = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance = "1", OnErrorFormat onErrorFormat = OnErrorFormat.json, string filePath = null);
        Task<string> ExportFileAsync(string record, string field, string eventName, string repeatInstance, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ExportFileAsync(string record, string field, string eventName, string repeatInstance, string filePath = null, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ExportFileAsync(string token, string record, string field, string eventName, string repeatInstance = "1", OnErrorFormat onErrorFormat = OnErrorFormat.json, string filePath = null);
        Task<string> ExportInstrumentMappingAsync(string token, Content content = Content.FormEventMapping, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportInstrumentMappingAsync(string token, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportInstrumentsAsync(string token, Content content = Content.Instrument, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportInstrumentsAsync(string token, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportMetaDataAsync(ReturnFormat? inputFormat, OnErrorFormat? returnFormat);
        Task<string> ExportMetaDataAsync(ReturnFormat? inputFormat, OnErrorFormat? returnFormat, char[] delimiters, string fields = "", string forms = "");
        Task<string> ExportMetaDataAsync(string token, Content content = Content.MetaData, ReturnFormat format = ReturnFormat.json, string[] fields = null, string[] forms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportMetaDataAsync(string token, ReturnFormat format = ReturnFormat.json, string[] fields = null, string[] forms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportPDFInstrumentsAsync(string token, Content content = Content.Pdf, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportPDFInstrumentsAsync(string token, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportPDFInstrumentsAsync(string token, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, string filePath = null, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportProjectInfoAsync(string token, Content content = Content.Project, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportProjectInfoAsync(string token, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportProjectXmlAsync(string token, bool returnMetadataOnly = false, string[] records = null, string[] fields = null, string[] events = null, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null, bool exportFiles = false);
        Task<string> ExportProjectXmlAsync(string token, Content content, bool returnMetadataOnly = false, string[] records = null, string[] fields = null, string[] events = null, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null, bool exportFiles = false);
        Task<string> ExportRecordAsync(string token, Content content, string record, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null);
        Task<string> ExportRecordAsync(string record, ReturnFormat inputFormat, RedcapDataType redcapDataType, OnErrorFormat returnFormat = OnErrorFormat.json, char[] delimiters = null, string forms = null, string events = null, string fields = null);
        Task<string> ExportRecordsAsync(ReturnFormat inputFormat, RedcapDataType redcapDataType, OnErrorFormat returnFormat = OnErrorFormat.json, char[] delimiters = null, string forms = null, string events = null, string fields = null);
        Task<string> ExportRecordsAsync(string token, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] records = null, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null);
        Task<string> ExportRecordsAsync(string token, Content content, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] records = null, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null);
        Task<string> ExportRecordsAsync(string records, ReturnFormat inputFormat, RedcapDataType redcapDataType, OnErrorFormat returnFormat = OnErrorFormat.json, char[] delimiters = null, string forms = null, string events = null, string fields = null);
        Task<string> ExportRedcapVersionAsync(ReturnFormat inputFormat, RedcapDataType redcapDataType);
        Task<string> ExportRedcapVersionAsync(string token, Content content = Content.Version, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportRedcapVersionAsync(string token, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportRepeatingInstrumentsAndEvents(string token, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportRepeatingInstrumentsAndEvents(string token, Content content, ReturnFormat format = ReturnFormat.json);
        Task<string> ExportReportsAsync(string token, Content content, int reportId, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false);
        Task<string> ExportReportsAsync(string token, int reportId, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false);
        Task<string> ExportSurveyLinkAsync(string token, Content content, string record, string instrument, string eventName, int repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyLinkAsync(string token, string record, string instrument, string eventName, int repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyParticipantsAsync(string token, Content content, string instrument, string eventName, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyParticipantsAsync(string token, string instrument, string eventName, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyQueueLinkAsync(string token, Content content, string record, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyQueueLinkAsync(string token, string record, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyReturnCodeAsync(string token, Content content, string record, string instrument, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportSurveyReturnCodeAsync(string token, string record, string instrument, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportUsersAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ExportUsersAsync(string token, Content content = Content.User, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ExportUsersAsync(string token, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> GenerateNextRecordNameAsync(string token);
        Task<string> GenerateNextRecordNameAsync(string token, Content content);
        Task<string> GetMetaDataAsync(ReturnFormat? inputFormat, OnErrorFormat? returnFormat);
        Task<string> GetMetaDataAsync(ReturnFormat? inputFormat, OnErrorFormat? returnFormat, char[] delimiters, string fields = "", string forms = "");
        Task<string> GetRecordAsync(string record, ReturnFormat inputFormat, OnErrorFormat returnFormat, RedcapDataType redcapDataType, char[] delimiters);
        Task<string> GetRecordAsync(string record, ReturnFormat inputFormat, RedcapDataType redcapDataType, OnErrorFormat returnFormat = OnErrorFormat.json, char[] delimiters = null, string forms = null, string events = null, string fields = null);
        Task<string> GetRecordsAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat, RedcapDataType redcapDataType);
        Task<string> GetRecordsAsync(ReturnFormat inputFormat, OnErrorFormat returnFormat, RedcapDataType redcapDataType, char[] delimiters);
        Task<string> GetRedcapVersionAsync(ReturnFormat inputFormat, RedcapDataType redcapDataType);
        Task<string> ImportArmsAsync<T>(List<T> data, Override overRide, ReturnFormat inputFormat, OnErrorFormat returnFormat);
        Task<string> ImportArmsAsync<T>(string token, Content content, Override overrideBhavior, RedcapAction action, ReturnFormat format, List<T> data, OnErrorFormat returnFormat);
        Task<string> ImportArmsAsync<T>(string token, Override overrideBhavior, RedcapAction action, ReturnFormat format, List<T> data, OnErrorFormat returnFormat);
        Task<string> ImportEventsAsync<T>(List<T> data, Override overRide, ReturnFormat inputFormat, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ImportEventsAsync<T>(string token, Content content, RedcapAction action, Override overRideBehavior, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportEventsAsync<T>(string token, Override overRideBehavior, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance, string fileName, string filePath, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportFileAsync(string record, string field, string eventName, string repeatInstance, string fileName, string filePath, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> ImportFileAsync(string token, string record, string field, string eventName, string repeatInstance, string fileName, string filePath, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportInstrumentMappingAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportInstrumentMappingAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportMetaDataAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportMetaDataAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportProjectInfoAsync(string token, Content content, ReturnFormat format, RedcapProjectInfo projectInfo);
        Task<string> ImportProjectInfoAsync(string token, ReturnFormat format, RedcapProjectInfo projectInfo);
        Task<string> ImportRecordsAsync(object data, ReturnContent returnContent, OverwriteBehavior overwriteBehavior, ReturnFormat? inputFormat, RedcapDataType? redcapDataType, OnErrorFormat? returnFormat, string dateFormat = "MDY");
        Task<string> ImportRecordsAsync(object data, ReturnContent returnContent, OverwriteBehavior overwriteBehavior, ReturnFormat? inputFormat, RedcapDataType? redcapDataType, OnErrorFormat? returnFormat, string apiToken, string dateFormat = "MDY");
        Task<string> ImportRecordsAsync<T>(string token, Content content, ReturnFormat format, RedcapDataType redcapDataType, OverwriteBehavior overwriteBehavior, bool forceAutoNumber, List<T> data, string dateFormat, ReturnContent returnContent = ReturnContent.count, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportRecordsAsync<T>(string token, ReturnFormat format, RedcapDataType redcapDataType, OverwriteBehavior overwriteBehavior, bool forceAutoNumber, List<T> data, string dateFormat, ReturnContent returnContent = ReturnContent.count, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportRepeatingInstrumentsAndEvents<T>(string token, List<T> data, Content content = Content.RepeatingFormsEvents, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportUsersAsync<T>(string token, Content content, List<T> data, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> ImportUsersAsync<T>(string token, List<T> data, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> SaveRecordsAsync(List<string> data, ReturnContent returnContent, OverwriteBehavior overwriteBehavior, ReturnFormat? inputFormat, RedcapDataType? redcapDataType, OnErrorFormat? returnFormat, string dateFormat = "MDY");
        Task<string> SaveRecordsAsync(object data, ReturnContent returnContent, OverwriteBehavior overwriteBehavior, ReturnFormat? inputFormat, RedcapDataType? redcapDataType, OnErrorFormat? returnFormat);
        Task<string> SaveRecordsAsync(object data, ReturnContent returnContent, OverwriteBehavior overwriteBehavior, ReturnFormat? inputFormat, RedcapDataType? redcapDataType, OnErrorFormat? returnFormat, string dateFormat = "MDY");
    }
}
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
        /// Test Summary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="content"></param>
        /// <param name="format"></param>
        /// <param name="data"></param>
        /// <param name="onErrorFormat"></param>
        /// <param name="odm"></param>
        /// <returns>
        /// Test Return
        /// </returns>
        Task<string> CreateProjectAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null);
        Task<string> CreateProjectAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null);
        Task<string> DeleteArmsAsync(string token, Content content, RedcapAction action, string[] arms);
        Task<string> DeleteArmsAsync(string token, string[] arms);
        Task<string> DeleteArmsAsync<T>(T data);
        Task<string> DeleteEventsAsync(string token, string[] events = null);
        Task<string> DeleteEventsAsync(string token, Content content, RedcapAction action, string[] events = null);
        Task<string> DeleteFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> DeleteFileAsync(string record, string field, string eventName, string repeatInstance, OnErrorFormat returnFormat = OnErrorFormat.json);
        Task<string> DeleteFileAsync(string token, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json);
        Task<string> DeleteRecordsAsync(string token, Content content, RedcapAction action, string[] records, int? arm);
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
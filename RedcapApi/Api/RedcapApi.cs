﻿using Newtonsoft.Json;
using VCU.Redcap.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VCU.Redcap.Interfaces;
using VCU.Redcap.Utilities;
using static System.String;

namespace VCU.Redcap
{
    /// <summary>
    /// This api interacts with redcap instances. https://project-redcap.org
    /// Go to your http://redcap_instance/api/help for Redcap Api documentations
    /// Author: Michael Tran tranpl@outlook.com, tranpl@vcu.edu
    /// </summary>
    public class RedcapApi : IRedcap
    {
        /// <summary>
        /// Redcap API URI
        /// Location of your redcap instance
        /// <list type="none">
        /// e.g https://localhost/redcap/api
        /// </list>
        /// </summary>
        private static Uri _uri;

        /// <summary>
        /// Constructor requires a valid URI.
        /// </summary>
        /// <example>
        /// https://localhost/redcap/api
        /// </example>
        /// <remarks>
        /// This is the default constructor for version 1.0+
        /// </remarks>
        /// 
        /// <param name="redcapApiUri">Redcap instance URI</param>
        /// <param name="useInsecureCertificates">Allows use of insecure certificates in HttpClient</param>
        public RedcapApi(string redcapApiUri, bool useInsecureCertificates = false)
        {
            _uri = new Uri(redcapApiUri);
            Utils.UseInsecureCertificate = useInsecureCertificates;
        }

        #region Arms
        /// <summary>
        /// API Version 1.0.0+ **
        /// From Redcap Version 4.7.0
        /// Export Arms
        /// This method allows you to export the Arms for a project
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="returnFormat">csv, json [default], xml</param>
        /// <param name="arms">an array of arm numbers that you wish to pull events for (by default, all events are pulled)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Arms for the project in the format specified(only ones with Events available)</returns>
        public async Task<string> ExportArmsAsync(string token, ReturnFormat returnFormat = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                /*
                 * Request payload
                 */
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Arm.GetDisplayName() },
                    { "format", returnFormat.GetDisplayName() }
                };
                // Optional
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());
                    }
                }
                // defaults to 'json'
                payload.Add("returnFormat", onErrorFormat.GetDisplayName());

                // Execute send request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+ **
        /// From Redcap Version 4.7.0
        /// Export Arms
        /// This method allows you to export the Arms for a project
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">arm</param>
        /// <param name="returnFormat">csv, json [default], xml</param>
        /// <param name="arms">e.g. ["1","2"] an array of arm numbers that you wish to pull events for (by default, all events are pulled)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Arms for the project in the format specified(only ones with Events available)</returns>
        public async Task<string> ExportArmsAsync(string token, Content content, ReturnFormat returnFormat = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                /*
                 * Request payload
                 */
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", returnFormat.GetDisplayName() }
                };
                // Optional
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());
                    }
                }
                // defaults to 'json'
                payload.Add("returnFormat", onErrorFormat.GetDisplayName());

                // Execute send request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Import Arms
        /// This method allows you to import Arms into a project or to rename existing Arms in a project. 
        /// You may use the parameter override=1 as a 'delete all + import' action in order to erase all existing Arms in the project while importing new Arms. 
        /// Notice: Because of the 'override' parameter's destructive nature, this method may only use override=1 for projects in Development status.
        /// NOTE: This only works for longitudinal projects. 
        /// 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="overrideBhavior">0 - false [default], 1 - true — You may use override=1 as a 'delete all + import' action in order to erase all existing Arms in the project while importing new Arms. If override=0, then you can only add new Arms or rename existing ones. </param>
        /// <param name="action">import</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">Contains the attributes 'arm_num' (referring to the arm number) and 'name' (referring to the arm's name) of each arm to be created/modified, in which they are provided in the specified format. 
        /// [{"arm_num":"1","name":"Drug A"},
        /// {"arm_num":"2","name":"Drug B"},
        /// {"arm_num":"3","name":"Drug C"}]
        /// </param>
        /// <param name="returnFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <returns>Number of Arms imported</returns>
        public async Task<string> ImportArmsAsync<T>(string token, Override overrideBhavior, RedcapAction action, ReturnFormat format, List<T> data, OnErrorFormat returnFormat)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                    {
                        { "token", token },
                        { "content", Content.Arm.GetDisplayName() },
                        { "action", action.GetDisplayName() },
                        { "format", format.GetDisplayName() },
                        { "override", overrideBhavior.GetDisplayName() },
                        { "returnFormat", returnFormat.GetDisplayName() },
                        { "data", _serializedData }
                    };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Import Arms
        /// This method allows you to import Arms into a project or to rename existing Arms in a project. 
        /// You may use the parameter override=1 as a 'delete all + import' action in order to erase all existing Arms in the project while importing new Arms. 
        /// Notice: Because of the 'override' parameter's destructive nature, this method may only use override=1 for projects in Development status.
        /// NOTE: This only works for longitudinal projects. 
        /// 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">arm</param>
        /// <param name="overrideBhavior">0 - false [default], 1 - true — You may use override=1 as a 'delete all + import' action in order to erase all existing Arms in the project while importing new Arms. If override=0, then you can only add new Arms or rename existing ones. </param>
        /// <param name="action">import</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">Contains the attributes 'arm_num' (referring to the arm number) and 'name' (referring to the arm's name) of each arm to be created/modified, in which they are provided in the specified format. 
        /// [{"arm_num":"1","name":"Drug A"},
        /// {"arm_num":"2","name":"Drug B"},
        /// {"arm_num":"3","name":"Drug C"}]
        /// </param>
        /// <param name="returnFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <returns>Number of Arms imported</returns>
        public async Task<string> ImportArmsAsync<T>(string token, Content content, Override overrideBhavior, RedcapAction action, ReturnFormat format, List<T> data, OnErrorFormat returnFormat)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                    {
                        { "token", token },
                        { "content", content.GetDisplayName() },
                        { "action", action.GetDisplayName() },
                        { "format", format.GetDisplayName() },
                        { "override", overrideBhavior.GetDisplayName() },
                        { "returnFormat", returnFormat.GetDisplayName() },
                        { "data", _serializedData }
                    };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Delete Arms
        /// This method allows you to delete Arms from a project.
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. Additionally, please be aware that deleting an arm also automatically deletes all events that belong to that arm, and will also automatically delete any records/data that have been collected under that arm (this is non-reversible data loss).        
        /// NOTE: This only works for longitudinal projects. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="arms">an array of arm numbers that you wish to delete</param>
        /// <returns>Number of Arms deleted</returns>
        public async Task<string> DeleteArmsAsync(string token, string[] arms)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (arms.Length < 1)
                {
                    throw new InvalidOperationException($"No arm to delete, specify arm");

                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Arm.GetDisplayName() },
                    { "action", RedcapAction.Delete.GetDisplayName() }
                };
                // Required
                for (var i = 0; i < arms.Length; i++)
                {
                    payload.Add($"arms[{i}]", arms[i].ToString());

                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Delete Arms
        /// This method allows you to delete Arms from a project.
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. Additionally, please be aware that deleting an arm also automatically deletes all events that belong to that arm, and will also automatically delete any records/data that have been collected under that arm (this is non-reversible data loss).        
        /// NOTE: This only works for longitudinal projects. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">arm</param>
        /// <param name="action">delete</param>
        /// <param name="arms">an array of arm numbers that you wish to delete</param>
        /// <returns>Number of Arms deleted</returns>
        public async Task<string> DeleteArmsAsync(string token, Content content, RedcapAction action, string[] arms)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (arms.Length < 1)
                {
                    throw new InvalidOperationException($"No arm to delete, specify arm");

                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "action", action.GetDisplayName() }
                };
                // Required
                for (var i = 0; i < arms.Length; i++)
                {
                    payload.Add($"arms[{i}]", arms[i].ToString());

                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Arms
        #region Events

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Export Events
        /// This method allows you to export the events for a project
        /// NOTE: This only works for longitudinal projects.
        /// 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">
        /// The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.
        /// </param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="arms"></param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Events for the project in the format specified</returns>
        public async Task<string> ExportEventsAsync(string token, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (arms.Length < 1)
                {
                    throw new InvalidOperationException($"Please specify the arm you wish to export the events from.");

                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Event.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());

                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0
        /// 
        /// Export Events
        /// This method allows you to export the events for a project
        /// NOTE: This only works for longitudinal projects.
        /// 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">
        /// The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.
        /// </param>
        /// <param name="content">event</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="arms"></param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Events for the project in the format specified</returns>
        public async Task<string> ExportEventsAsync(string token, Content content = Content.Event, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (arms.Length < 1)
                {
                    throw new InvalidOperationException($"Please specify the arm you wish to export the events from.");

                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());

                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0
        /// 
        /// Import Events
        /// This method allows you to import Events into a project or to update existing Events' attributes, such as the event name, days offset, etc. The unique event name of an Event cannot be changed because it is auto-generated by REDCap. Please note that the only way to update an existing Event is to provide the unique_event_name attribute, and if the unique_event_name attribute is missing for an Event being imported (when override=0), it will assume it to be a new Event that should be created. Notice: Because of the 'override' parameter's destructive nature, this method may only use override=1 for projects in Development status.
        /// NOTE: This only works for longitudinal projects. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="overRideBehavior">0 - false [default], 1 - true — You may use override=1 as a 'delete all + import' action in order to erase all existing Events in the project while importing new Events. If override=0, then you can only add new Events or modify existing ones. </param>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Contains the required attributes 'event_name' (referring to the name/label of the event) and 'arm_num' (referring to the arm number to which the event belongs - assumes '1' if project only contains one arm). In order to modify an existing event, you must provide the attribute 'unique_event_name' (referring to the auto-generated unique event name of the given event). If the project utilizes the Scheduling module, the you may optionally provide the following attributes, which must be numerical: day_offset, offset_min, offset_max. If the day_offset is not provided, then the events will be auto-numbered in the order in which they are provided in the API request. 
        /// [{"event_name":"Baseline","arm_num":"1","day_offset":"1","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"baseline_arm_1"},
        /// {"event_name":"Visit 1","arm_num":"1","day_offset":"2","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"visit_1_arm_1"},
        /// {"event_name":"Visit 2","arm_num":"1","day_offset":"3","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"visit_2_arm_1"}]
        /// </param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of Events imported</returns>
        public async Task<string> ImportEventsAsync<T>(string token, Override overRideBehavior, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (data.Count < 1)
                {
                    throw new InvalidOperationException($"Events can not be empty or null");
                }
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Event.GetDisplayName() },
                    { "action", RedcapAction.Import.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "override", overRideBehavior.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0
        /// 
        /// Import Events
        /// This method allows you to import Events into a project or to update existing Events' attributes, such as the event name, days offset, etc. The unique event name of an Event cannot be changed because it is auto-generated by REDCap. Please note that the only way to update an existing Event is to provide the unique_event_name attribute, and if the unique_event_name attribute is missing for an Event being imported (when override=0), it will assume it to be a new Event that should be created. Notice: Because of the 'override' parameter's destructive nature, this method may only use override=1 for projects in Development status.
        /// NOTE: This only works for longitudinal projects. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">event</param>
        /// <param name="action">import</param>
        /// <param name="overRideBehavior">0 - false [default], 1 - true — You may use override=1 as a 'delete all + import' action in order to erase all existing Events in the project while importing new Events. If override=0, then you can only add new Events or modify existing ones. </param>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Contains the required attributes 'event_name' (referring to the name/label of the event) and 'arm_num' (referring to the arm number to which the event belongs - assumes '1' if project only contains one arm). In order to modify an existing event, you must provide the attribute 'unique_event_name' (referring to the auto-generated unique event name of the given event). If the project utilizes the Scheduling module, the you may optionally provide the following attributes, which must be numerical: day_offset, offset_min, offset_max. If the day_offset is not provided, then the events will be auto-numbered in the order in which they are provided in the API request. 
        /// [{"event_name":"Baseline","arm_num":"1","day_offset":"1","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"baseline_arm_1"},
        /// {"event_name":"Visit 1","arm_num":"1","day_offset":"2","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"visit_1_arm_1"},
        /// {"event_name":"Visit 2","arm_num":"1","day_offset":"3","offset_min":"0",
        /// "offset_max":"0","unique_event_name":"visit_2_arm_1"}]
        /// </param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of Events imported</returns>
        public async Task<string> ImportEventsAsync<T>(string token, Content content, RedcapAction action, Override overRideBehavior, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (data.Count < 1)
                {
                    throw new InvalidOperationException($"Events can not be empty or null");
                }
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "action", action.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "override", overRideBehavior.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0
        /// 
        /// Delete Events
        /// This method allows you to delete Events from a project. 
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. 
        /// Additionally, please be aware that deleting an event will automatically delete any records/data that have been collected under that event (this is non-reversible data loss).
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        ///  
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="events">Array of unique event names</param>
        /// <returns>Number of Events deleted</returns>
        public async Task<string> DeleteEventsAsync(string token, string[] events = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (events.Length < 1)
                {
                    throw new InvalidOperationException($"No events to delete...");
                }

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Event.GetDisplayName() },
                    { "action", RedcapAction.Delete.GetDisplayName() }
                };
                // Required
                if (events?.Length > 0)
                {
                    for (var i = 0; i < events.Length; i++)
                    {
                        payload.Add($"events[{i}]", events[i]);

                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0
        /// 
        /// Delete Events
        /// This method allows you to delete Events from a project. 
        /// Notice: Because of this method's destructive nature, it is only available for use for projects in Development status. 
        /// Additionally, please be aware that deleting an event will automatically delete any records/data that have been collected under that event (this is non-reversible data loss).
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        ///  
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token"></param>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="events">Array of unique event names</param>
        /// <returns>Number of Events deleted</returns>
        public async Task<string> DeleteEventsAsync(string token, Content content, RedcapAction action, string[] events = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (events.Length < 1)
                {
                    throw new InvalidOperationException($"No events to delete...");
                }

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "action", action.GetDisplayName() }
                };
                // Required
                if (events?.Length > 0)
                {
                    for (var i = 0; i < events.Length; i++)
                    {
                        payload.Add($"events[{i}]", events[i]);

                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Events
        #region Field Names

        /// <summary>
        /// Export List of Export Field Names (i.e. variables used during exports and imports)
        /// 
        /// This method returns a list of the export/import-specific version of field names for all fields (or for one field, if desired) in a project. 
        /// This is mostly used for checkbox fields because during data exports and data imports, checkbox fields have a different variable name used than the exact one defined for them in the Online Designer and Data Dictionary, in which *each checkbox option* gets represented as its own export field name in the following format: field_name + triple underscore + converted coded value for the choice. 
        /// For non-checkbox fields, the export field name will be exactly the same as the original field name. 
        /// Note: The following field types will be automatically removed from the list returned by this method since they cannot be utilized during the data import process: 'calc', 'file', and 'descriptive'.
        /// 
        /// The list that is returned will contain the three following attributes for each field/choice: 'original_field_name', 'choice_value', and 'export_field_name'. 
        /// The choice_value attribute represents the raw coded value for a checkbox choice.For non-checkbox fields, the choice_value attribute will always be blank/empty.
        /// The export_field_name attribute represents the export/import-specific version of that field name.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="field">A field's variable name. By default, all fields are returned, but if field is provided, then it will only the export field name(s) for that field. If the field name provided is invalid, it will return an error.</param>
        /// <param name="onErrorFormat">csv, json [default], xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'. 
        /// The list that is returned will contain the original field name (variable) of the field and also the export field name(s) of that field.</param>
        /// <returns>Returns a list of the export/import-specific version of field names for all fields (or for one field, if desired) in a project in the format specified and ordered by their field order . 
        /// The list that is returned will contain the three following attributes for each field/choice: 'original_field_name', 'choice_value', and 'export_field_name'. The choice_value attribute represents the raw coded value for a checkbox choice. For non-checkbox fields, the choice_value attribute will always be blank/empty. The export_field_name attribute represents the export/import-specific version of that field name.
        /// </returns>
        public async Task<string> ExportFieldNamesAsync(string token, ReturnFormat format = ReturnFormat.json, string field = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.ExportFieldNames.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }

                };
                if (!IsNullOrEmpty(field))
                {
                    payload.Add("field", field);
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }


        /// <summary>
        /// Export List of Export Field Names (i.e. variables used during exports and imports)
        /// 
        /// This method returns a list of the export/import-specific version of field names for all fields (or for one field, if desired) in a project. 
        /// This is mostly used for checkbox fields because during data exports and data imports, checkbox fields have a different variable name used than the exact one defined for them in the Online Designer and Data Dictionary, in which *each checkbox option* gets represented as its own export field name in the following format: field_name + triple underscore + converted coded value for the choice. 
        /// For non-checkbox fields, the export field name will be exactly the same as the original field name. 
        /// Note: The following field types will be automatically removed from the list returned by this method since they cannot be utilized during the data import process: 'calc', 'file', and 'descriptive'.
        /// 
        /// The list that is returned will contain the three following attributes for each field/choice: 'original_field_name', 'choice_value', and 'export_field_name'. 
        /// The choice_value attribute represents the raw coded value for a checkbox choice.For non-checkbox fields, the choice_value attribute will always be blank/empty.
        /// The export_field_name attribute represents the export/import-specific version of that field name.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">exportFieldNames</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="field">A field's variable name. By default, all fields are returned, but if field is provided, then it will only the export field name(s) for that field. If the field name provided is invalid, it will return an error.</param>
        /// <param name="onErrorFormat">csv, json [default], xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'. 
        /// The list that is returned will contain the original field name (variable) of the field and also the export field name(s) of that field.</param>
        /// <returns>Returns a list of the export/import-specific version of field names for all fields (or for one field, if desired) in a project in the format specified and ordered by their field order . 
        /// The list that is returned will contain the three following attributes for each field/choice: 'original_field_name', 'choice_value', and 'export_field_name'. The choice_value attribute represents the raw coded value for a checkbox choice. For non-checkbox fields, the choice_value attribute will always be blank/empty. The export_field_name attribute represents the export/import-specific version of that field name.
        /// </returns>
        public async Task<string> ExportFieldNamesAsync(string token, Content content = Content.ExportFieldNames, ReturnFormat format = ReturnFormat.json, string field = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }

                };
                if (!IsNullOrEmpty(field))
                {
                    payload.Add("field", field);
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Field Names
        #region Files

        /// <summary>
        /// API Version 1.0.0+
        /// Export a File
        /// This method allows you to download a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API file export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then the API file export will fail and return an error *only if* the File Upload field has been tagged as an Identifier field.To make sure that your API request does not return an error, you should have 'Full Data Set' export rights in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <example>
        /// The MIME type of the file, along with the name of the file and its extension, can be found in the header of the returned response. Thus in order to determine these attributes of the file being exported, you will need to parse the response header. Example: content-type = application/vnd.openxmlformats-officedocument.wordprocessingml.document; name='FILE_NAME.docx'
        /// </example>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <param name="filePath">File path which the file will be saved.</param>
        /// <returns>the contents of the file</returns>
        public async Task<string> ExportFileAsync(string token, string record, string field, string eventName, string repeatInstance = "1", OnErrorFormat onErrorFormat = OnErrorFormat.json, string filePath = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException($"Must contain a file path to save the file.");
                }
                /*
                 * FilePath check..
                 */
                if (!Directory.Exists(filePath) && !IsNullOrEmpty(filePath))
                {
                    Log.Warning($"The directory provided does not exist! Creating a folder for you.");
                    Directory.CreateDirectory(filePath);
                }
                if (IsNullOrEmpty(record))
                {
                    throw new InvalidOperationException($"No record provided to export");
                }
                if (IsNullOrEmpty(field) || IsNullOrEmpty(eventName))
                {
                    throw new InvalidOperationException($"No field provided to export");
                }
                if (IsNullOrEmpty(eventName))
                {
                    throw new InvalidOperationException($"No eventName provided to export");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.File.GetDisplayName() },
                    { "action", RedcapAction.Export.GetDisplayName() },
                    { "record", record },
                    { "field", field },
                    { "event", eventName },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "repeat_instance", repeatInstance  }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export a File
        /// **Allows for file download to a path.**
        /// This method allows you to download a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API file export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then the API file export will fail and return an error *only if* the File Upload field has been tagged as an Identifier field.To make sure that your API request does not return an error, you should have 'Full Data Set' export rights in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <example>
        /// The MIME type of the file, along with the name of the file and its extension, can be found in the header of the returned response. Thus in order to determine these attributes of the file being exported, you will need to parse the response header. Example: content-type = application/vnd.openxmlformats-officedocument.wordprocessingml.document; name='FILE_NAME.docx'
        /// </example>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">file</param>
        /// <param name="action">export</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <param name="filePath">File path which the file will be saved.</param>
        /// <returns>the file name that was exported</returns>
        public async Task<string> ExportFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance = "1", OnErrorFormat onErrorFormat = OnErrorFormat.json, string filePath = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                if (IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException($"Must contain a file path to save the file.");
                }
                /*
                 * FilePath check..
                 */
                if (!Directory.Exists(filePath) && !IsNullOrEmpty(filePath))
                {
                    Log.Warning($"The directory provided does not exist! Creating a folder for you.");
                    Directory.CreateDirectory(filePath);
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "action", action.GetDisplayName() },
                    { "record", record },
                    { "field", field },
                    { "event", eventName },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "filePath", $@"{filePath}" }
                };
                // Optional
                if (!IsNullOrEmpty(repeatInstance))
                {
                    payload.Add("repeat_instance", repeatInstance);
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Import a File
        /// This method allows you to upload a document that will be attached to an individual record for a File Upload field. Please note that this method may NOT be used for Signature fields (i.e. File Upload fields with 'signature' validation type) because a signature can only be captured and stored using the web interface. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// If you pass in a record parameter that does not exist, Redcap will create it for you.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param> 
        /// <param name="fileName">The File you be imported, contents of the file</param>
        /// <param name="filePath">the path where the file is located</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <returns>csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</returns>
        public async Task<string> ImportFileAsync(string token, string record, string field, string eventName, string repeatInstance, string fileName, string filePath, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _fileName = fileName;
                var _filePath = filePath;
                var _binaryFile = Path.Combine(_filePath, _fileName);
                ByteArrayContent _fileContent;
                var payload = new MultipartFormDataContent()
                {
                        {new StringContent(token), "token" },
                        {new StringContent(Content.File.GetDisplayName()) ,"content" },
                        {new StringContent(RedcapAction.Import.GetDisplayName()), "action" },
                        {new StringContent(record), "record" },
                        {new StringContent(field), "field" },
                        {new StringContent(eventName),  "event" },
                        {new StringContent(onErrorFormat.GetDisplayName()), "returnFormat" }
                };
                if (!IsNullOrEmpty(repeatInstance))
                {
                    // add repeat instrument params if available
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");
                }
                else
                {
                    repeatInstance = "1";
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");

                }
                if (IsNullOrEmpty(_fileName) || IsNullOrEmpty(_filePath))
                {

                    throw new InvalidOperationException($"file can not be empty or null");
                }
                else
                {
                    // add the binary file in specific content type
                    _fileContent = new ByteArrayContent(File.ReadAllBytes(_binaryFile));
                    _fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    payload.Add(_fileContent, "file", _fileName);
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Import a File
        /// This method allows you to upload a document that will be attached to an individual record for a File Upload field. Please note that this method may NOT be used for Signature fields (i.e. File Upload fields with 'signature' validation type) because a signature can only be captured and stored using the web interface. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// If you pass in a record parameter that does not exist, Redcap will create it for you.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">file</param>
        /// <param name="action">import</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param> 
        /// <param name="fileName">The File you be imported, contents of the file</param>
        /// <param name="filePath">the path where the file is located</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'xml'.</param>
        /// <returns>csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</returns>
        public async Task<string> ImportFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance, string fileName, string filePath, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _fileName = fileName;
                var _filePath = filePath;
                var _binaryFile = Path.Combine(_filePath, _fileName);
                ByteArrayContent _fileContent;
                var payload = new MultipartFormDataContent()
                {
                        {new StringContent(token), "token" },
                        {new StringContent(content.GetDisplayName()) ,"content" },
                        {new StringContent(action.GetDisplayName()), "action" },
                        {new StringContent(record), "record" },
                        {new StringContent(field), "field" },
                        {new StringContent(eventName),  "event" },
                        {new StringContent(onErrorFormat.GetDisplayName()), "returnFormat" }
                };
                if (!IsNullOrEmpty(repeatInstance))
                {
                    // add repeat instrument params if available
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");
                }
                else
                {
                    repeatInstance = "1";
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");

                }
                if (IsNullOrEmpty(_fileName) || IsNullOrEmpty(_filePath))
                {

                    throw new InvalidOperationException($"file can not be empty or null");
                }
                else
                {
                    // add the binary file in specific content type
                    _fileContent = new ByteArrayContent(File.ReadAllBytes(_binaryFile));
                    _fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    payload.Add(_fileContent, "file", _fileName);
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Delete a File
        /// This method allows you to remove a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>String</returns>
        public async Task<string> DeleteFileAsync(string token, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new MultipartFormDataContent()
                {
                    {new StringContent(token), "token" },
                    {new StringContent(Content.File.GetDisplayName()) ,"content" },
                    {new StringContent(RedcapAction.Delete.GetDisplayName()), "action" },
                    {new StringContent(record), "record" },
                    {new StringContent(field), "field" },
                    {new StringContent(eventName),  "event" },
                    {new StringContent(onErrorFormat.GetDisplayName()), "returnFormat" }
                };
                if (!IsNullOrEmpty(repeatInstance))
                {
                    // add repeat instrument params if available
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");
                }
                else
                {
                    repeatInstance = "1";
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");

                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Delete a File
        /// This method allows you to remove a document that has been attached to an individual record for a File Upload field. Please note that this method may also be used for Signature fields (i.e. File Upload fields with 'signature' validation type).
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">file</param>
        /// <param name="action">delete</param>
        /// <param name="record">the record ID</param>
        /// <param name="field">the name of the field that contains the file</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>String</returns>
        public async Task<string> DeleteFileAsync(string token, Content content, RedcapAction action, string record, string field, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new MultipartFormDataContent()
                {
                    {new StringContent(token), "token" },
                    {new StringContent(content.GetDisplayName()) ,"content" },
                    {new StringContent(action.GetDisplayName()), "action" },
                    {new StringContent(record), "record" },
                    {new StringContent(field), "field" },
                    {new StringContent(eventName),  "event" },
                    {new StringContent(onErrorFormat.GetDisplayName()), "returnFormat" }
                };
                if (!IsNullOrEmpty(repeatInstance))
                {
                    // add repeat instrument params if available
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");
                }
                else
                {
                    repeatInstance = "1";
                    payload.Add(new StringContent(repeatInstance), "repeat_instance");

                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Files
        #region Instruments

        /// <summary>
        /// API Version 1.0.0+
        /// Export Instruments (Data Entry Forms)
        /// This method allows you to export a list of the data collection instruments for a project. 
        /// This includes their unique instrument name as seen in the second column of the Data Dictionary, as well as each instrument's corresponding instrument label, which is seen on a project's left-hand menu when entering data. The instruments will be ordered according to their order in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <returns>Instruments for the project in the format specified and will be ordered according to their order in the project.</returns>
        public async Task<string> ExportInstrumentsAsync(string token, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Instrument.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export Instruments (Data Entry Forms)
        /// This method allows you to export a list of the data collection instruments for a project. 
        /// This includes their unique instrument name as seen in the second column of the Data Dictionary, as well as each instrument's corresponding instrument label, which is seen on a project's left-hand menu when entering data. The instruments will be ordered according to their order in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">instrument</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <returns>Instruments for the project in the format specified and will be ordered according to their order in the project.</returns>
        public async Task<string> ExportInstrumentsAsync(string token, Content content = Content.Instrument, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export PDF file of Data Collection Instruments (either as blank or with data)
        /// This method allows you to export a PDF file for any of the following: 1) a single data collection instrument (blank), 2) all instruments (blank), 3) a single instrument (with data from a single record), 4) all instruments (with data from a single record), or 5) all instruments (with data from ALL records). 
        /// This is the exact same PDF file that is downloadable from a project's data entry form in the web interface, and additionally, the user's privileges with regard to data exports will be applied here just like they are when downloading the PDF in the web interface (e.g., if they have de-identified data export rights, then it will remove data from certain fields in the PDF). 
        /// If the user has 'No Access' data export rights, they will not be able to use this method, and an error will be returned.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="recordId">the record ID. The value is blank by default. If record is blank, it will return the PDF as blank (i.e. with no data). If record is provided, it will return a single instrument or all instruments containing data from that record only.</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects. For a longitudinal project, if record is not blank and event is blank, it will return data for all events from that record. If record is not blank and event is not blank, it will return data only for the specified event from that record.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. The value is blank by default, which returns all instruments. If record is not blank and instrument is blank, it will return all instruments for that record.</param>
        /// <param name="allRecord">[The value of this parameter does not matter and is ignored.] If this parameter is passed with any value, it will export all instruments (and all events, if longitudinal) with data from all records. Note: If this parameter is passed, the parameters record, event, and instrument will be ignored.</param>
        /// <param name="onErrorFormat">csv, json [default] , xml- The returnFormat is only used with regard to the format of any error messages that might be returned.</param>
        /// <returns>A PDF file containing one or all data collection instruments from the project, in which the instruments will be blank (no data), contain data from a single record, or contain data from all records in the project, depending on the parameters passed in the API request.</returns>
        public async Task<string> ExportPDFInstrumentsAsync(string token, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Pdf.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Add all optional parameters
                if (!IsNullOrEmpty(recordId))
                {
                    payload.Add("record", recordId);
                }
                if (!IsNullOrEmpty(eventName))
                {
                    payload.Add("event", eventName);
                }
                if (!IsNullOrEmpty(instrument))
                {
                    payload.Add("instrument", instrument);
                }
                if (allRecord)
                {
                    payload.Add("allRecords", allRecord.ToString());
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export PDF file of Data Collection Instruments (either as blank or with data)
        /// This method allows you to export a PDF file for any of the following: 1) a single data collection instrument (blank), 2) all instruments (blank), 3) a single instrument (with data from a single record), 4) all instruments (with data from a single record), or 5) all instruments (with data from ALL records). 
        /// This is the exact same PDF file that is downloadable from a project's data entry form in the web interface, and additionally, the user's privileges with regard to data exports will be applied here just like they are when downloading the PDF in the web interface (e.g., if they have de-identified data export rights, then it will remove data from certain fields in the PDF). 
        /// If the user has 'No Access' data export rights, they will not be able to use this method, and an error will be returned.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">pdf</param>
        /// <param name="recordId">the record ID. The value is blank by default. If record is blank, it will return the PDF as blank (i.e. with no data). If record is provided, it will return a single instrument or all instruments containing data from that record only.</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects. For a longitudinal project, if record is not blank and event is blank, it will return data for all events from that record. If record is not blank and event is not blank, it will return data only for the specified event from that record.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. The value is blank by default, which returns all instruments. If record is not blank and instrument is blank, it will return all instruments for that record.</param>
        /// <param name="allRecord">[The value of this parameter does not matter and is ignored.] If this parameter is passed with any value, it will export all instruments (and all events, if longitudinal) with data from all records. Note: If this parameter is passed, the parameters record, event, and instrument will be ignored.</param>
        /// <param name="onErrorFormat">csv, json [default] , xml- The returnFormat is only used with regard to the format of any error messages that might be returned.</param>
        /// <returns>A PDF file containing one or all data collection instruments from the project, in which the instruments will be blank (no data), contain data from a single record, or contain data from all records in the project, depending on the parameters passed in the API request.</returns>
        public async Task<string> ExportPDFInstrumentsAsync(string token, Content content = Content.Pdf, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Add all optional parameters
                if (!IsNullOrEmpty(recordId))
                {
                    payload.Add("record", recordId);
                }
                if (!IsNullOrEmpty(eventName))
                {
                    payload.Add("event", eventName);
                }
                if (!IsNullOrEmpty(instrument))
                {
                    payload.Add("instrument", instrument);
                }
                if (allRecord)
                {
                    payload.Add("allRecords", allRecord.ToString());
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// **Allows for file download to a path.**
        /// Export PDF file of Data Collection Instruments (either as blank or with data)
        /// This method allows you to export a PDF file for any of the following: 1) a single data collection instrument (blank), 2) all instruments (blank), 3) a single instrument (with data from a single record), 4) all instruments (with data from a single record), or 5) all instruments (with data from ALL records). 
        /// This is the exact same PDF file that is downloadable from a project's data entry form in the web interface, and additionally, the user's privileges with regard to data exports will be applied here just like they are when downloading the PDF in the web interface (e.g., if they have de-identified data export rights, then it will remove data from certain fields in the PDF). 
        /// If the user has 'No Access' data export rights, they will not be able to use this method, and an error will be returned.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="recordId">the record ID. The value is blank by default. If record is blank, it will return the PDF as blank (i.e. with no data). If record is provided, it will return a single instrument or all instruments containing data from that record only.</param>
        /// <param name="eventName">the unique event name - only for longitudinal projects. For a longitudinal project, if record is not blank and event is blank, it will return data for all events from that record. If record is not blank and event is not blank, it will return data only for the specified event from that record.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. The value is blank by default, which returns all instruments. If record is not blank and instrument is blank, it will return all instruments for that record.</param>
        /// <param name="allRecord">[The value of this parameter does not matter and is ignored.] If this parameter is passed with any value, it will export all instruments (and all events, if longitudinal) with data from all records. Note: If this parameter is passed, the parameters record, event, and instrument will be ignored.</param>
        /// <param name="filePath">the path where the file is located</param>
        /// <param name="onErrorFormat">csv, json [default] , xml- The returnFormat is only used with regard to the format of any error messages that might be returned.</param>
        /// <returns>A PDF file containing one or all data collection instruments from the project, in which the instruments will be blank (no data), contain data from a single record, or contain data from all records in the project, depending on the parameters passed in the API request.</returns>
        public async Task<string> ExportPDFInstrumentsAsync(string token, string recordId = null, string eventName = null, string instrument = null, bool allRecord = false, string filePath = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                /*
                 * FilePath check..
                 */
                if (!Directory.Exists(filePath) && !IsNullOrEmpty(filePath))
                {
                    Log.Warning($"The directory provided does not exist! Creating a folder for you.");
                    Directory.CreateDirectory(filePath);
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Pdf.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "filePath", $@"{filePath}" }
                };
                // Add all optional parameters
                if (!IsNullOrEmpty(recordId))
                {
                    payload.Add("record", recordId);
                }
                if (!IsNullOrEmpty(eventName))
                {
                    payload.Add("event", eventName);
                }
                if (!IsNullOrEmpty(instrument))
                {
                    payload.Add("instrument", instrument);
                }
                if (allRecord)
                {
                    payload.Add("allRecords", allRecord.ToString());
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Export Instrument-Event Mappings
        /// This method allows you to export the instrument-event mappings for a project (i.e., how the data collection instruments are designated for certain events in a longitudinal project).
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="arms">an array of arm numbers that you wish to pull events for (by default, all events are pulled)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Instrument-event mappings for the project in the format specified</returns>
        public async Task<string> ExportInstrumentMappingAsync(string token, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.FormEventMapping.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Add all optional parameters
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());
                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Export Instrument-Event Mappings
        /// This method allows you to export the instrument-event mappings for a project (i.e., how the data collection instruments are designated for certain events in a longitudinal project).
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">formEventMapping</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="arms">an array of arm numbers that you wish to pull events for (by default, all events are pulled)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Instrument-event mappings for the project in the format specified</returns>
        public async Task<string> ExportInstrumentMappingAsync(string token, Content content = Content.FormEventMapping, ReturnFormat format = ReturnFormat.json, string[] arms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Add all optional parameters
                if (arms?.Length > 0)
                {
                    for (var i = 0; i < arms.Length; i++)
                    {
                        payload.Add($"arms[{i}]", arms[i].ToString());
                    }
                }
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// FFrom Redcap Version 4.7.0
        /// 
        /// Import Instrument-Event Mappings
        /// This method allows you to import Instrument-Event Mappings into a project (this corresponds to the 'Designate Instruments for My Events' page in the project). 
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Contains the attributes 'arm_num' (referring to the arm number), 'unique_event_name' (referring to the auto-generated unique event name of the given event), and 'form' (referring to the unique form name of the given data collection instrument), in which they are provided in the specified format. 
        /// JSON Example:[{"arm_num":"1","unique_event_name":"baseline_arm_1","form":"demographics"},
        /// {"arm_num":"1","unique_event_name":"visit_1_arm_1","form":"day_3"},
        /// {"arm_num":"1","unique_event_name":"visit_1_arm_1","form":"other"},
        /// {"arm_num":"1","unique_event_name":"visit_2_arm_1","form":"other"}]
        /// </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of Instrument-Event Mappings imported</returns>
        public async Task<string> ImportInstrumentMappingAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.FormEventMapping.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0 
        /// 
        /// Import Instrument-Event Mappings
        /// This method allows you to import Instrument-Event Mappings into a project (this corresponds to the 'Designate Instruments for My Events' page in the project). 
        /// NOTE: This only works for longitudinal projects.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">formEventMapping</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Contains the attributes 'arm_num' (referring to the arm number), 'unique_event_name' (referring to the auto-generated unique event name of the given event), and 'form' (referring to the unique form name of the given data collection instrument), in which they are provided in the specified format. 
        /// JSON Example:[{"arm_num":"1","unique_event_name":"baseline_arm_1","form":"demographics"},
        /// {"arm_num":"1","unique_event_name":"visit_1_arm_1","form":"day_3"},
        /// {"arm_num":"1","unique_event_name":"visit_1_arm_1","form":"other"},
        /// {"arm_num":"1","unique_event_name":"visit_2_arm_1","form":"other"}]
        /// </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of Instrument-Event Mappings imported</returns>
        public async Task<string> ImportInstrumentMappingAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                // Execute request
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Instruments
        #region Metadata
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 3.4.0+
        /// Export Metadata (Data Dictionary)
        /// This method allows you to export the metadata for a project
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">metadata</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all metadata is pulled)</param>
        /// <param name="forms">an array of form names specifying specific data collection instruments for which you wish to pull metadata (by default, all metadata is pulled). NOTE: These 'forms' are not the form label values that are seen on the webpages, but instead they are the unique form names seen in Column B of the data dictionary.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Metadata from the project (i.e. Data Dictionary values) in the format specified ordered by the field order</returns>
        public async Task<string> ExportMetaDataAsync(string token, Content content = Content.MetaData, ReturnFormat format = ReturnFormat.json, string[] fields = null, string[] forms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (fields?.Length > 0)
                {
                    for (var i = 0; i < fields.Length; i++)
                    {
                        payload.Add($"fields[{i}]", fields[i].ToString());
                    }
                }
                if (forms?.Length > 0)
                {
                    for (var i = 0; i < forms.Length; i++)
                    {
                        payload.Add($"forms[{i}]", forms[i].ToString());
                    }
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 3.4.0+
        /// Export Metadata (Data Dictionary)
        /// This method allows you to export the metadata for a project
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all metadata is pulled)</param>
        /// <param name="forms">an array of form names specifying specific data collection instruments for which you wish to pull metadata (by default, all metadata is pulled). NOTE: These 'forms' are not the form label values that are seen on the webpages, but instead they are the unique form names seen in Column B of the data dictionary.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Metadata from the project (i.e. Data Dictionary values) in the format specified ordered by the field order</returns>
        public async Task<string> ExportMetaDataAsync(string token, ReturnFormat format = ReturnFormat.json, string[] fields = null, string[] forms = null, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.MetaData.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (fields?.Length > 0)
                {
                    for (var i = 0; i < fields.Length; i++)
                    {
                        payload.Add($"fields[{i}]", fields[i].ToString());
                    }
                }
                if (forms?.Length > 0)
                {
                    for (var i = 0; i < forms.Length; i++)
                    {
                        payload.Add($"forms[{i}]", forms[i].ToString());
                    }
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0 
        /// 
        /// Import Metadata (Data Dictionary)
        /// 
        /// This method allows you to import metadata (i.e., Data Dictionary) into a project. Notice: Because of this method's destructive nature, it is only available for use for projects in Development status.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">The formatted data to be imported.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of fields imported</returns>
        public async Task<string> ImportMetaDataAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.MetaData.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0 
        /// 
        /// Import Metadata (Data Dictionary)
        /// 
        /// This method allows you to import metadata (i.e., Data Dictionary) into a project. Notice: Because of this method's destructive nature, it is only available for use for projects in Development status.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* Project Design/Setup privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">metadata</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">The formatted data to be imported.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of fields imported</returns>
        public async Task<string> ImportMetaDataAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Metadata
        #region Projects
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0 
        /// 
        /// Create A New Project
        /// 
        /// This method allows you to create a new REDCap project. A 64-character Super API Token is required for this method (as opposed to project-level API methods that require a regular 32-character token associated with the project-user). In the API request, you must minimally provide the project attributes 'project_title' and 'purpose' (with numerical value 0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support) when creating a project. 
        /// When a project is created with this method, the project will automatically be given all the project-level defaults just as if you created a new empty project via the web user interface, such as a automatically creating a single data collection instrument seeded with a single Record ID field and Form Status field, as well as (for longitudinal projects) one arm with one event. And if you intend to create your own arms or events immediately after creating the project, it is recommended that you utilize the override=1 parameter in the 'Import Arms' or 'Import Events' method, respectively, so that the default arm and event are removed when you add your own.Also, the user creating the project will automatically be added to the project as a user with full user privileges and a project-level API token, which could then be used for subsequent project-level API requests.
        /// NOTE: Only users with Super API Tokens can utilize this method.Users can only be granted a super token by a REDCap administrator(using the API Tokens page in the REDCap Control Center). Please be advised that users with a Super API Token can create new REDCap projects via the API without any approval needed by a REDCap administrator.If you are interested in obtaining a super token, please contact your local REDCap administrator.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have a Super API Token.
        /// </remarks>
        /// To use this method, you must have a Super API Token.
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The Super API Token specific to a user</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">
        /// Contains the attributes of the project to be created, in which they are provided in the specified format. While the only required attributes are 'project_title' and 'purpose', the fields listed below are all the possible attributes that can be provided in the 'data' parameter. The 'purpose' attribute must have a numerical value (0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support), in which 'purpose_other' is only required to have a value (as a text string) if purpose=1. The attributes is_longitudinal (0=False, 1=True; Default=0), surveys_enabled (0=False, 1=True; Default=0), and record_autonumbering_enabled (0=False, 1=True; Default=1) are all boolean. Please note that either is_longitudinal=1 or surveys_enabled=1 does not add arms/events or surveys to the project, respectively, but it merely enables those settings which are seen at the top of the project's Project Setup page.
        /// All available attributes:
        /// project_title, purpose, purpose_other, project_notes, is_longitudinal, surveys_enabled, record_autonumbering_enabled
        /// JSON Example:
        /// [{"project_title":"My New REDCap Project","purpose":"0"}]
        /// </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="odm">default: NULL - The 'odm' parameter must be an XML string in CDISC ODM XML format that contains project metadata (fields, forms, events, arms) and might optionally contain data to be imported as well. The XML contained in this parameter can come from a REDCap Project XML export file from REDCap itself, or may come from another system that is capable of exporting projects and data in CDISC ODM format. If the 'odm' parameter is included in the API request, it will use the XML to import its contents into the newly created project. This will allow you not only to create the project with the API request, but also to import all fields, forms, and project attributes (and events and arms, if longitudinal) as well as record data all at the same time.</param>
        /// <returns>When a project is created, a 32-character project-level API Token is returned (associated with both the project and user creating the project). This token could then ostensibly be used to make subsequent API calls to this project, such as for adding new events, fields, records, etc.</returns>
        public async Task<string> CreateProjectAsync<T>(string token, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Project.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                if (!IsNullOrEmpty(odm))
                {
                    payload.Add("odm", odm);
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.11.0 
        /// 
        /// Create A New Project
        /// 
        /// This method allows you to create a new REDCap project. A 64-character Super API Token is required for this method (as opposed to project-level API methods that require a regular 32-character token associated with the project-user). In the API request, you must minimally provide the project attributes 'project_title' and 'purpose' (with numerical value 0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support) when creating a project. 
        /// When a project is created with this method, the project will automatically be given all the project-level defaults just as if you created a new empty project via the web user interface, such as a automatically creating a single data collection instrument seeded with a single Record ID field and Form Status field, as well as (for longitudinal projects) one arm with one event. And if you intend to create your own arms or events immediately after creating the project, it is recommended that you utilize the override=1 parameter in the 'Import Arms' or 'Import Events' method, respectively, so that the default arm and event are removed when you add your own.Also, the user creating the project will automatically be added to the project as a user with full user privileges and a project-level API token, which could then be used for subsequent project-level API requests.
        /// NOTE: Only users with Super API Tokens can utilize this method.Users can only be granted a super token by a REDCap administrator(using the API Tokens page in the REDCap Control Center). Please be advised that users with a Super API Token can create new REDCap projects via the API without any approval needed by a REDCap administrator.If you are interested in obtaining a super token, please contact your local REDCap administrator.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have a Super API Token.
        /// </remarks>
        /// To use this method, you must have a Super API Token.
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The Super API Token specific to a user</param>
        /// <param name="content">project</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="data">
        /// Contains the attributes of the project to be created, in which they are provided in the specified format. While the only required attributes are 'project_title' and 'purpose', the fields listed below are all the possible attributes that can be provided in the 'data' parameter. The 'purpose' attribute must have a numerical value (0=Practice/Just for fun, 1=Other, 2=Research, 3=Quality Improvement, 4=Operational Support), in which 'purpose_other' is only required to have a value (as a text string) if purpose=1. The attributes is_longitudinal (0=False, 1=True; Default=0), surveys_enabled (0=False, 1=True; Default=0), and record_autonumbering_enabled (0=False, 1=True; Default=1) are all boolean. Please note that either is_longitudinal=1 or surveys_enabled=1 does not add arms/events or surveys to the project, respectively, but it merely enables those settings which are seen at the top of the project's Project Setup page.
        /// All available attributes:
        /// project_title, purpose, purpose_other, project_notes, is_longitudinal, surveys_enabled, record_autonumbering_enabled
        /// JSON Example:
        /// [{"project_title":"My New REDCap Project","purpose":"0"}]
        /// </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="odm">default: NULL - The 'odm' parameter must be an XML string in CDISC ODM XML format that contains project metadata (fields, forms, events, arms) and might optionally contain data to be imported as well. The XML contained in this parameter can come from a REDCap Project XML export file from REDCap itself, or may come from another system that is capable of exporting projects and data in CDISC ODM format. If the 'odm' parameter is included in the API request, it will use the XML to import its contents into the newly created project. This will allow you not only to create the project with the API request, but also to import all fields, forms, and project attributes (and events and arms, if longitudinal) as well as record data all at the same time.</param>
        /// <returns>When a project is created, a 32-character project-level API Token is returned (associated with both the project and user creating the project). This token could then ostensibly be used to make subsequent API calls to this project, such as for adding new events, fields, records, etc.</returns>
        public async Task<string> CreateProjectAsync<T>(string token, Content content, ReturnFormat format, List<T> data, OnErrorFormat onErrorFormat = OnErrorFormat.json, string odm = null)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                if (!IsNullOrEmpty(odm))
                {
                    payload.Add("odm", odm);
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Import Project Information
        /// This method allows you to update some of the basic attributes of a given REDCap project, such as the project's title, if it is longitudinal, if surveys are enabled, etc. Its data format corresponds to the format in the API method Export Project Information. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">project_settings</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="projectInfo">Contains some or all of the attributes from Export Project Information in the same data format as in the export. These attributes will change the project information.
        /// Attributes for the project in the format specified. For any values that are boolean, they should be represented as either a '0' (no/false) or '1' (yes/true). The following project attributes can be udpated:
        /// project_title, project_language, purpose, purpose_other, project_notes, custom_record_label, secondary_unique_field, is_longitudinal, surveys_enabled, scheduling_enabled, record_autonumbering_enabled, randomization_enabled, project_irb_number, project_grant_number, project_pi_firstname, project_pi_lastname, display_today_now_button
        /// </param>
        /// <returns>Returns the number of values accepted to be updated in the project settings (including values which remained the same before and after the import).</returns>
        public async Task<string> ImportProjectInfoAsync(string token, Content content, ReturnFormat format, RedcapProjectInfo projectInfo)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var _serializedData = JsonConvert.SerializeObject(projectInfo);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "data", _serializedData }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }
        /// <summary>
        /// API Version 1.0.0+
        /// Import Project Information
        /// This method allows you to update some of the basic attributes of a given REDCap project, such as the project's title, if it is longitudinal, if surveys are enabled, etc. Its data format corresponds to the format in the API method Export Project Information. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="projectInfo">Contains some or all of the attributes from Export Project Information in the same data format as in the export. These attributes will change the project information.
        /// Attributes for the project in the format specified. For any values that are boolean, they should be represented as either a '0' (no/false) or '1' (yes/true). The following project attributes can be udpated:
        /// project_title, project_language, purpose, purpose_other, project_notes, custom_record_label, secondary_unique_field, is_longitudinal, surveys_enabled, scheduling_enabled, record_autonumbering_enabled, randomization_enabled, project_irb_number, project_grant_number, project_pi_firstname, project_pi_lastname, display_today_now_button
        /// </param>
        /// <returns>Returns the number of values accepted to be updated in the project settings (including values which remained the same before and after the import).</returns>
        public async Task<string> ImportProjectInfoAsync(string token, ReturnFormat format, RedcapProjectInfo projectInfo)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var _serializedData = JsonConvert.SerializeObject(projectInfo);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.ProjectSettings.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "data", _serializedData }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export Project Information
        /// This method allows you to export some of the basic attributes of a given REDCap project, such as the project's title, if it is longitudinal, if surveys are enabled, the time the project was created and moved to production, etc.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">project</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>
        /// Attributes for the project in the format specified. For any values that are boolean, they will be represented as either a '0' (no/false) or '1' (yes/true). Also, all date/time values will be returned in Y-M-D H:M:S format. The following attributes will be returned:
        /// project_id, project_title, creation_time, production_time, in_production, project_language, purpose, purpose_other, project_notes, custom_record_label, secondary_unique_field, is_longitudinal, surveys_enabled, scheduling_enabled, record_autonumbering_enabled, randomization_enabled, ddp_enabled, project_irb_number, project_grant_number, project_pi_firstname, project_pi_lastname, display_today_now_button
        /// </returns>
        public async Task<string> ExportProjectInfoAsync(string token, Content content = Content.Project, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        /// <summary>
        /// API Version 1.0.0+
        /// Export Project Information
        /// This method allows you to export some of the basic attributes of a given REDCap project, such as the project's title, if it is longitudinal, if surveys are enabled, the time the project was created and moved to production, etc.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>
        /// Attributes for the project in the format specified. For any values that are boolean, they will be represented as either a '0' (no/false) or '1' (yes/true). Also, all date/time values will be returned in Y-M-D H:M:S format. The following attributes will be returned:
        /// project_id, project_title, creation_time, production_time, in_production, project_language, purpose, purpose_other, project_notes, custom_record_label, secondary_unique_field, is_longitudinal, surveys_enabled, scheduling_enabled, record_autonumbering_enabled, randomization_enabled, ddp_enabled, project_irb_number, project_grant_number, project_pi_firstname, project_pi_lastname, display_today_now_button
        /// </returns>
        public async Task<string> ExportProjectInfoAsync(string token, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Project.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.12.0
        /// 
        /// Export Entire Project as REDCap XML File (containing metadata and data)
        /// The entire project(all records, events, arms, instruments, fields, and project attributes) can be downloaded as a single XML file, which is in CDISC ODM format(ODM version 1.3.1). This XML file can be used to create a clone of the project(including its data, optionally) on this REDCap server or on another REDCap server (it can be uploaded on the Create New Project page). Because it is in CDISC ODM format, it can also be used to import the project into another ODM-compatible system. NOTE: All the option paramters listed below ONLY apply to data returned if the 'returnMetadataOnly' parameter is set to FALSE (default). For this API method, ALL metadata (all fields, forms, events, and arms) will always be exported.Only the data returned can be filtered using the optional parameters.
        /// Note about export rights: If the 'returnMetadataOnly' parameter is set to FALSE, then please be aware that Data Export user rights will be applied to any data returned from this API request. For example, if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project. 
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">project_xml</param>
        /// <param name="returnMetadataOnly">true, false [default] - TRUE returns only metadata (all fields, forms, events, and arms), whereas FALSE returns all metadata and also data (and optionally filters the data according to any of the optional parameters provided in the request) </param>
        /// <param name="records">an array of record names specifying specific records you wish to pull (by default, all records are pulled)</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all fields are pulled)</param>
        /// <param name="events">an array of unique event names that you wish to pull records for - only for longitudinal projects</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="exportSurveyFields">true, false [default] - specifies whether or not to export the survey identifier field (e.g., 'redcap_survey_identifier') or survey timestamp fields (e.g., instrument+'_timestamp') when surveys are utilized in the project. If you do not pass in this flag, it will default to 'false'. If set to 'true', it will return the redcap_survey_identifier field and also the survey timestamp field for a particular survey when at least one field from that survey is being exported. NOTE: If the survey identifier field or survey timestamp fields are imported via API data import, they will simply be ignored since they are not real fields in the project but rather are pseudo-fields.</param>
        /// <param name="exportDataAccessGroups">true, false [default] - specifies whether or not to export the 'redcap_data_access_group' field when data access groups are utilized in the project. If you do not pass in this flag, it will default to 'false'. NOTE: This flag is only viable if the user whose token is being used to make the API request is *not* in a data access group. If the user is in a group, then this flag will revert to its default value.</param>
        /// <param name="filterLogic">String of logic text (e.g., [age] > 30) for filtering the data to be returned by this API method, in which the API will only return the records (or record-events, if a longitudinal project) where the logic evaluates as TRUE. This parameter is blank/null by default unless a value is supplied. Please note that if the filter logic contains any incorrect syntax, the API will respond with an error message. </param>
        /// <param name="exportFiles">true, false [default] - TRUE will cause the XML returned to include all files uploaded for File Upload and Signature fields for all records in the project, whereas FALSE will cause all such fields not to be included. NOTE: Setting this option to TRUE can make the export very large and may prevent it from completing if the project contains many files or very large files. </param>
        /// <returns>The entire REDCap project's metadata (and data, if specified) will be returned in CDISC ODM format as a single XML string.</returns>
        public async Task<string> ExportProjectXmlAsync(string token, Content content, bool returnMetadataOnly = false, string[] records = null, string[] fields = null, string[] events = null, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null, bool exportFiles = false)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (returnMetadataOnly)
                {
                    payload.Add("returnMetadataOnly", returnMetadataOnly.ToString());
                }
                if (records?.Length > 0)
                {
                    payload.Add("records", await this.ConvertArraytoString(records));
                }
                if (events?.Length > 0)
                {
                    payload.Add("events", await this.ConvertArraytoString(events));
                }
                if (exportSurveyFields)
                {
                    payload.Add("exportSurveyFields", exportSurveyFields.ToString());
                }
                if (exportDataAccessGroups)
                {
                    payload.Add("exportDataAccessGroups", exportDataAccessGroups.ToString());
                }
                if (!IsNullOrEmpty(filterLogic))
                {
                    payload.Add("filterLogic", filterLogic);
                }
                if (exportFiles)
                {
                    payload.Add("exportFiles", exportFiles.ToString());
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.12.0
        /// 
        /// Export Entire Project as REDCap XML File (containing metadata and data)
        /// The entire project(all records, events, arms, instruments, fields, and project attributes) can be downloaded as a single XML file, which is in CDISC ODM format(ODM version 1.3.1). This XML file can be used to create a clone of the project(including its data, optionally) on this REDCap server or on another REDCap server (it can be uploaded on the Create New Project page). Because it is in CDISC ODM format, it can also be used to import the project into another ODM-compatible system. NOTE: All the option paramters listed below ONLY apply to data returned if the 'returnMetadataOnly' parameter is set to FALSE (default). For this API method, ALL metadata (all fields, forms, events, and arms) will always be exported.Only the data returned can be filtered using the optional parameters.
        /// Note about export rights: If the 'returnMetadataOnly' parameter is set to FALSE, then please be aware that Data Export user rights will be applied to any data returned from this API request. For example, if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project. 
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="returnMetadataOnly">true, false [default] - TRUE returns only metadata (all fields, forms, events, and arms), whereas FALSE returns all metadata and also data (and optionally filters the data according to any of the optional parameters provided in the request) </param>
        /// <param name="records">an array of record names specifying specific records you wish to pull (by default, all records are pulled)</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all fields are pulled)</param>
        /// <param name="events">an array of unique event names that you wish to pull records for - only for longitudinal projects</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="exportSurveyFields">true, false [default] - specifies whether or not to export the survey identifier field (e.g., 'redcap_survey_identifier') or survey timestamp fields (e.g., instrument+'_timestamp') when surveys are utilized in the project. If you do not pass in this flag, it will default to 'false'. If set to 'true', it will return the redcap_survey_identifier field and also the survey timestamp field for a particular survey when at least one field from that survey is being exported. NOTE: If the survey identifier field or survey timestamp fields are imported via API data import, they will simply be ignored since they are not real fields in the project but rather are pseudo-fields.</param>
        /// <param name="exportDataAccessGroups">true, false [default] - specifies whether or not to export the 'redcap_data_access_group' field when data access groups are utilized in the project. If you do not pass in this flag, it will default to 'false'. NOTE: This flag is only viable if the user whose token is being used to make the API request is *not* in a data access group. If the user is in a group, then this flag will revert to its default value.</param>
        /// <param name="filterLogic">String of logic text (e.g., [age] > 30) for filtering the data to be returned by this API method, in which the API will only return the records (or record-events, if a longitudinal project) where the logic evaluates as TRUE. This parameter is blank/null by default unless a value is supplied. Please note that if the filter logic contains any incorrect syntax, the API will respond with an error message. </param>
        /// <param name="exportFiles">true, false [default] - TRUE will cause the XML returned to include all files uploaded for File Upload and Signature fields for all records in the project, whereas FALSE will cause all such fields not to be included. NOTE: Setting this option to TRUE can make the export very large and may prevent it from completing if the project contains many files or very large files. </param>
        /// <returns>The entire REDCap project's metadata (and data, if specified) will be returned in CDISC ODM format as a single XML string.</returns>
        public async Task<string> ExportProjectXmlAsync(string token, bool returnMetadataOnly = false, string[] records = null, string[] fields = null, string[] events = null, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null, bool exportFiles = false)
        {
            try
            {
                /*
                 * Check for presence of token
                 */
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.ProjectXml.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (returnMetadataOnly)
                {
                    payload.Add("returnMetadataOnly", returnMetadataOnly.ToString());
                }
                if (records?.Length > 0)
                {
                    payload.Add("records", await this.ConvertArraytoString(records));
                }
                if (events?.Length > 0)
                {
                    payload.Add("events", await this.ConvertArraytoString(events));
                }
                if (exportSurveyFields)
                {
                    payload.Add("exportSurveyFields", exportSurveyFields.ToString());
                }
                if (exportDataAccessGroups)
                {
                    payload.Add("exportDataAccessGroups", exportDataAccessGroups.ToString());
                }
                if (!IsNullOrEmpty(filterLogic))
                {
                    payload.Add("filterLogic", filterLogic);
                }
                if (exportFiles)
                {
                    payload.Add("exportFiles", exportFiles.ToString());
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        #endregion Projects
        #region Records
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.18.0
        /// Generate Next Record Name
        /// To be used by projects with record auto-numbering enabled, this method exports the next potential record ID for a project. It generates the next record name by determining the current maximum numerical record ID and then incrementing it by one.
        /// Note: This method does not create a new record, but merely determines what the next record name would be.
        /// If using Data Access Groups (DAGs) in the project, this method accounts for the special formatting of the record name for users in DAGs (e.g., DAG-ID); in this case, it only assigns the next value for ID for all numbers inside a DAG. For example, if a DAG has a corresponding DAG number of 223 wherein records 223-1 and 223-2 already exist, then the next record will be 223-3 if the API user belongs to the DAG that has DAG number 223. (The DAG number is auto-assigned by REDCap for each DAG when the DAG is first created.) When generating a new record name in a DAG, the method considers all records in the entire project when determining the maximum record ID, including those that might have been originally created in that DAG but then later reassigned to another DAG.
        /// Note: This method functions the same even for projects that do not have record auto-numbering enabled.
        /// </summary>
        /// 
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <returns>The maximum integer record ID + 1.</returns>
        public async Task<string> GenerateNextRecordNameAsync(string token)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.GenerateNextRecordName.GetDisplayName() },
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.18.0
        /// Generate Next Record Name
        /// To be used by projects with record auto-numbering enabled, this method exports the next potential record ID for a project. It generates the next record name by determining the current maximum numerical record ID and then incrementing it by one.
        /// Note: This method does not create a new record, but merely determines what the next record name would be.
        /// If using Data Access Groups (DAGs) in the project, this method accounts for the special formatting of the record name for users in DAGs (e.g., DAG-ID); in this case, it only assigns the next value for ID for all numbers inside a DAG. For example, if a DAG has a corresponding DAG number of 223 wherein records 223-1 and 223-2 already exist, then the next record will be 223-3 if the API user belongs to the DAG that has DAG number 223. (The DAG number is auto-assigned by REDCap for each DAG when the DAG is first created.) When generating a new record name in a DAG, the method considers all records in the entire project when determining the maximum record ID, including those that might have been originally created in that DAG but then later reassigned to another DAG.
        /// Note: This method functions the same even for projects that do not have record auto-numbering enabled.
        /// </summary>
        /// 
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">generateNextRecordName</param>
        /// <returns>The maximum integer record ID + 1.</returns>
        public async Task<string> GenerateNextRecordNameAsync(string token, Content content)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export Records
        /// This method allows you to export a set of records for a project.
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API data export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml, odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="redcapDataType">flat - output as one record per row [default], eav - output as one data point per row. Non-longitudinal: Will have the fields - record*, field_name, value. Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name</param>
        /// <param name="records">an array of record names specifying specific records you wish to pull (by default, all records are pulled)</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all fields are pulled)</param>
        /// <param name="forms">an array of form names you wish to pull records for. If the form name has a space in it, replace the space with an underscore (by default, all records are pulled)</param>
        /// <param name="events">an array of unique event names that you wish to pull records for - only for longitudinal projects</param>
        /// <param name="rawOrLabel">raw [default], label - export the raw coded values or labels for the options of multiple choice fields</param>
        /// <param name="rawOrLabelHeaders">raw [default], label - (for 'csv' format 'flat' type only) for the CSV headers, export the variable/field names (raw) or the field labels (label)</param>
        /// <param name="exportCheckboxLabel">true, false [default] - specifies the format of checkbox field values specifically when exporting the data as labels (i.e., when rawOrLabel=label) in flat format (i.e., when type=flat). When exporting labels, by default (without providing the exportCheckboxLabel flag or if exportCheckboxLabel=false), all checkboxes will either have a value 'Checked' if they are checked or 'Unchecked' if not checked. But if exportCheckboxLabel is set to true, it will instead export the checkbox value as the checkbox option's label (e.g., 'Choice 1') if checked or it will be blank/empty (no value) if not checked. If rawOrLabel=false or if type=eav, then the exportCheckboxLabel flag is ignored. (The exportCheckboxLabel parameter is ignored for type=eav because 'eav' type always exports checkboxes differently anyway, in which checkboxes are exported with their true variable name (whereas the 'flat' type exports them as variable___code format), and another difference is that 'eav' type *always* exports checkbox values as the choice label for labels export, or as 0 or 1 (if unchecked or checked, respectively) for raw export.)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="exportSurveyFields">true, false [default] - specifies whether or not to export the survey identifier field (e.g., 'redcap_survey_identifier') or survey timestamp fields (e.g., instrument+'_timestamp') when surveys are utilized in the project. If you do not pass in this flag, it will default to 'false'. If set to 'true', it will return the redcap_survey_identifier field and also the survey timestamp field for a particular survey when at least one field from that survey is being exported. NOTE: If the survey identifier field or survey timestamp fields are imported via API data import, they will simply be ignored since they are not real fields in the project but rather are pseudo-fields.</param>
        /// <param name="exportDataAccessGroups">true, false [default] - specifies whether or not to export the 'redcap_data_access_group' field when data access groups are utilized in the project. If you do not pass in this flag, it will default to 'false'. NOTE: This flag is only viable if the user whose token is being used to make the API request is *not* in a data access group. If the user is in a group, then this flag will revert to its default value.</param>
        /// <param name="filterLogic">String of logic text (e.g., [age] > 30) for filtering the data to be returned by this API method, in which the API will only return the records (or record-events, if a longitudinal project) where the logic evaluates as TRUE. This parameter is blank/null by default unless a value is supplied. Please note that if the filter logic contains any incorrect syntax, the API will respond with an error message. </param>
        /// <returns>Data from the project in the format and type specified ordered by the record (primary key of project) and then by event id</returns>
        public async Task<string> ExportRecordsAsync(string token, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] records = null, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null)
        {
            try
            {
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Record.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "type", redcapDataType.GetDisplayName() }
                };

                // Optional
                if (records?.Length > 0)
                {
                    payload.Add("records", await this.ConvertArraytoString(records));
                }
                if (fields?.Length > 0)
                {
                    payload.Add("fields", await this.ConvertArraytoString(fields));
                }
                if (forms?.Length > 0)
                {
                    payload.Add("forms", await this.ConvertArraytoString(forms));
                }
                if (events?.Length > 0)
                {
                    payload.Add("events", await this.ConvertArraytoString(events));
                }
                /*
                 * Pertains to CSV data only
                 */
                var _rawOrLabelValue = rawOrLabelHeaders.ToString();
                if (!IsNullOrEmpty(_rawOrLabelValue))
                {
                    payload.Add("rawOrLabel", _rawOrLabelValue);
                }
                // Optional (defaults to false)
                if (exportCheckboxLabel)
                {
                    payload.Add("exportCheckboxLabel", exportCheckboxLabel.ToString());
                }
                // Optional (defaults to false)
                if (exportSurveyFields)
                {
                    payload.Add("exportSurveyFields", exportSurveyFields.ToString());
                }
                // Optional (defaults to false)
                if (exportDataAccessGroups)
                {
                    payload.Add("exportDataAccessGroups", exportDataAccessGroups.ToString());
                }
                if (!IsNullOrEmpty(filterLogic))
                {
                    payload.Add("filterLogic", filterLogic);
                }
                return await this.SendPostRequestAsync(payload, _uri);

            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export Records
        /// This method allows you to export a set of records for a project.
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API data export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">record</param>
        /// <param name="format">csv, json [default], xml, odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="redcapDataType">flat - output as one record per row [default], eav - output as one data point per row. Non-longitudinal: Will have the fields - record*, field_name, value. Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name</param>
        /// <param name="records">an array of record names specifying specific records you wish to pull (by default, all records are pulled)</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all fields are pulled)</param>
        /// <param name="forms">an array of form names you wish to pull records for. If the form name has a space in it, replace the space with an underscore (by default, all records are pulled)</param>
        /// <param name="events">an array of unique event names that you wish to pull records for - only for longitudinal projects</param>
        /// <param name="rawOrLabel">raw [default], label - export the raw coded values or labels for the options of multiple choice fields</param>
        /// <param name="rawOrLabelHeaders">raw [default], label - (for 'csv' format 'flat' type only) for the CSV headers, export the variable/field names (raw) or the field labels (label)</param>
        /// <param name="exportCheckboxLabel">true, false [default] - specifies the format of checkbox field values specifically when exporting the data as labels (i.e., when rawOrLabel=label) in flat format (i.e., when type=flat). When exporting labels, by default (without providing the exportCheckboxLabel flag or if exportCheckboxLabel=false), all checkboxes will either have a value 'Checked' if they are checked or 'Unchecked' if not checked. But if exportCheckboxLabel is set to true, it will instead export the checkbox value as the checkbox option's label (e.g., 'Choice 1') if checked or it will be blank/empty (no value) if not checked. If rawOrLabel=false or if type=eav, then the exportCheckboxLabel flag is ignored. (The exportCheckboxLabel parameter is ignored for type=eav because 'eav' type always exports checkboxes differently anyway, in which checkboxes are exported with their true variable name (whereas the 'flat' type exports them as variable___code format), and another difference is that 'eav' type *always* exports checkbox values as the choice label for labels export, or as 0 or 1 (if unchecked or checked, respectively) for raw export.)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="exportSurveyFields">true, false [default] - specifies whether or not to export the survey identifier field (e.g., 'redcap_survey_identifier') or survey timestamp fields (e.g., instrument+'_timestamp') when surveys are utilized in the project. If you do not pass in this flag, it will default to 'false'. If set to 'true', it will return the redcap_survey_identifier field and also the survey timestamp field for a particular survey when at least one field from that survey is being exported. NOTE: If the survey identifier field or survey timestamp fields are imported via API data import, they will simply be ignored since they are not real fields in the project but rather are pseudo-fields.</param>
        /// <param name="exportDataAccessGroups">true, false [default] - specifies whether or not to export the 'redcap_data_access_group' field when data access groups are utilized in the project. If you do not pass in this flag, it will default to 'false'. NOTE: This flag is only viable if the user whose token is being used to make the API request is *not* in a data access group. If the user is in a group, then this flag will revert to its default value.</param>
        /// <param name="filterLogic">String of logic text (e.g., [age] > 30) for filtering the data to be returned by this API method, in which the API will only return the records (or record-events, if a longitudinal project) where the logic evaluates as TRUE. This parameter is blank/null by default unless a value is supplied. Please note that if the filter logic contains any incorrect syntax, the API will respond with an error message. </param>
        /// <returns>Data from the project in the format and type specified ordered by the record (primary key of project) and then by event id</returns>
        public async Task<string> ExportRecordsAsync(string token, Content content, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] records = null, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null)
        {
            try
            {
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "type", redcapDataType.GetDisplayName() }
                };

                // Optional
                if (records?.Length > 0)
                {
                    payload.Add("records", await this.ConvertArraytoString(records));
                }
                if (fields?.Length > 0)
                {
                    payload.Add("fields", await this.ConvertArraytoString(fields));
                }
                if (forms?.Length > 0)
                {
                    payload.Add("forms", await this.ConvertArraytoString(forms));
                }
                if (events?.Length > 0)
                {
                    payload.Add("events", await this.ConvertArraytoString(events));
                }
                /*
                 * Pertains to CSV data only
                 */
                var _rawOrLabelValue = rawOrLabelHeaders.ToString();
                if (!IsNullOrEmpty(_rawOrLabelValue))
                {
                    payload.Add("rawOrLabel", _rawOrLabelValue);
                }
                // Optional (defaults to false)
                if (exportCheckboxLabel)
                {
                    payload.Add("exportCheckboxLabel", exportCheckboxLabel.ToString());
                }
                // Optional (defaults to false)
                if (exportSurveyFields)
                {
                    payload.Add("exportSurveyFields", exportSurveyFields.ToString());
                }
                // Optional (defaults to false)
                if (exportDataAccessGroups)
                {
                    payload.Add("exportDataAccessGroups", exportDataAccessGroups.ToString());
                }
                if (!IsNullOrEmpty(filterLogic))
                {
                    payload.Add("filterLogic", filterLogic);
                }
                return await this.SendPostRequestAsync(payload, _uri);

            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }

        /// <summary>
        /// API Version 1.0.0++
        /// Export Record
        /// This method allows you to export a single record for a project.
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API data export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">record</param>
        /// <param name="format">csv, json [default], xml, odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="redcapDataType">flat - output as one record per row [default], eav - output as one data point per row. Non-longitudinal: Will have the fields - record*, field_name, value. Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name</param>
        /// <param name="record">a single record specifying specific records you wish to pull (by default, all records are pulled)</param>
        /// <param name="fields">an array of field names specifying specific fields you wish to pull (by default, all fields are pulled)</param>
        /// <param name="forms">an array of form names you wish to pull records for. If the form name has a space in it, replace the space with an underscore (by default, all records are pulled)</param>
        /// <param name="events">an array of unique event names that you wish to pull records for - only for longitudinal projects</param>
        /// <param name="rawOrLabel">raw [default], label - export the raw coded values or labels for the options of multiple choice fields</param>
        /// <param name="rawOrLabelHeaders">raw [default], label - (for 'csv' format 'flat' type only) for the CSV headers, export the variable/field names (raw) or the field labels (label)</param>
        /// <param name="exportCheckboxLabel">true, false [default] - specifies the format of checkbox field values specifically when exporting the data as labels (i.e., when rawOrLabel=label) in flat format (i.e., when type=flat). When exporting labels, by default (without providing the exportCheckboxLabel flag or if exportCheckboxLabel=false), all checkboxes will either have a value 'Checked' if they are checked or 'Unchecked' if not checked. But if exportCheckboxLabel is set to true, it will instead export the checkbox value as the checkbox option's label (e.g., 'Choice 1') if checked or it will be blank/empty (no value) if not checked. If rawOrLabel=false or if type=eav, then the exportCheckboxLabel flag is ignored. (The exportCheckboxLabel parameter is ignored for type=eav because 'eav' type always exports checkboxes differently anyway, in which checkboxes are exported with their true variable name (whereas the 'flat' type exports them as variable___code format), and another difference is that 'eav' type *always* exports checkbox values as the choice label for labels export, or as 0 or 1 (if unchecked or checked, respectively) for raw export.)</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="exportSurveyFields">true, false [default] - specifies whether or not to export the survey identifier field (e.g., 'redcap_survey_identifier') or survey timestamp fields (e.g., instrument+'_timestamp') when surveys are utilized in the project. If you do not pass in this flag, it will default to 'false'. If set to 'true', it will return the redcap_survey_identifier field and also the survey timestamp field for a particular survey when at least one field from that survey is being exported. NOTE: If the survey identifier field or survey timestamp fields are imported via API data import, they will simply be ignored since they are not real fields in the project but rather are pseudo-fields.</param>
        /// <param name="exportDataAccessGroups">true, false [default] - specifies whether or not to export the 'redcap_data_access_group' field when data access groups are utilized in the project. If you do not pass in this flag, it will default to 'false'. NOTE: This flag is only viable if the user whose token is being used to make the API request is *not* in a data access group. If the user is in a group, then this flag will revert to its default value.</param>
        /// <param name="filterLogic">String of logic text (e.g., [age] > 30) for filtering the data to be returned by this API method, in which the API will only return the records (or record-events, if a longitudinal project) where the logic evaluates as TRUE. This parameter is blank/null by default unless a value is supplied. Please note that if the filter logic contains any incorrect syntax, the API will respond with an error message. </param>
        /// <returns>Data from the project in the format and type specified ordered by the record (primary key of project) and then by event id</returns>
        public async Task<string> ExportRecordAsync(string token, Content content, string record, ReturnFormat format = ReturnFormat.json, RedcapDataType redcapDataType = RedcapDataType.flat, string[] fields = null, string[] forms = null, string[] events = null, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false, OnErrorFormat onErrorFormat = OnErrorFormat.json, bool exportSurveyFields = false, bool exportDataAccessGroups = false, string filterLogic = null)
        {
            try
            {
                this.CheckToken(token);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "records", record },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "type", redcapDataType.GetDisplayName() }
                };

                // Optional
                if (fields?.Length > 0)
                {
                    payload.Add("fields", await this.ConvertArraytoString(fields));
                }
                if (forms?.Length > 0)
                {
                    payload.Add("forms", await this.ConvertArraytoString(forms));
                }
                if (events?.Length > 0)
                {
                    payload.Add("events", await this.ConvertArraytoString(events));
                }
                /*
                 * Pertains to CSV data only
                 */
                var _rawOrLabelValue = rawOrLabelHeaders.ToString();
                if (!IsNullOrEmpty(_rawOrLabelValue))
                {
                    payload.Add("rawOrLabel", _rawOrLabelValue);
                }
                // Optional (defaults to false)
                if (exportCheckboxLabel)
                {
                    payload.Add("exportCheckboxLabel", exportCheckboxLabel.ToString());
                }
                // Optional (defaults to false)
                if (exportSurveyFields)
                {
                    payload.Add("exportSurveyFields", exportSurveyFields.ToString());
                }
                // Optional (defaults to false)
                if (exportDataAccessGroups)
                {
                    payload.Add("exportDataAccessGroups", exportDataAccessGroups.ToString());
                }
                if (!IsNullOrEmpty(filterLogic))
                {
                    payload.Add("filterLogic", filterLogic);
                }
                return await this.SendPostRequestAsync(payload, _uri);

            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }

        }


        /// <summary>
        /// API Version 1.0.0+
        /// Import Records
        /// This method allows you to import a set of records for a project
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml, odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="redcapDataType">flat - output as one record per row [default]
        /// eav - input as one data point per row
        /// Non-longitudinal: Will have the fields - record*, field_name, value
        /// Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name
        /// </param>
        /// <param name="overwriteBehavior">
        /// normal - blank/empty values will be ignored [default]
        /// overwrite - blank/empty values are valid and will overwrite data</param>
        /// <param name="forceAutoNumber">If record auto-numbering has been enabled in the project, it may be desirable to import records where each record's record name is automatically determined by REDCap (just as it does in the user interface). If this parameter is set to 'true', the record names provided in the request will not be used (although they are still required in order to associate multiple rows of data to an individual record in the request), but instead those records in the request will receive new record names during the import process. NOTE: To see how the provided record names get translated into new auto record names, the returnContent parameter should be set to 'auto_ids', which will return a record list similar to 'ids' value, but it will have the new record name followed by the provided record name in the request, in which the two are comma-delimited. For example, if 
        /// false (or 'false') - The record names provided in the request will be used. [default]
        /// true (or 'true') - New record names will be automatically determined.</param>
        /// <param name="data">The formatted data to be imported. The data should be a List of Dictionary(string,string) or object that contains the fields and values.
        /// NOTE: When importing data in EAV type format, please be aware that checkbox fields must have their field_name listed as variable+'___'+optionCode and its value as either '0' or '1' (unchecked or checked, respectively). For example, for a checkbox field with variable name 'icecream', it would be imported as EAV with the field_name as 'icecream___4' having a value of '1' in order to set the option coded with '4' (which might be 'Chocolate') as 'checked'.</param>
        /// <param name="dateFormat">MDY, DMY, YMD [default] - the format of values being imported for dates or datetime fields (understood with M representing 'month', D as 'day', and Y as 'year') - NOTE: The default format is Y-M-D (with dashes), while MDY and DMY values should always be formatted as M/D/Y or D/M/Y (with slashes), respectively.</param>
        /// <param name="returnContent">count [default] - the number of records imported, ids - a list of all record IDs that were imported, auto_ids = (used only when forceAutoNumber=true) a list of pairs of all record IDs that were imported, includes the new ID created and the ID value that was sent in the API request (e.g., 323,10). </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>the content specified by returnContent</returns>
        public async Task<string> ImportRecordsAsync<T>(string token, ReturnFormat format, RedcapDataType redcapDataType, OverwriteBehavior overwriteBehavior, bool forceAutoNumber, List<T> data, string dateFormat, ReturnContent returnContent = ReturnContent.count, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                this.CheckToken(token);
                var _serializedData = JsonConvert.SerializeObject(data);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Record.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "type", redcapDataType.GetDisplayName() },
                    { "overwriteBehavior", overwriteBehavior.ToString() },
                    { "forceAutoNumber", forceAutoNumber.ToString() },
                    { "data", _serializedData },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (!IsNullOrEmpty(dateFormat))
                {
                    payload.Add("dateFormat", dateFormat);
                }
                if (!IsNullOrEmpty(returnContent.ToString()))
                {
                    payload.Add("returnContent", returnContent.ToString());
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Import Records
        /// This method allows you to import a set of records for a project
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">record</param>
        /// <param name="format">csv, json [default], xml, odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="redcapDataType">flat - output as one record per row [default]
        /// eav - input as one data point per row
        /// Non-longitudinal: Will have the fields - record*, field_name, value
        /// Longitudinal: Will have the fields - record*, field_name, value, redcap_event_name
        /// </param>
        /// <param name="overwriteBehavior">
        /// normal - blank/empty values will be ignored [default]
        /// overwrite - blank/empty values are valid and will overwrite data</param>
        /// <param name="forceAutoNumber">If record auto-numbering has been enabled in the project, it may be desirable to import records where each record's record name is automatically determined by REDCap (just as it does in the user interface). If this parameter is set to 'true', the record names provided in the request will not be used (although they are still required in order to associate multiple rows of data to an individual record in the request), but instead those records in the request will receive new record names during the import process. NOTE: To see how the provided record names get translated into new auto record names, the returnContent parameter should be set to 'auto_ids', which will return a record list similar to 'ids' value, but it will have the new record name followed by the provided record name in the request, in which the two are comma-delimited. For example, if 
        /// false (or 'false') - The record names provided in the request will be used. [default]
        /// true (or 'true') - New record names will be automatically determined.</param>
        /// <param name="data">The formatted data to be imported. The data should be a List of Dictionary(string,string) or object that contains the fields and values.
        /// NOTE: When importing data in EAV type format, please be aware that checkbox fields must have their field_name listed as variable+'___'+optionCode and its value as either '0' or '1' (unchecked or checked, respectively). For example, for a checkbox field with variable name 'icecream', it would be imported as EAV with the field_name as 'icecream___4' having a value of '1' in order to set the option coded with '4' (which might be 'Chocolate') as 'checked'.</param>
        /// <param name="dateFormat">MDY, DMY, YMD [default] - the format of values being imported for dates or datetime fields (understood with M representing 'month', D as 'day', and Y as 'year') - NOTE: The default format is Y-M-D (with dashes), while MDY and DMY values should always be formatted as M/D/Y or D/M/Y (with slashes), respectively.</param>
        /// <param name="returnContent">count [default] - the number of records imported, ids - a list of all record IDs that were imported, auto_ids = (used only when forceAutoNumber=true) a list of pairs of all record IDs that were imported, includes the new ID created and the ID value that was sent in the API request (e.g., 323,10). </param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>the content specified by returnContent</returns>
        public async Task<string> ImportRecordsAsync<T>(string token, Content content, ReturnFormat format, RedcapDataType redcapDataType, OverwriteBehavior overwriteBehavior, bool forceAutoNumber, List<T> data, string dateFormat, ReturnContent returnContent = ReturnContent.count, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                this.CheckToken(token);


                var _serializedData = JsonConvert.SerializeObject(data);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "type", redcapDataType.GetDisplayName() },
                    { "overwriteBehavior", overwriteBehavior.ToString() },
                    { "forceAutoNumber", forceAutoNumber.ToString() },
                    { "data", _serializedData },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                if (!IsNullOrEmpty(dateFormat))
                {
                    payload.Add("dateFormat", dateFormat);
                }
                if (!IsNullOrEmpty(returnContent.ToString()))
                {
                    payload.Add("returnContent", returnContent.ToString());
                }
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Delete Records
        /// This method allows you to delete one or more records from a project in a single API request.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have 'Delete Record' user privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="records">an array of record names specifying specific records you wish to delete</param>
        /// <param name="arm">the arm number of the arm in which the record(s) should be deleted. 
        /// (This can only be used if the project is longitudinal with more than one arm.) NOTE: If the arm parameter is not provided, the specified records will be deleted from all arms in which they exist. Whereas, if arm is provided, they will only be deleted from the specified arm. </param>
        /// <returns>the number of records deleted.</returns>
        public async Task<string> DeleteRecordsAsync(string token, string[] records, int? arm)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Record.GetDisplayName() },
                    { "action",  RedcapAction.Delete.GetDisplayName() }
                };
                // Required
                //payload.Add("records", await this.ConvertArraytoString(records));
                for (var i = 0; i < records.Length; i++)
                {
                    payload.Add($"records[{i}]", records[i]);
                }

                // Optional
                payload.Add("arm", arm?.ToString());

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Delete Records
        /// This method allows you to delete one or more records from a project in a single API request.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have 'Delete Record' user privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">record</param>
        /// <param name="action">delete</param>
        /// <param name="records">an array of record names specifying specific records you wish to delete</param>
        /// <param name="arm">the arm number of the arm in which the record(s) should be deleted. 
        /// (This can only be used if the project is longitudinal with more than one arm.) NOTE: If the arm parameter is not provided, the specified records will be deleted from all arms in which they exist. Whereas, if arm is provided, they will only be deleted from the specified arm. </param>
        /// <returns>the number of records deleted.</returns>
        public async Task<string> DeleteRecordsAsync(string token, Content content, RedcapAction action, string[] records, int? arm)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "action",  action.GetDisplayName() }
                };
                // Required
                //payload.Add("records", await this.ConvertArraytoString(records));
                for (var i = 0; i < records.Length; i++)
                {
                    payload.Add($"records[{i}]", records[i]);
                }

                // Optional
                payload.Add("arm", arm?.ToString());

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Records
        #region Repeating Instruments and Events

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 8.2.0
        /// 
        /// Export Repeating Instruments and Events
        /// 
        /// This method allows you to export a list of the repeated instruments and repeating events for a project. This includes their unique instrument name as seen in the second column of the Data Dictionary, as well as each repeating instrument's corresponding custom repeating instrument label. For longitudinal projects, the unique event name is also returned for each repeating instrument. Additionally, repeating events are returned as separate items, in which the instrument name will be blank/null to indicate that it is a repeating event (rather than a repeating instrument). 
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <returns>Repeated instruments and events for the project in the format specified and will be ordered according to their order in the project.</returns>
        public async Task<string> ExportRepeatingInstrumentsAndEvents(string token, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.RepeatingFormsEvents.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 8.2.0
        /// 
        /// Export Repeating Instruments and Events
        /// 
        /// This method allows you to export a list of the repeated instruments and repeating events for a project. This includes their unique instrument name as seen in the second column of the Data Dictionary, as well as each repeating instrument's corresponding custom repeating instrument label. For longitudinal projects, the unique event name is also returned for each repeating instrument. Additionally, repeating events are returned as separate items, in which the instrument name will be blank/null to indicate that it is a repeating event (rather than a repeating instrument). 
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">repeatingFormsEvents</param>
        /// <param name="format">csv, json [default], xml odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <returns>Repeated instruments and events for the project in the format specified and will be ordered according to their order in the project.</returns>
        public async Task<string> ExportRepeatingInstrumentsAndEvents(string token, Content content, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 8.10.0
        /// 
        /// Import Repeating Instruments and Events
        /// This method allows you to import a list of the repeated instruments and repeating events for a project. This includes their unique instrument name as seen in the second column of the Data Dictionary, as well as each repeating instrument's corresponding custom repeating instrument label. For longitudinal projects, the unique event name is also needed for each repeating instrument. Additionally, repeating events must be submitted as separate items, in which the instrument name will be blank/null to indicate that it is a repeating event (rather than a repeating instrument).
        /// </summary>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="data">Note: Super API Tokens can also be utilized for this method instead of a project-level API token. Users can only be granted a super token by a REDCap administrator (using the API Tokens page in the REDCap Control Center).</param>
        /// <param name="content">repeatingFormsEvents</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Repeated instruments and events for the project in the format specified and will be ordered according to their order in the project.</returns>
        public async Task<string> ImportRepeatingInstrumentsAndEvents<T>(string token, List<T> data, Content content = Content.RepeatingFormsEvents, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);

                var _serializedData = JsonConvert.SerializeObject(data);

                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };
                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        #endregion Repeating Instruments and Events
        #region Reports
        /// <summary>
        /// API Version 1.0.0+
        /// Export Reports
        /// This method allows you to export the data set of a report created on a project's 'Data Exports, Reports, and Stats' page.
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API report export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project.
        /// Also, please note the the 'Export Reports' method does *not* make use of the 'type' (flat/eav) parameter, which can be used in the 'Export Records' method.All data for the 'Export Reports' method is thus exported in flat format.If the 'type' parameter is supplied in the API request, it will be ignored.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="reportId">the report ID number provided next to the report name on the report list page</param>
        /// <param name="format">csv, json [default], xml odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="onErrorFormat">csv, json [default], xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="rawOrLabel">raw [default], label - export the raw coded values or labels for the options of multiple choice fields</param>
        /// <param name="rawOrLabelHeaders">raw [default], label - (for 'csv' format 'flat' type only) for the CSV headers, export the variable/field names (raw) or the field labels (label)</param>
        /// <param name="exportCheckboxLabel">true, false [default] - specifies the format of checkbox field values specifically when exporting the data as labels (i.e., when rawOrLabel=label). When exporting labels, by default (without providing the exportCheckboxLabel flag or if exportCheckboxLabel=false), all checkboxes will either have a value 'Checked' if they are checked or 'Unchecked' if not checked. But if exportCheckboxLabel is set to true, it will instead export the checkbox value as the checkbox option's label (e.g., 'Choice 1') if checked or it will be blank/empty (no value) if not checked. If rawOrLabel=false, then the exportCheckboxLabel flag is ignored.</param>
        /// <returns>Data from the project in the format and type specified ordered by the record (primary key of project) and then by event id</returns>
        public async Task<string> ExportReportsAsync(string token, int reportId, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Report.GetDisplayName() },
                    { "report_id", reportId.ToString() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                var _rawOrLabel = rawOrLabel.ToString();
                if (!IsNullOrEmpty(_rawOrLabel))
                {
                    payload.Add("rawOrLabel", _rawOrLabel);
                }
                var _rawOrLabelHeaders = rawOrLabelHeaders.ToString();
                if (!IsNullOrEmpty(_rawOrLabelHeaders))
                {
                    payload.Add("rawOrLabelHeaders", _rawOrLabelHeaders);
                }
                if (exportCheckboxLabel)
                {
                    payload.Add("exportCheckboxLabel", exportCheckboxLabel.ToString());
                }

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export Reports
        /// This method allows you to export the data set of a report created on a project's 'Data Exports, Reports, and Stats' page.
        /// Note about export rights: Please be aware that Data Export user rights will be applied to this API request.For example, if you have 'No Access' data export rights in the project, then the API report export will fail and return an error. And if you have 'De-Identified' or 'Remove all tagged Identifier fields' data export rights, then some data fields *might* be removed and filtered out of the data set returned from the API. To make sure that no data is unnecessarily filtered out of your API request, you should have 'Full Data Set' export rights in the project.
        /// Also, please note the the 'Export Reports' method does *not* make use of the 'type' (flat/eav) parameter, which can be used in the 'Export Records' method.All data for the 'Export Reports' method is thus exported in flat format.If the 'type' parameter is supplied in the API request, it will be ignored.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">report</param>
        /// <param name="reportId">the report ID number provided next to the report name on the report list page</param>
        /// <param name="format">csv, json [default], xml odm ('odm' refers to CDISC ODM XML format, specifically ODM version 1.3.1)</param>
        /// <param name="onErrorFormat">csv, json [default], xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <param name="rawOrLabel">raw [default], label - export the raw coded values or labels for the options of multiple choice fields</param>
        /// <param name="rawOrLabelHeaders">raw [default], label - (for 'csv' format 'flat' type only) for the CSV headers, export the variable/field names (raw) or the field labels (label)</param>
        /// <param name="exportCheckboxLabel">true, false [default] - specifies the format of checkbox field values specifically when exporting the data as labels (i.e., when rawOrLabel=label). When exporting labels, by default (without providing the exportCheckboxLabel flag or if exportCheckboxLabel=false), all checkboxes will either have a value 'Checked' if they are checked or 'Unchecked' if not checked. But if exportCheckboxLabel is set to true, it will instead export the checkbox value as the checkbox option's label (e.g., 'Choice 1') if checked or it will be blank/empty (no value) if not checked. If rawOrLabel=false, then the exportCheckboxLabel flag is ignored.</param>
        /// <returns>Data from the project in the format and type specified ordered by the record (primary key of project) and then by event id</returns>
        public async Task<string> ExportReportsAsync(string token, Content content, int reportId, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json, RawOrLabel rawOrLabel = RawOrLabel.raw, RawOrLabelHeaders rawOrLabelHeaders = RawOrLabelHeaders.raw, bool exportCheckboxLabel = false)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "report_id", reportId.ToString() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };
                // Optional
                var _rawOrLabel = rawOrLabel.ToString();
                if (!IsNullOrEmpty(_rawOrLabel))
                {
                    payload.Add("rawOrLabel", _rawOrLabel);
                }
                var _rawOrLabelHeaders = rawOrLabelHeaders.ToString();
                if (!IsNullOrEmpty(_rawOrLabelHeaders))
                {
                    payload.Add("rawOrLabelHeaders", _rawOrLabelHeaders);
                }
                if (exportCheckboxLabel)
                {
                    payload.Add("exportCheckboxLabel", exportCheckboxLabel.ToString());
                }

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Reports
        #region Redcap

        /// <summary>
        /// API Version 1.0.0+
        /// Export REDCap Version
        /// This method returns the current REDCap version number as plain text (e.g., 4.13.18, 5.12.2, 6.0.0).
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content"></param>
        /// <param name="format">csv, json [default], xml</param>
        /// <returns>The current REDCap version number (three numbers delimited with two periods) as plain text - e.g., 4.13.18, 5.12.2, 6.0.0</returns>
        public async Task<string> ExportRedcapVersionAsync(string token, Content content = Content.Version, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export REDCap Version
        /// This method returns the current REDCap version number as plain text (e.g., 4.13.18, 5.12.2, 6.0.0).
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <returns>The current REDCap version number (three numbers delimited with two periods) as plain text - e.g., 4.13.18, 5.12.2, 6.0.0</returns>
        public async Task<string> ExportRedcapVersionAsync(string token, ReturnFormat format = ReturnFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.Version.GetDisplayName() },
                    { "format", format.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Reports
        #region Surveys
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export a Survey Link for a Participant
        /// This method returns a unique survey link (i.e., a URL) in plain text format for a specified record and data collection instrument (and event, if longitudinal) in a project. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project, an error will be returned.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json [default], xml - The returnFormat is only used with regard to the format of any error messages that might be returned.</param>
        /// <returns>Returns a unique survey link (i.e., a URL) in plain text format for the specified record and instrument (and event, if longitudinal).</returns>
        public async Task<string> ExportSurveyLinkAsync(string token, string record, string instrument, string eventName, int repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.SurveyLink.GetDisplayName() },
                    { "record", record },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "repeat_instance", repeatInstance.ToString() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export a Survey Link for a Participant
        /// This method returns a unique survey link (i.e., a URL) in plain text format for a specified record and data collection instrument (and event, if longitudinal) in a project. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project, an error will be returned.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">surveyLink</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json [default], xml - The returnFormat is only used with regard to the format of any error messages that might be returned.</param>
        /// <returns>Returns a unique survey link (i.e., a URL) in plain text format for the specified record and instrument (and event, if longitudinal).</returns>
        public async Task<string> ExportSurveyLinkAsync(string token, Content content, string record, string instrument, string eventName, int repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "record", record },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "repeat_instance", repeatInstance.ToString() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export a Survey Participant List
        /// This method returns the list of all participants for a specific survey instrument (and for a specific event, if a longitudinal project). If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project, an error will be returned.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns the list of all participants for the specified survey instrument [and event] in the desired format. The following fields are returned: email, email_occurrence, identifier, invitation_sent_status, invitation_send_time, response_status, survey_access_code, survey_link. The attribute 'email_occurrence' represents the current count that the email address has appeared in the list (because emails can be used more than once), thus email + email_occurrence represent a unique value pair. 'invitation_sent_status' is '0' if an invitation has not yet been sent to the participant, and is '1' if it has. 'invitation_send_time' is the date/time in which the next invitation will be sent, and is blank if there is no invitation that is scheduled to be sent. 'response_status' represents whether the participant has responded to the survey, in which its value is 0, 1, or 2 for 'No response', 'Partial', or 'Completed', respectively. Note: If an incorrect event_id or instrument name is used or if the instrument has not been enabled as a survey, then an error will be returned.</returns>
        public async Task<string> ExportSurveyParticipantsAsync(string token, string instrument, string eventName, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.ParticipantList.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// Export a Survey Participant List
        /// This method returns the list of all participants for a specific survey instrument (and for a specific event, if a longitudinal project). If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project, an error will be returned.
        /// </summary>
        /// <remarks>
        /// 
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">participantList</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns the list of all participants for the specified survey instrument [and event] in the desired format. The following fields are returned: email, email_occurrence, identifier, invitation_sent_status, invitation_send_time, response_status, survey_access_code, survey_link. The attribute 'email_occurrence' represents the current count that the email address has appeared in the list (because emails can be used more than once), thus email + email_occurrence represent a unique value pair. 'invitation_sent_status' is '0' if an invitation has not yet been sent to the participant, and is '1' if it has. 'invitation_send_time' is the date/time in which the next invitation will be sent, and is blank if there is no invitation that is scheduled to be sent. 'response_status' represents whether the participant has responded to the survey, in which its value is 0, 1, or 2 for 'No response', 'Partial', or 'Completed', respectively. Note: If an incorrect event_id or instrument name is used or if the instrument has not been enabled as a survey, then an error will be returned.</returns>
        public async Task<string> ExportSurveyParticipantsAsync(string token, Content content, string instrument, string eventName, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// 
        /// Export a Survey Queue Link for a Participant
        /// This method returns a unique Survey Queue link (i.e., a URL) in plain text format for the specified record in a project that is utilizing the Survey Queue feature. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the Survey Queue feature has not been enabled in the project, an error will be
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns a unique Survey Queue link (i.e., a URL) in plain text format for the specified record in the project.</returns>
        public async Task<string> ExportSurveyQueueLinkAsync(string token, string record, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.SurveyQueueLink.GetDisplayName() },
                    { "record", record },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0 
        /// 
        /// Export a Survey Queue Link for a Participant
        /// This method returns a unique Survey Queue link (i.e., a URL) in plain text format for the specified record in a project that is utilizing the Survey Queue feature. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the Survey Queue feature has not been enabled in the project, an error will be
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">surveyQueueLink</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns a unique Survey Queue link (i.e., a URL) in plain text format for the specified record in the project.</returns>
        public async Task<string> ExportSurveyQueueLinkAsync(string token, Content content, string record, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                this.CheckToken(token);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "record", record },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export a Survey Return Code for a Participant
        /// This method returns a unique Return Code in plain text format for a specified record and data collection instrument (and event, if longitudinal) in a project. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project or does not have the 'Save and Return Later' feature enabled, an error will be returned.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns a unique Return Code in plain text format for the specified record and instrument (and event, if longitudinal).</returns>
        public async Task<string> ExportSurveyReturnCodeAsync(string token, string record, string instrument, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.SurveyReturnCode.GetDisplayName() },
                    { "record", record },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "repeat_instance", repeatInstance.ToString()},
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 6.4.0
        /// Export a Survey Return Code for a Participant
        /// This method returns a unique Return Code in plain text format for a specified record and data collection instrument (and event, if longitudinal) in a project. If the user does not have 'Manage Survey Participants' privileges, they will not be able to use this method, and an error will be returned. If the specified data collection instrument has not been enabled as a survey in the project or does not have the 'Save and Return Later' feature enabled, an error will be returned.
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">surveyReturnCode</param>
        /// <param name="record">the record ID. The name of the record in the project.</param>
        /// <param name="instrument">the unique instrument name as seen in the second column of the Data Dictionary. This instrument must be enabled as a survey in the project.</param>
        /// <param name="eventName">the unique event name (for longitudinal projects only).</param>
        /// <param name="repeatInstance">(only for projects with repeating instruments/events) The repeat instance number of the repeating event (if longitudinal) or the repeating instrument (if classic or longitudinal). Default value is '1'.</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Returns a unique Return Code in plain text format for the specified record and instrument (and event, if longitudinal).</returns>
        public async Task<string> ExportSurveyReturnCodeAsync(string token, Content content, string record, string instrument, string eventName, string repeatInstance, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "record", record },
                    { "instrument", instrument },
                    { "event", eventName },
                    { "repeat_instance", repeatInstance.ToString()},
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Surveys
        #region Users & User Privileges
        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Export Users
        /// This method allows you to export the list of users for a project, including their user privileges and also email address, first name, and last name. Note: If the user has been assigned to a user role, it will return the user with the role's defined privileges. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>The method will return all the attributes below with regard to user privileges in the format specified. Please note that the 'forms' attribute is the only attribute that contains sub-elements (one for each data collection instrument), in which each form will have its own Form Rights value (see the key below to learn what each numerical value represents). Most user privilege attributes are boolean (0=No Access, 1=Access). Attributes returned:
        /// username, email, firstname, lastname, expiration, data_access_group, design, user_rights, data_access_groups, data_export, reports, stats_and_charts, manage_survey_participants, calendar, data_import_tool, data_comparison_tool, logging, file_repository, data_quality_create, data_quality_execute, api_export, api_import, mobile_app, mobile_app_download_data, record_create, record_rename, record_delete, lock_records_customization, lock_records, lock_records_all_forms, forms</returns>
        /// <example>
        /// KEY: Data Export: 0=No Access, 2=De-Identified, 1=Full Data Set
        /// Form Rights: 0=No Access, 2=Read Only, 1=View records/responses and edit records(survey responses are read-only), 3=Edit survey responses
        /// Other attribute values: 0=No Access, 1=Access.
        /// </example>
        public async Task<string> ExportUsersAsync(string token, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.User.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Export Users
        /// This method allows you to export the list of users for a project, including their user privileges and also email address, first name, and last name. Note: If the user has been assigned to a user role, it will return the user with the role's defined privileges. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Export privileges in the project.
        /// </remarks>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">user</param>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>The method will return all the attributes below with regard to user privileges in the format specified. Please note that the 'forms' attribute is the only attribute that contains sub-elements (one for each data collection instrument), in which each form will have its own Form Rights value (see the key below to learn what each numerical value represents). Most user privilege attributes are boolean (0=No Access, 1=Access). Attributes returned:
        /// username, email, firstname, lastname, expiration, data_access_group, design, user_rights, data_access_groups, data_export, reports, stats_and_charts, manage_survey_participants, calendar, data_import_tool, data_comparison_tool, logging, file_repository, data_quality_create, data_quality_execute, api_export, api_import, mobile_app, mobile_app_download_data, record_create, record_rename, record_delete, lock_records_customization, lock_records, lock_records_all_forms, forms</returns>
        /// <example>
        /// KEY: Data Export: 0=No Access, 2=De-Identified, 1=Full Data Set
        /// Form Rights: 0=No Access, 2=Read Only, 1=View records/responses and edit records(survey responses are read-only), 3=Edit survey responses
        /// Other attribute values: 0=No Access, 1=Access.
        /// </example>
        public async Task<string> ExportUsersAsync(string token, Content content = Content.User, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0 
        /// 
        /// Import Users
        /// This method allows you to import new users into a project while setting their user privileges, or update the privileges of existing users in the project. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* User Rights privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="data">
        /// Contains the attributes of the user to be added to the project or whose privileges in the project are being updated, in which they are provided in the specified format. All values should be numerical with the exception of username, expiration, data_access_group, and forms. Please note that the 'forms' attribute is the only attribute that contains sub-elements (one for each data collection instrument), in which each form will have its own Form Rights value (see the key below to learn what each numerical value represents). Most user privilege attributes are boolean (0=No Access, 1=Access). Please notice the distinction between data_access_group (contains the unique DAG name for a user) and data_access_groups (denotes whether the user has access to the Data Access Groups page).
        /// Missing attributes: If a user is being added to a project in the API request, then any attributes not provided for a user in the request(including form-level rights) will automatically be given the minimum privileges(typically 0=No Access) for the attribute/privilege.However, if an existing user's privileges are being modified in the API request, then any attributes not provided will not be modified from their current value but only the attributes provided in the request will be modified.
        /// Data Export: 0=No Access, 2=De-Identified, 1=Full Data Set
        /// Form Rights: 0=No Access, 2=Read Only, 1=View records/responses and edit records(survey responses are read-only), 3=Edit survey responses
        /// Other attribute values: 0=No Access, 1=Access.
        /// All available attributes:
        /// username, expiration, data_access_group, design, user_rights, data_access_groups, data_export, reports, stats_and_charts, manage_survey_participants, calendar, data_import_tool, data_comparison_tool, logging, file_repository, data_quality_create, data_quality_execute, api_export, api_import, mobile_app, mobile_app_download_data, record_create, record_rename, record_delete, lock_records_customization, lock_records, lock_records_all_forms, forms
        /// </param>
        /// <example>
        /// JSON Example:
        /// [{"username":"harrispa","expiration":"","data_access_group":"","design":"1","user_rights":"1","data_access_groups":"1","data_export":"1","reports":"1","stats_and_charts":"1",
        /// "manage_survey_participants":"1","calendar":"1","data_import_tool":"1","data_comparison_tool":"1",
        /// "logging":"1","file_repository":"1","data_quality_create":"1","data_quality_execute":"1",
        /// "api_export":"1","api_import":"1","mobile_app":"1","mobile_app_download_data":"0","record_create":"1",
        /// "record_rename":"0","record_delete":"0","lock_records_all_forms":"0","lock_records":"0",
        /// "lock_records_customization":"0","forms":{"demographics":"1","day_3":"1","other":"1"}
        /// },{"username":"taylorr4","expiration":"2015-12-07","data_access_group":"","design":"0",
        /// "user_rights":"0","data_access_groups":"0","data_export":"2","reports":"1","stats_and_charts":"1",
        /// "manage_survey_participants":"1","calendar":"1","data_import_tool":"0",
        /// "data_comparison_tool":"0","logging":"0","file_repository":"1","data_quality_create":"0",
        /// "data_quality_execute":"0","api_export":"0","api_import":"0","mobile_app":"0",
        /// "mobile_app_download_data":"0","record_create":"1","record_rename":"0","record_delete":"0",
        /// "lock_records_all_forms":"0","lock_records":"0","lock_records_customization":"0",
        /// "forms":{"demographics":"1","day_3":"2","other":"0"}}]
        /// </example>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of users added or updated</returns>
        public async Task<string> ImportUsersAsync<T>(string token, List<T> data, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                // Handle formats
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", Content.User.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }

        /// <summary>
        /// API Version 1.0.0+
        /// From Redcap Version 4.7.0
        /// 
        /// Import Users
        /// This method allows you to import new users into a project while setting their user privileges, or update the privileges of existing users in the project. 
        /// </summary>
        /// <remarks>
        /// To use this method, you must have API Import/Update privileges *and* User Rights privileges in the project.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The API token specific to your REDCap project and username (each token is unique to each user for each project). See the section on the left-hand menu for obtaining a token for a given project.</param>
        /// <param name="content">user</param>
        /// <param name="data">
        /// Contains the attributes of the user to be added to the project or whose privileges in the project are being updated, in which they are provided in the specified format. All values should be numerical with the exception of username, expiration, data_access_group, and forms. Please note that the 'forms' attribute is the only attribute that contains sub-elements (one for each data collection instrument), in which each form will have its own Form Rights value (see the key below to learn what each numerical value represents). Most user privilege attributes are boolean (0=No Access, 1=Access). Please notice the distinction between data_access_group (contains the unique DAG name for a user) and data_access_groups (denotes whether the user has access to the Data Access Groups page).
        /// Missing attributes: If a user is being added to a project in the API request, then any attributes not provided for a user in the request(including form-level rights) will automatically be given the minimum privileges(typically 0=No Access) for the attribute/privilege.However, if an existing user's privileges are being modified in the API request, then any attributes not provided will not be modified from their current value but only the attributes provided in the request will be modified.
        /// Data Export: 0=No Access, 2=De-Identified, 1=Full Data Set
        /// Form Rights: 0=No Access, 2=Read Only, 1=View records/responses and edit records(survey responses are read-only), 3=Edit survey responses
        /// Other attribute values: 0=No Access, 1=Access.
        /// All available attributes:
        /// username, expiration, data_access_group, design, user_rights, data_access_groups, data_export, reports, stats_and_charts, manage_survey_participants, calendar, data_import_tool, data_comparison_tool, logging, file_repository, data_quality_create, data_quality_execute, api_export, api_import, mobile_app, mobile_app_download_data, record_create, record_rename, record_delete, lock_records_customization, lock_records, lock_records_all_forms, forms
        /// </param>
        /// <example>
        /// JSON Example:
        /// [{"username":"harrispa","expiration":"","data_access_group":"","design":"1","user_rights":"1","data_access_groups":"1","data_export":"1","reports":"1","stats_and_charts":"1",
        /// "manage_survey_participants":"1","calendar":"1","data_import_tool":"1","data_comparison_tool":"1",
        /// "logging":"1","file_repository":"1","data_quality_create":"1","data_quality_execute":"1",
        /// "api_export":"1","api_import":"1","mobile_app":"1","mobile_app_download_data":"0","record_create":"1",
        /// "record_rename":"0","record_delete":"0","lock_records_all_forms":"0","lock_records":"0",
        /// "lock_records_customization":"0","forms":{"demographics":"1","day_3":"1","other":"1"}
        /// },{"username":"taylorr4","expiration":"2015-12-07","data_access_group":"","design":"0",
        /// "user_rights":"0","data_access_groups":"0","data_export":"2","reports":"1","stats_and_charts":"1",
        /// "manage_survey_participants":"1","calendar":"1","data_import_tool":"0",
        /// "data_comparison_tool":"0","logging":"0","file_repository":"1","data_quality_create":"0",
        /// "data_quality_execute":"0","api_export":"0","api_import":"0","mobile_app":"0",
        /// "mobile_app_download_data":"0","record_create":"1","record_rename":"0","record_delete":"0",
        /// "lock_records_all_forms":"0","lock_records":"0","lock_records_customization":"0",
        /// "forms":{"demographics":"1","day_3":"2","other":"0"}}]
        /// </example>
        /// <param name="format">csv, json [default], xml</param>
        /// <param name="onErrorFormat">csv, json, xml - specifies the format of error messages. If you do not pass in this flag, it will select the default format for you passed based on the 'format' flag you passed in or if no format flag was passed in, it will default to 'json'.</param>
        /// <returns>Number of users added or updated</returns>
        public async Task<string> ImportUsersAsync<T>(string token, Content content, List<T> data, ReturnFormat format = ReturnFormat.json, OnErrorFormat onErrorFormat = OnErrorFormat.json)
        {
            try
            {
                /*
                 * Check the required parameters for empty or null
                 */
                if (IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Please provide a valid Redcap token.");
                }
                // Handle formats
                var _serializedData = JsonConvert.SerializeObject(data);
                var payload = new Dictionary<string, string>
                {
                    { "token", token },
                    { "content", content.GetDisplayName() },
                    { "format", format.GetDisplayName() },
                    { "returnFormat", onErrorFormat.GetDisplayName() },
                    { "data", _serializedData }
                };

                return await this.SendPostRequestAsync(payload, _uri);
            }
            catch (Exception Ex)
            {
                /*
                 * We'll just log the error and return the error message.
                 */
                Log.Error($"{Ex.Message}");
                return Ex.Message;
            }
        }
        #endregion Users & User Privileges

    }
}

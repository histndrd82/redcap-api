using Moq;
using Newtonsoft.Json;
using Redcap;
using Redcap.Interfaces;
using Redcap.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    /// <summary>
    /// Simplified demographics instrument that we can test with.
    /// </summary>
    public class RedcapInstrument
    {
        [JsonRequired]
        [JsonProperty("record_id")]
        public string RecordId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
    /// <summary>
    /// Redcap Api Unit Tests
    /// </summary>
    public class RedcapUnitTests
    {
        // FAKE API Token
        private const string _token = "A8E6949EF4380F1111C66D5374E1AE6C";

        #region Helpers
        private static string GetToken()
        {
            return _token;
        }
        private RedcapInstrument GetRedcapInstrument()
        {
            return new RedcapInstrument { FirstName = "Red", LastName = "Cap", RecordId = "1" };
        }
        private List<RedcapArm> CreateArms()
        {
            return new List<RedcapArm> {
                new RedcapArm {ArmNumber = "1", Name = "arm1" },
                new RedcapArm{ArmNumber = "2", Name ="arm2" }
            };
        }
        private List<RedcapEvent> CreateEvents(int returnCount)
        {
            var events = new List<RedcapEvent>
            {
                new RedcapEvent{ ArmNumber = "1", EventName = "Event 1"},
                new RedcapEvent { ArmNumber = "2", EventName = "Event 2" },
                new RedcapEvent{ ArmNumber = "3", EventName = "Event 3"}
            };
            if(returnCount == 0)
            {
                return events;
            }
            return events.Take(returnCount).ToList();
        }
        #endregion
        #region ExportArmsAsync
        [Fact]
        public async void ExportArmsAsync_WithContentType_ShouldReturn_Arms()
        {
            var arms = "[{\"arm_num\":1,\"name\":\"Arm 1\"}]";
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportArmsAsync(_token, Content.Arm, ReturnFormat.json, null, OnErrorFormat.json)).Returns(Task.FromResult(arms));
            var api = mock.Object;

            // Act
            var result = await api.ExportArmsAsync(_token, Content.Arm);

            // Assert
            Assert.Contains("arm_num\":1", result);

            // Verify
            mock.Verify(x => x.ExportArmsAsync(_token, Content.Arm, ReturnFormat.json, null, OnErrorFormat.json), Times.AtLeastOnce());
        }
        [Fact]
        public async void ExportArmsAsync_ShouldReturn_Arms()
        {
            var arms = "[{\"arm_num\":1,\"name\":\"Arm 1\"}]";
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportArmsAsync(_token, ReturnFormat.json, null, OnErrorFormat.json)).Returns(Task.FromResult(arms));
            var api = mock.Object;

            // Act
            var result = await api.ExportArmsAsync(_token);

            // Assert
            Assert.Contains("arm_num\":1", result);

            // Verify
            mock.Verify(x => x.ExportArmsAsync(_token, ReturnFormat.json, null, OnErrorFormat.json), Times.AtLeastOnce());
        }
        #endregion ExportArmsAsync

        #region ImportArmsAsync
        [Fact]
        public async void ImportArmsAsync_ShouldReturn_Num_Arms()
        {
            var arms = CreateArms();
            var armNumbers = arms.Select(x => x.ArmNumber).ToList();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ImportArmsAsync(_token, Override.True, RedcapAction.Import, ReturnFormat.json, arms, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(arms.Count)));
            var api = mock.Object;

            // Act
            var armResult = await api.ImportArmsAsync(GetToken(), Override.True, RedcapAction.Import, ReturnFormat.json, arms, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(armResult);
            // Assert
            Assert.Contains("2", result.ToString());
        }
        [Fact]
        public async void ImportArmsAsyncWithContent_ShouldReturn_Num_Arms()
        {
            var arms = CreateArms();
            var armNumbers = arms.Select(x => x.ArmNumber).ToList();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ImportArmsAsync<RedcapArm>(GetToken(), Content.Arm, Override.True, RedcapAction.Import, ReturnFormat.json, arms, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(arms.Count)));
            var api = mock.Object;

            // Act
            var armResult = await api.ImportArmsAsync<RedcapArm>(GetToken(), Content.Arm, Override.True, RedcapAction.Import, ReturnFormat.json, arms, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(armResult);
            // Assert
            Assert.Contains("2", result.ToString());
        }
        #endregion

        #region DeleteArmsAsync
        [Fact]
        public async void DeleteArmsAsync_ShouldReturn_Num_Arms()
        {
            var arms = CreateArms();
            var armNumbers = arms.Select(x => x.ArmNumber).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.DeleteArmsAsync(GetToken(), armNumbers))
                .Returns(Task.FromResult(JsonConvert.SerializeObject(arms.Count)));
            var api = mock.Object;

            // Act
            var armResult = await api.DeleteArmsAsync(GetToken(), armNumbers);
            var result = JsonConvert.DeserializeObject(armResult);

            // Assert
            Assert.Contains("2", result.ToString());
        }
        [Fact]
        public async void DeleteArmsAsyncWithContent_ShouldReturn_Num_Arms()
        {
            var arms = CreateArms();
            var armNumbers = arms.Select(x => x.ArmNumber).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.DeleteArmsAsync(GetToken(), Content.Arm, RedcapAction.Delete, armNumbers)).Returns(Task.FromResult(JsonConvert.SerializeObject(arms.Count)));
            var api = mock.Object;

            // Act
            var armResult = await api.DeleteArmsAsync(GetToken(), Content.Arm, RedcapAction.Delete, armNumbers);
            var result = JsonConvert.DeserializeObject(armResult);

            // Assert
            Assert.Contains("2", result.ToString());
        }
        #endregion

        #region ExportEventsAsync
        [Fact]
        public async void ExportEventsAsyncWithContent_ShouldReturn_Multiple_Events()
        {
            // create 3 events
            var events = CreateEvents(3);
            var arms = new string[] { };
            var eventNames = events.Select(x => x.EventName).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportEventsAsync(GetToken(), Content.Event, ReturnFormat.json, arms, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(eventNames.Length)));
            var api = mock.Object;

            // Act
            var eventResult = await api.ExportEventsAsync(GetToken(), Content.Event, ReturnFormat.json, arms, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(eventResult);

            // Assert
            Assert.Contains("3", result.ToString());

        }
        [Fact]
        public async void ExportEventsAsync_ShouldReturn_Multiple_Events()
        {
            // create 3 events
            var events = CreateEvents(3);
            var arms = new string[] { };
            var eventNames = events.Select(x => x.EventName).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportEventsAsync(GetToken(), ReturnFormat.json, arms, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(eventNames.Length)));
            var api = mock.Object;

            // Act
            var eventResult = await api.ExportEventsAsync(GetToken(), ReturnFormat.json, arms, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(eventResult);

            // Assert
            Assert.Contains("3", result.ToString());

        }
        #endregion
        #region ImportEventsAsync
        [Fact]
        public async void ImportEventsAsync_ShouldReturn_Single_Count()
        {
            // create 3 events
            var events = CreateEvents(1);
            var arms = new string[] { "testArm" };
            var eventNames = events.Select(x => x.EventName).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ImportEventsAsync(GetToken(), Override.False, ReturnFormat.json, events, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(eventNames.Length)));
            var api = mock.Object;

            // Act
            var eventResult = await api.ImportEventsAsync(GetToken(), Override.False, ReturnFormat.json, events, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(eventResult);

            // Assert
            Assert.Contains("1", result.ToString());

        }
        [Fact]
        public async void ImportEventsAsync_ShouldReturn_Multiple_Counts()
        {
            // create 3 events
            var events = CreateEvents(2);
            var arms = new string[] { "testArm" };
            var eventNames = events.Select(x => x.EventName).ToArray();
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ImportEventsAsync(GetToken(), Override.False, ReturnFormat.json, events, OnErrorFormat.json)).Returns(Task.FromResult(JsonConvert.SerializeObject(eventNames.Length)));
            var api = mock.Object;

            // Act
            var eventResult = await api.ImportEventsAsync(GetToken(), Override.False, ReturnFormat.json, events, OnErrorFormat.json);
            var result = JsonConvert.DeserializeObject(eventResult);

            // Assert
            Assert.Contains("2", result.ToString());

        }
        #endregion
        #region ExportRedcapVersionAsync
        [Fact]
        public async void ExportRedcapVersionAsync_ShouldReturn_RedcapVersion()
        {
            // Arrange
            const string version = "8.10.15";
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportRedcapVersionAsync(GetToken(), ReturnFormat.json)).Returns(Task.FromResult(version));
            var api = mock.Object;

            // Act
            var result = await api.ExportRedcapVersionAsync(GetToken(), ReturnFormat.json);

            // Assert
            Assert.Contains("8.10.15", result);
        }
        [Theory]
        [InlineData("8.10.16", "8.10.16")]
        [InlineData("8.10.17", "8.10.17")]
        [InlineData("8.10.18", "8.10.18")]
        public async void ExportRedcapVersionAsync_ShouldReturn_RedcapVersions(string input, string expected)
        {
            // Arrange
            var version = input;
            // Mocking API Call and results
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportRedcapVersionAsync(GetToken(), ReturnFormat.json)).Returns(Task.FromResult(version));
            var api = mock.Object;

            // Act
            var result = await api.ExportRedcapVersionAsync(GetToken(), ReturnFormat.json);

            // Assert
            Assert.Contains(expected, result);
        }
        #endregion ExportRedcapVersionAsync

    }
}

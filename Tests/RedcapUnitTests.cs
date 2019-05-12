using Moq;
using Newtonsoft.Json;
using Redcap;
using Redcap.Interfaces;
using Redcap.Models;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    /// <summary>
    /// Simplified demographics instrument that we can test with.
    /// </summary>
    public class MyDemographicInstrument
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
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x=>x.ExportArmsAsync(_token, ReturnFormat.json, null, OnErrorFormat.json)).Returns(Task.FromResult(arms));
            var api = mock.Object;

            // Act
            var result = await api.ExportArmsAsync(_token);

            // Assert
            Assert.Contains("arm_num\":1", result);

            // Verify
            mock.Verify(x => x.ExportArmsAsync(_token, ReturnFormat.json, null, OnErrorFormat.json), Times.AtLeastOnce());
        }
        #endregion ExportArmsAsync
        #region ExportRedcapVersionAsync
        [Fact]
        public async void ExportRedcapVersionAsync_ShouldReturn_RedcapVersion()
        {
            // Arrange
            const string version = "8.10.15";
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportRedcapVersionAsync(_token, ReturnFormat.json)).Returns(Task.FromResult(version));
            var api = mock.Object;

            // Act
            var result = await api.ExportRedcapVersionAsync(_token, ReturnFormat.json);

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
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportRedcapVersionAsync(_token, ReturnFormat.json)).Returns(Task.FromResult(version));
            var api = mock.Object;

            // Act
            var result = await api.ExportRedcapVersionAsync(_token, ReturnFormat.json);

            // Assert
            Assert.Contains(expected, result);
        }
        #endregion ExportRedcapVersionAsync

    }
}

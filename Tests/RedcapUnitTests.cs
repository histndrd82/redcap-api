using Moq;
using Newtonsoft.Json;
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

        [Fact]
        public void ExportRedcapVersionAsync_ShouldReturn_RedcapVersion()
        {
            // Arrange
            const string version = "8.10.15";
            var mock = new Mock<IRedcapApi>();
            mock.Setup(x => x.ExportRedcapVersionAsync(_token, ReturnFormat.json)).Returns(Task.FromResult(version));
            var api = mock.Object;

            // Act
            var result = api.ExportRedcapVersionAsync(_token, ReturnFormat.json).Result;

            // Assert
            Assert.Contains("8.10.15", result);
        }


    }
}

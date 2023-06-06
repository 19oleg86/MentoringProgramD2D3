using System.Text.Json.Serialization;

namespace Categories.WebApi.Models.Output
{
    public class OutputErrorDataType
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}

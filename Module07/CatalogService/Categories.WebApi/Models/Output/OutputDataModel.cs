using System.Text.Json.Serialization;

namespace Categories.WebApi.Models.Output
{
    public class OutputDataModel<TData>
    {
        [JsonPropertyName("isError")]
        public bool IsError { get; protected set; }

        [JsonPropertyName("data")]
        public TData Data { get; }

        public OutputDataModel(TData data)
        {
            Data = data;
            IsError = false;
        }
    }
}

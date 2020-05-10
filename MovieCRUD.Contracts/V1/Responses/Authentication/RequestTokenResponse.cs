using Newtonsoft.Json;

namespace MovieCRUD.Contracts.V1.Responses.Authentication
{
    public class RequestTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty(".issued")]
        public string TokenIssueDate { get; set; }

        [JsonProperty(".expires")]
        public string TokenExpires { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreHttpTest.Models
{
    public class GitHubBranch
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        //[JsonPropertyName("commit")]
        //public IEnumerable<GitHubCommits>? Commits { get; set; }

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }

        //public class GitHubCommits
        //{
        //    [JsonPropertyName("sha")]
        //    public string SHA { get; set; }
        //    [JsonPropertyName("url")]
        //    public string URL { get; set; }
        //}

    }

    public class BasicModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BasicModel(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        public IEnumerable<GitHubBranch>? GitHubBranches { get; set; }

        public async Task OnGet()
        {
            var httpRequestMessage =
                new HttpRequestMessage( HttpMethod.Get,
                "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches")
                {
                    Headers ={
                        { HeaderNames.Accept, "application/vnd.github.v3+json" },
                        { HeaderNames.UserAgent, "HttpRequestsSample" }
                    }
                };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                GitHubBranches = await JsonSerializer.DeserializeAsync
                    <IEnumerable<GitHubBranch>>(contentStream);
            }
        }
    }
}

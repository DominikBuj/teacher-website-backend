using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Models;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Authorize]
    [Route("api/orcid")]
    [ApiController]
    public class OrcidController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrcidController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<ActionResult<string>> GetInformation(string accessToken, string orcidId, string endpoint)
        {
            UriBuilder uriBuilder = new UriBuilder($"https://pub.orcid.org/v3.0/{orcidId}/{endpoint}");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/orcid+json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uriBuilder.Uri);
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) return BadRequest();
            return Ok(responseString);
        }

        [HttpGet("record/{accessToken}/{orcidId}")]
        public Task<ActionResult<string>> GetRecord(string accessToken, string orcidId)
        {
            return GetInformation(accessToken, orcidId, "record");
        }

        [HttpGet("works/{accessToken}/{orcidId}")]
        public Task<ActionResult<string>> GetWorks(string accessToken, string orcidId)
        {
            return GetInformation(accessToken, orcidId, "works");
        }

        [HttpPost]
        public async Task<ActionResult<OrcidAccessTokenResponse>> GetAccessToken([FromBody] AccessTokenRequest accessTokenRequest)
        {
            UriBuilder uriBuilder = new UriBuilder("https://orcid.org/oauth/token");
            uriBuilder.Query =
                $"client_id={accessTokenRequest.client_id}&" +
                $"client_secret={accessTokenRequest.client_secret}&" +
                $"grant_type={accessTokenRequest.grant_type}&" +
                $"code={accessTokenRequest.code}&" +
                $"redirect_uri={accessTokenRequest.redirect_uri}";
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(accessTokenRequest), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uriBuilder.Uri, httpContent);
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) return BadRequest();
            OrcidAccessTokenResponse accessTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OrcidAccessTokenResponse>(responseString);
            return Ok(accessTokenResponse);
        }
    }
}

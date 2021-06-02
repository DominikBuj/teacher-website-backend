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
    [Route("api/linkedIn")]
    [ApiController]
    public class LinkedInController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LinkedInController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("profile/{accessToken}")]
        public async Task<ActionResult> GetLinkedInProfileInformation(string accessToken)
        {
            UriBuilder uriBuilder = new UriBuilder("https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uriBuilder.Uri);
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode) return BadRequest();

            return Ok(responseString);
        }

        [HttpGet("email/{accessToken}")]
        public async Task<ActionResult> GetLinkedInEmail(string accessToken)
        {
            UriBuilder uriBuilder = new UriBuilder("https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uriBuilder.Uri);
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode) return BadRequest();

            return Ok(responseString);
        }

        [HttpPost]
        public async Task<ActionResult> GetLinkedInAccessToken([FromBody] AccessTokenRequest accessTokenRequest)
        {
            UriBuilder uriBuilder = new UriBuilder("https://www.linkedin.com/oauth/v2/accessToken");
            uriBuilder.Query =
                $"client_id={accessTokenRequest.client_id}&" +
                $"client_secret={accessTokenRequest.client_secret}&" +
                $"grant_type={accessTokenRequest.grant_type}&" +
                $"code={accessTokenRequest.code}&" +
                $"redirect_uri={accessTokenRequest.redirect_uri}";
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(accessTokenRequest), Encoding.UTF8, "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uriBuilder.Uri, httpContent);
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode) return BadRequest();

            LinkedInAccessTokenResponse accessTokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LinkedInAccessTokenResponse>(responseString);
            return Ok(accessTokenResponse);
        }
    }
}

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend.Services
{
    public class RequestsHandler
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly string secret_key = Environment.GetEnvironmentVariable("APP_SECRET_KEY")
            ?? throw new Exception("APP_SECRET_KEY not set.");
        private readonly string base_api = Environment.GetEnvironmentVariable("BASE_API")
            ?? throw new Exception("BASE_API not set.");




        public RequestsHandler(HttpClient http, IHttpContextAccessor httpContext)
        {
            _http = http;
            _httpcontext= httpContext;

            var token = httpContext.HttpContext?.Request.Cookies[".AccessToken"];

            //Create HTTP Request
            _http.BaseAddress = new Uri(base_api);
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            _http.DefaultRequestHeaders.Add("X-PattareeyaApp-Secret", secret_key);
        }

        //GET METHOD
        public async Task<TResponse?> GetAsync<TResponse>(string url)
        {
            var token = _httpcontext.HttpContext?.Request.Cookies[".AccessToken"];

            _http.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            _http.DefaultRequestHeaders.Add("X-PattareeyaApp-Secret", secret_key);

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        //POST
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            //Send Request
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // ส่ง request เป็น JSON
            var response = await _http.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        //PUT
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest request)
        {
            //Send Request
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // ส่ง request เป็น JSON
            var response = await _http.PutAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        //Delete :  Set flag insteed
        public async Task<TResponse?> DeleteAsync<TRequest, TResponse>(string url, TRequest request)
        {
            //Send Request
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // ส่ง request เป็น JSON
            var response = await _http.PutAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}

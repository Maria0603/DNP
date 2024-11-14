using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiContracts;

namespace BlazorApp.Services {
    public class HttpPostService : IPostService {
        private readonly HttpClient _httpClient;

        public HttpPostService(HttpClient httpClient) {
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri("https://localhost:7197/"); // Ensure this is correct
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsAsync() {
            HttpResponseMessage response = await _httpClient.GetAsync("posts");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<GetPostDto>>(
                responseContent,
                new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<GetPostDto> GetPostAsync(int postId) {
            HttpResponseMessage response =
                await _httpClient.GetAsync($"posts/{postId}");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetPostDto>(responseContent,
                new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<GetPostDto> CreatePostAsync(CreatePostDto request) {
            string requestJson = JsonSerializer.Serialize(request);
            StringContent content =
                new(requestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await _httpClient.PostAsync("posts", content);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetPostDto>(responseContent,
                new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true })!;
        }

        public async Task AddCommentAsync(CreateCommentDto request) {
            string requestJson = JsonSerializer.Serialize(request);
            StringContent content =
                new(requestJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await _httpClient.PostAsync("comments", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService {
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /*public async Task<UserDto> AddUserAsync(CreateUserDto request) {
        string requestJson = JsonSerializer.Serialize(request);
        StringContent content = new(requestJson, Encoding.UTF8, "application/json");

        Console.WriteLine("Username http: " + request.Username + "; Password: " + request.Password);

        HttpResponseMessage response = await _httpClient.PostAsync("User", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.Content},         {response.ReasonPhrase},      {content}");
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        UserDto receivedDto =
            JsonSerializer.Deserialize<UserDto>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        return receivedDto;
    }*/

    /*public async Task<UserDto> AddUserAsync(CreateUserDto request) {
        HttpResponseMessage httpResponse =
            await _httpClient.PostAsJsonAsync("User", request);
        /#1#/ string response = await httpResponse.Content.ReadAsStringAsync();
        // if (!httpResponse.IsSuccessStatusCode) {
        //     throw new Exception(response);
        // }
        //
        // return JsonSerializer.Deserialize<UserDto>(response,
        //     new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;#1#
        //return Task.FromResult(new UserDto() { Id = 1, Username = "test" }).Result;;
    }*/


    public async Task<GetUserDto> AddUserAsync(CreateUserDto request) {
        HttpResponseMessage httpResponse =
            await _httpClient.PostAsJsonAsync("User", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<GetUserDto>(response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public IQueryable<GetUserDto> GetUsers(string? username) {
        var users = _httpClient.GetFromJsonAsync<GetUserDto[]>("api/users").Result;
        return users.AsQueryable();
    }

    public async Task<GetUserDto> GetUserAsync(int id) {
        return await _httpClient.GetFromJsonAsync<GetUserDto>($"api/users/{id}");
    }

    public async Task DeleteUserAsync(int id) {
        var response = await _httpClient.DeleteAsync($"api/users/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request) {
        var response =
            await _httpClient.PutAsJsonAsync($"api/users/{id}", request);
        response.EnsureSuccessStatusCode();
    }
}
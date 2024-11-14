﻿using System.Security.Claims;
using System.Text.Json;
using System.Transactions;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;
    private string? cachedUserJson;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
        cachedUserJson = null;
    }

    public async Task Login(string username, string password)
    {
        Console.WriteLine("Username: " + username + " Password: " + password);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "Auth/login",
            new LoginRequestDto() { Username = username, Password = password });

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        GetUserDto userDto =
            JsonSerializer.Deserialize<GetUserDto>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

        string serialisedData = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser",
            serialisedData);

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim("Id", userDto.Id.ToString())
            //new Claim("DateOfBirth", userDto.DateOfBirth.ToString("yyyy-MM-dd"));
            // new Claim("IsAdmin", userDto.IsAdmin.ToString())
            // new Claim("IsModerator", userDto.IsModerator.ToString())
            // new Claim("Email", userDto.Email)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async void Logout()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser",
            "");
        cachedUserJson = null;
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new())));
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        if (string.IsNullOrEmpty(cachedUserJson))
        {
            try
            {
                cachedUserJson =
                    await jsRuntime.InvokeAsync<string>(
                        "sessionStorage.getItem",
                        "currentUser");
            }
            catch (Exception e)
            {
                return new AuthenticationState(new());
            }
        }

        if (string.IsNullOrEmpty(cachedUserJson))
            return new AuthenticationState(new());

        GetUserDto userDto =
            JsonSerializer.Deserialize<GetUserDto>(cachedUserJson)!;

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(claimsPrincipal);
    }
}
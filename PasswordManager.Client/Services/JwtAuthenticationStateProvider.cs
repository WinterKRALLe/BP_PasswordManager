using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace PasswordManager.Client.Services;

public class JwtAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient, UserState userState)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = JwtParser.ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtToken"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);

        // Set the Authorization header for HttpClient
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Optionally, store token in localStorage or secure storage
        jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);

        // Optionally, store user state
        userState.Username = authenticatedUser.Identity?.Name; // Store username or relevant user state
    }

    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
        httpClient.DefaultRequestHeaders.Authorization = null;

        userState.Username = null; // Clear user state

        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}

using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace PasswordManager.Client.Services;

public class AuthenticatedHttpClientHandler(IJSRuntime jsRuntime) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}
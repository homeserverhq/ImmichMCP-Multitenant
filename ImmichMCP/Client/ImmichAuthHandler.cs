using System.Threading;
using Microsoft.Extensions.Options;
using ImmichMCP.Configuration;

namespace ImmichMCP.Client;

public class ImmichAuthHandler : DelegatingHandler
{
    private static readonly AsyncLocal<string?> _currentAuth = new();

    private readonly ImmichOptions _options;

    public ImmichAuthHandler(IOptions<ImmichOptions> options)
    {
        _options = options.Value;
    }

    public static void SetCurrentAuth(string? authHeader)
    {
        _currentAuth.Value = authHeader;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = ExtractToken(_currentAuth.Value) ?? _options.ApiKey;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("x-api-key", token);
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static string? ExtractToken(string? rawAuth)
    {
        if (string.IsNullOrEmpty(rawAuth))
            return rawAuth;

        if (rawAuth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return rawAuth.Substring(7);
        if (rawAuth.StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
            return rawAuth.Substring(6);
        
        return rawAuth;
    }
}

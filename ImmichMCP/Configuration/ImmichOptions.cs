namespace ImmichMCP.Configuration;

/// <summary>
/// Configuration options for connecting to the Immich API.
/// </summary>
public class ImmichOptions
{
    /// <summary>
    /// Base URL of the Immich instance (e.g., http://immich-app:2283).
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// External URL for asset links (e.g., https://photos.example.com).
    /// If not set, BaseUrl is used.
    /// </summary>
    public string ExternalUrl { get; set; } = string.Empty;

    /// <summary>
    /// API key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Maximum page size for paginated requests.
    /// </summary>
    public int MaxPageSize { get; set; } = 100;

    /// <summary>
    /// Download mode: "url" returns URLs, "base64" returns encoded content.
    /// </summary>
    public string DownloadMode { get; set; } = "url";
}

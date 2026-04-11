using System.Net;
using FluentAssertions;
using RichardSzalay.MockHttp;
using ImmichMCP.Models.Search;
using ImmichMCP.Tests.Fixtures;

namespace ImmichMCP.Tests.Client;

public class ImmichClientSearchTests
{
    [Fact]
    public async Task SearchMetadataAsync_ReturnsResults_WhenSuccessful()
    {
        // Arrange
        var (client, handler) = MockHttpClientFactory.CreateMockClient();
        var searchResult = new
        {
            assets = new
            {
                total = 10,
                count = 2,
                items = new[]
                {
                    TestFixtures.CreateAsset(id: "asset-1"),
                    TestFixtures.CreateAsset(id: "asset-2")
                },
                nextPage = (string?)null
            }
        };

        handler.When(HttpMethod.Post, "*/search/metadata")
            .Respond("application/json", TestFixtures.ToJson(searchResult));

        // Act
        var result = await client.SearchMetadataAsync(new MetadataSearchRequest { Type = "IMAGE" });

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchMetadataAsync_ReturnsEmpty_WhenNoResults()
    {
        // Arrange
        var (client, handler) = MockHttpClientFactory.CreateMockClient();
        var searchResult = new
        {
            assets = new
            {
                total = 0,
                count = 0,
                items = Array.Empty<object>(),
                nextPage = (string?)null
            }
        };

        handler.When(HttpMethod.Post, "*/search/metadata")
            .Respond("application/json", TestFixtures.ToJson(searchResult));

        // Act
        var result = await client.SearchMetadataAsync(new MetadataSearchRequest());

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchMetadataAsync_SendsWithExifTrue_InRequestBody()
    {
        // Arrange
        var (client, handler) = MockHttpClientFactory.CreateMockClient();
        var searchResult = new
        {
            assets = new
            {
                total = 1,
                count = 1,
                items = new[] { TestFixtures.CreateAsset(id: "asset-1") },
                nextPage = (string?)null
            }
        };

        string? capturedRequestBody = null;
        handler.When(HttpMethod.Post, "*/search/metadata")
            .With(req =>
            {
                capturedRequestBody = req.Content!.ReadAsStringAsync().Result;
                return true;
            })
            .Respond("application/json", TestFixtures.ToJson(searchResult));

        var request = new MetadataSearchRequest { WithExif = true };

        // Act
        await client.SearchMetadataAsync(request);

        // Assert
        capturedRequestBody.Should().NotBeNull();
        capturedRequestBody.Should().Contain("\"withExif\":true");
    }

    [Fact]
    public async Task SmartSearchAsync_ReturnsResults_WhenSuccessful()
    {
        // Arrange
        var (client, handler) = MockHttpClientFactory.CreateMockClient();
        var searchResult = new
        {
            assets = new
            {
                total = 5,
                count = 5,
                items = new[]
                {
                    TestFixtures.CreateAsset(id: "asset-1"),
                    TestFixtures.CreateAsset(id: "asset-2")
                },
                nextPage = (string?)null
            }
        };

        handler.When(HttpMethod.Post, "*/search/smart")
            .Respond("application/json", TestFixtures.ToJson(searchResult));

        // Act
        var result = await client.SearchSmartAsync(new SmartSearchRequest { Query = "sunset on beach" });

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
    }
}

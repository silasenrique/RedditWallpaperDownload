using System.Text.Json.Serialization;

namespace RedditWall;


public record Favorites
{
    [JsonPropertyName("data")]
    public Data? Data { get; set; }
}

public record Data
{
    [JsonPropertyName("after")]
    public string? After { get; set; }

    [JsonPropertyName("before")]
    public string? Before { get; set; }

    [JsonPropertyName("children")]
    public List<Children>? Children { get; set; }
}

public record Children
{
    [JsonPropertyName("data")]
    public ChildrenData? Data { get; set; }
}

public record ChildrenData
{
    [JsonPropertyName("subreddit")]
    public string? SubReddit { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("url_overridden_by_dest")]
    public string? UrlOverriddenByDest { get; set; }
}
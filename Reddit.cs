using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;


namespace RedditWall;

public static class Reddit
{
    static void LogExit(string? message = null)
    {
        if (message == null)
        {
            System.Console.WriteLine(message);
        }

        Environment.Exit(0);
    }

    static async Task<RedditToken?> GetToken()
    {
        var client = new RestClient("https://www.reddit.com/api/v1/access_token")
        {
            Authenticator = new HttpBasicAuthenticator("WhqBV_kv4pmxfg", "SqQEsARyMGO73B0Lca-uGUPEI3_ZDw")
        };

        var request = new RestRequest();
        RestResponse response = new();
        request.AddObject(new
        {
            grant_type = "password",
            username = "",
            password = ""
        });

        try
        {

            response = await client.PostAsync(request);
        }
        catch (HttpRequestException ex)
        {
            LogExit($"Não foi possível autenticar! Erro: {ex.Message}");
        }

        return JsonSerializer.Deserialize<RedditToken>(response.Content!);
    }

    private static async Task<Favorites> RequestFavorites(string after)
    {
        var token = await GetToken();

        if (token == null)
        {
            LogExit();
        }

        var client = new RestClient("https://oauth.reddit.com");
        var request = new RestRequest($"/user/SilasEnrique/saved?limit=100&after={after}");
        request.AddHeader("Authorization", $"bearer {token!.AccessToken}");
        request.AddHeader("User-Agent", "wallpaper-download by SilasEnrique");

        var response = await client.GetAsync(request);

        return JsonSerializer.Deserialize<Favorites>(response.Content!)!; ;
    }

    public static async Task<Favorites> GetFavorites()
    {
        var favorites = new Favorites(
            new Data(new List<Children>())
        );

        do
        {
            var getFavorites = await RequestFavorites(favorites.Data!.After!);
            favorites.Data!.Children!.AddRange(getFavorites.Data!.Children!);
            favorites.Data.After = getFavorites.Data.After;
        } while (favorites.Data.After != null);

        return favorites;
    }
}

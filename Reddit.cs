using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;


namespace RedditWall
{
    public static class Reddit
    {
        private static async Task<RedditToken?> GetToken()
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
                System.Console.WriteLine($"Não foi possível autenticar! Erro: {ex.Message}");
                Environment.Exit(0);
            }

            return JsonSerializer.Deserialize<RedditToken>(response.Content!);
        }

        public static async Task GetFavorites()
        {
            var token = await GetToken();

            var client = new RestClient("https://oauth.reddit.com");
            var request = new RestRequest("/user/SilasEnrique/saved?limit=2");
            request.AddHeader("Authorization", $"bearer {token!.AccessToken}");
            request.AddHeader("User-Agent", "wallpaper-download by SilasEnrique");

            var content = await client.GetAsync(request);
            System.Console.WriteLine(content.Content);
        }
    }
}
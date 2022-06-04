using System.Xml.Linq;
using RestSharp;

namespace RedditWall;

public static class Wallpaper
{
    static IEnumerable<string> GetWallpapersAlreadyDownloaded()
    {
        return from num in Directory.GetFiles("/home/senrique/Imagens/Wall")
               select num.Substring(num.LastIndexOf("/") + 1).
               Remove(num.Substring(num.LastIndexOf("/") + 1).
               LastIndexOf("."));
    }

    static async Task<IEnumerable<UrlFavorite>> GetNewWallpapers()
    {
        var favorites = await RedditWall.Reddit.GetFavorites();
        var favUrl = new List<UrlFavorite>();

        favorites!.Data!.Children!.RemoveAll(x => x.Data!.SubReddit != "WidescreenWallpaper" || x.Data.GalleryItems != null);
        favorites.Data.Children.ForEach(
            x =>
            {
                favUrl.Add(new UrlFavorite
                {
                    Url = x.Data!.UrlOverriddenByDest,
                    Name = x.Data.Title!.Replace(" ", "_")
                });
            });

        return favUrl;
    }

    static async Task<byte[]?> DownloadNewWallpapers(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest("/");

        return await client.DownloadDataAsync(request);
    }

    static void SaveWallpaper(string file, byte[] content)
    {
        File.WriteAllBytes($"/home/senrique/Imagens/Wall/{file}", content);
    }

    static List<UrlFavorite> RemoveWallAlreadyDownloaded(IEnumerable<UrlFavorite> newWalls)
    {
        return newWalls.Where(x => !GetWallpapersAlreadyDownloaded().Contains(x.Name)).ToList();
    }

    public static async Task Download()
    {
        var walls = RemoveWallAlreadyDownloaded(await GetNewWallpapers());
        var rand = new Random();
        var intList = new List<int>();
        var wallsCount = walls.Count();

        for (int i = 0; i < 10; i++)
        {
            var randomInt = rand.Next(wallsCount - 1);
            intList.Add(randomInt);

            System.Console.WriteLine(intList[i]);
        }

        // randomIntArray.ToList().ForEach(x => { System.Console.WriteLine(x); });
    }
}

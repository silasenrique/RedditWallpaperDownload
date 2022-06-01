// await RedditWall.Reddit.GetFavorites();

using System.Text.Json;
using RedditWall;

var hml = JsonSerializer.Deserialize<Favorites>(File.ReadAllText("./json.json"));
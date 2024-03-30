using ImxHoldersByAsset.Models;
using Newtonsoft.Json;

namespace ImxHoldersByAsset.Services;

public interface IImxApiService
{
    Task<List<Result>> GetHoldersByCollection(string assetId);
}

public class ImxApiService : IImxApiService
{
    private readonly HttpClient _client;

    public ImxApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Result>> GetHoldersByCollection(string assetId)
    {
        try
        {
            var response = await _client.GetAsync($"assets?collection={assetId}&page_size=200");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            AssetsResponse assets = JsonConvert.DeserializeObject<AssetsResponse>(json)!;
            var cursor = assets.cursor;
            var remaining = assets.remaining;
            var users = new List<Result>();

            
            users.AddRange(assets.result);

            while (!string.IsNullOrEmpty(cursor) && remaining == 1)
            {
                Task.Delay(500).Wait();
                var responseTemp = await _client.GetAsync($"assets?collection={assetId}&cursor={cursor}&page_size=200");
                responseTemp.EnsureSuccessStatusCode();
                var jsonTemp = await responseTemp.Content.ReadAsStringAsync();
                var assetsTemp = JsonConvert.DeserializeObject<AssetsResponse>(jsonTemp)!;

                cursor = assetsTemp.cursor;
                remaining = assetsTemp.remaining;

                users.AddRange(assetsTemp.result);
            }

            return users;
            //var users = assets?.result.Select(r => r.user).Distinct().ToList();
            

        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}

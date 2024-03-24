using ImxHoldersByAsset.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ImxHoldersByAsset.Services;

public interface IImxApiService
{
    Task<List<Result>> GetHoldersByCollection(string assetId, bool distinctUsers = true);
}

public class ImxApiService : IImxApiService
{
    private readonly HttpClient _client;

    public ImxApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Result>> GetHoldersByCollection(string assetId, bool distinctUsers = true)
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

            if (distinctUsers)
            {
                var temp = assets?.result.Distinct(new MyObjectComparer()).ToList();
                users.AddRange(temp);
                //return string.Join(", ", users);
            }
            else
            {
                users.AddRange(assets.result);
            }

            while (!string.IsNullOrEmpty(cursor) && remaining == 1)
            {
                Task.Delay(500).Wait();
                var responseTemp = await _client.GetAsync($"assets?collection={assetId}&cursor={cursor}&page_size=200");
                responseTemp.EnsureSuccessStatusCode();
                var jsonTemp = await responseTemp.Content.ReadAsStringAsync();
                var assetsTemp = JsonConvert.DeserializeObject<AssetsResponse>(jsonTemp)!;

                cursor = assetsTemp.cursor;
                remaining = assetsTemp.remaining;

                if (distinctUsers)
                {
                    var temp = assets?.result.Distinct(new MyObjectComparer()).ToList();
                    users.AddRange(temp);
                    //return string.Join(", ", users);
                }
                else
                {
                    users.AddRange(assets.result);
                }
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

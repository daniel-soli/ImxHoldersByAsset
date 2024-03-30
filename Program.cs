using ImxHoldersByAsset.Models;
using ImxHoldersByAsset.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ImxHoldersByAsset;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Kira!");

        //setup our DI
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHttpClient<IImxApiService, ImxApiService>(c => c.BaseAddress = new Uri("https://api.x.immutable.com/v1/"));
        var serviceProvider = services.BuildServiceProvider();

        var imxApiService = serviceProvider.GetService<IImxApiService>();

        Console.WriteLine("Enter the collection address (0x...):");
        var assetId = Console.ReadLine();

        Console.WriteLine("Do you want distinct users? (Y/N)");
        var distinctUsers = Console.ReadLine().ToLower() == "y";

        Console.WriteLine("Fetching holders...");

        var holders = await imxApiService.GetHoldersByCollection(assetId);

        //holders = holders.Distinct(new CompareAsset()).ToList();

        holders = holders.OrderBy(h => h.token_id).ToList();

        if (distinctUsers)
        {
            holders = holders.Distinct(new CompareUser()).ToList();
        } 

        Console.WriteLine("Do you want to save the holders to a file? (Y/N)");
        var saveToFile = Console.ReadLine().ToLower() == "y";

        if (saveToFile)
        {

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? @"c:\temp\kira";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = $"Holders_{assetId}.txt";
            filePath = Path.Combine(path, filePath);

            var holdersToFile = holders.Select(h => h.user).ToList();

            File.WriteAllLines(filePath, holdersToFile);
            Console.WriteLine($"Holders saved to {filePath}");
        }
        if (!saveToFile)
        {
            Console.WriteLine(holders.Count + " holders found");
            Console.WriteLine("Holders:");
            Console.WriteLine(string.Join(", ", holders.Select(h => h.user)));
        }


        Console.WriteLine("Press key to exit");
        Console.ReadKey();

    }
}

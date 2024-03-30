using Newtonsoft.Json;

namespace ImxHoldersByAsset.Models;

public class AssetsResponse
{
    public AssetsResponse(List<Result> results, string cursor, int remaining)
    {
        result = results;
        this.cursor = cursor;
        this.remaining = remaining;
    }
    public List<Result> result { get; set; } 
    public string cursor { get; set; }
    public int remaining { get; set; }
}

public class Result(string token_address, string token_id, string id, string user, string status, string uri, string name)
{
    public string token_address { get; set; } = token_address;
    public string token_id { get; set; } = token_id;
    public string id { get; set; } = id;
    public string user { get; set; } = user;
    public string status { get; set; } = status;
    public string uri { get; set; } = uri;
    public string name { get; set; } = name;
}

public class Collection
{
    public Collection()
    {
        // Parameterless constructor
    }
    public string name { get; set; }
    public string icon_url { get; set; }
}

class CompareUser : IEqualityComparer<Result>
{
    public bool Equals(Result x, Result y)
    {
        // Check if the PropertyToCompare values are equal
        return x.user == y.user;
    }

    public int GetHashCode(Result obj)
    {
        // Return the hash code of the PropertyToCompare value
        return obj.user.GetHashCode();
    }
}

class CompareAsset : IEqualityComparer<Result>
{
    public bool Equals(Result x, Result y)
    {
        // Check if the PropertyToCompare values are equal
        return x.token_id == y.token_id;
    }

    public int GetHashCode(Result obj)
    {
        // Return the hash code of the PropertyToCompare value
        return obj.token_id.GetHashCode();
    }
}
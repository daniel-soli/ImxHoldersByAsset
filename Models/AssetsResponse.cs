using Newtonsoft.Json;

namespace ImxHoldersByAsset.Models;

public class AssetsResponse
{
    public AssetsResponse(List<Result> results, string cursor, int remaining)
    {
        Result = results;
        Cursor = cursor;
        Remaining = remaining;
    }
    public List<Result> Result { get; set; }
    public string Cursor { get; set; }
    public int Remaining { get; set; }
}

public class Result
{
    public Result(string token_address, string token_id, string id, string user, string status, string uri, string name)
    {
        Token_address = token_address;
        Token_id = token_id;
        Id = id;
        User = user;
        Status = status;
        Uri = uri;
        Name = name;
        // Parameterless constructor
    }
    public string Token_address { get; set; }
    public string Token_id { get; set; }
    public string Id { get; set; }
    public string User { get; set; }
    public string Status { get; set; }
    public string Uri { get; set; }
    public string Name { get; set; }
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

class MyObjectComparer : IEqualityComparer<Result>
{
    public bool Equals(Result x, Result y)
    {
        // Check if the PropertyToCompare values are equal
        return x.User == y.User;
    }

    public int GetHashCode(Result obj)
    {
        // Return the hash code of the PropertyToCompare value
        return obj.User.GetHashCode();
    }
}
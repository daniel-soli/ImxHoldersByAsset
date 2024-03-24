using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImxHoldersByAsset.Models;

public class AssetsResponse
{
    public List<Result> result { get; set; }
    public string cursor { get; set; }
    public int remaining { get; set; }
}

public class Result
{
    public string token_address { get; set; }
    public string token_id { get; set; }
    public string id { get; set; }
    public string user { get; set; }
    public string status { get; set; }
    public string uri { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string image_url { get; set; }
    public Metadata metadata { get; set; }
    public Collection collection { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

public class Metadata
{
    public string Body { get; set; }
    public string Eyes { get; set; }
    public long date { get; set; }
    public string name { get; set; }
    public string Halos { get; set; }
    public string Masks { get; set; }
    public string image { get; set; }
    public int edition { get; set; }
    public string Clothing { get; set; }
    public string Background { get; set; }
    public string Foundation { get; set; }
    public string description { get; set; }
    public string Kabutos { get; set; }
    public string KabutoMasks { get; set; }
    public string Balds { get; set; }
    public string Elements { get; set; }
    public string Vetements { get; set; }
    public string BaldMasks { get; set; }
    public string Bandanas { get; set; }
    public string BandanaMasks { get; set; }
}

public class Collection
{
    public string name { get; set; }
    public string icon_url { get; set; }
}

class MyObjectComparer : IEqualityComparer<Result>
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
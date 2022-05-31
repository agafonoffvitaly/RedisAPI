using RedisAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.Services;

public class RedisPlatformRepository : IPlatformRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatformRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void CreatePlatform(Platform plat)
    {
        if (plat == null)
        { 
        throw new ArgumentNullException(nameof(plat));
        }

        var db = _redis.GetDatabase(1);
        var serialPlat = JsonSerializer.Serialize(plat);

        //For Strings key-value
        //db.StringSet(plat.Id, serialPlat);
        
        //For Sets Key: id-value, id-value
        //db.SetAdd("PlatformSet", serialPlat);

        //For Hash
        db.HashSet("hashplatform", new HashEntry[]
            { new HashEntry(plat.Id, serialPlat)});

    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase(1);

        //For Sets Key: id-value, id-value
        //var completeSet = db.SetMembers("PlatformSet");
        //if (completeSet.Length > 0)
        //{
        //    var obj = Array.ConvertAll(completeSet, val =>
        //        JsonSerializer.Deserialize<Platform>(val)).ToList();

        //    return obj;
        //}



        //For Hash
        var completeHash = db.HashGetAll("hashplatform");


        if (completeHash.Length > 0)
        {
            var obj = Array.ConvertAll(completeHash, val =>
                JsonSerializer.Deserialize<Platform>(val.Value)).ToList();

            return obj;
        }

        return null;
    }

    public Platform? GetPlatformBuId(string Id)
    {
        var db = _redis.GetDatabase(1);

        //var plat = db.StringGet(Id);
        var plat = db.HashGet("hashplatform", Id);

        if (!string.IsNullOrEmpty(plat))
        {
            return JsonSerializer.Deserialize<Platform>(plat);
        }

        return null;
    }
}

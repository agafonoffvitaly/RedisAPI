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

        var db = _redis.GetDatabase();
        var serialPlat = JsonSerializer.Serialize(plat);

        db.StringSet(plat.Id, serialPlat);

        db.SetAdd("PlatformSet", serialPlat);

    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase();

        var completeSet = db.SetMembers("PlatformSet");

        if (completeSet.Length > 0)
        {
            var obj = Array.ConvertAll(completeSet, val =>
                JsonSerializer.Deserialize<Platform>(val)).ToList();

            return obj;
        }

        return null;
    }

    public Platform? GetPlatformBuId(string Id)
    {
        var db = _redis.GetDatabase();

        var plat = db.StringGet(Id);

        if (!string.IsNullOrEmpty(plat))
        {
            return JsonSerializer.Deserialize<Platform>(plat);
        }

        return null;
    }
}

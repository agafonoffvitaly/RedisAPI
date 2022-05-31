using RedisAPI.Models;

namespace RedisAPI.Services
{
    public interface IPlatformRepository
    {
        void CreatePlatform(Platform plat);
        Platform? GetPlatformBuId(string Id);
        IEnumerable<Platform?>? GetAllPlatforms();
    }
}

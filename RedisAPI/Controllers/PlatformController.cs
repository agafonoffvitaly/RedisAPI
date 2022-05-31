using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisAPI.Models;
using RedisAPI.Services;

namespace RedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository _repository;

        public PlatformController(IPlatformRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{Id}",Name = "GetPlatformBuId")]
        public ActionResult<Platform> GetPlatformBuId(string Id)
        {
            var plat = _repository.GetPlatformBuId(Id);

            if (plat == null)
                return NotFound();

            return Ok(plat);
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _repository.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformBuId), new { Id = platform.Id }, platform);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
        {
            var platformList = _repository.GetAllPlatforms();
            return Ok(platformList);
        }
    }
}

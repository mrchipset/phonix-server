using Microsoft.Extensions.Caching.Memory;

namespace phonix_api.Services
{
    public class MemoryCacheService : MemoryCache
    {
        public const long CACHE_LIMIT_SIZE = 1024;
        public MemoryCacheService() : base(new MemoryCacheOptions(){
            SizeLimit = CACHE_LIMIT_SIZE
        })
        {
            
        }
    }
}
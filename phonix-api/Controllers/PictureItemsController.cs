using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

using phonix_api.Services;
using System.Text;

namespace phonix_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureItemsController : ControllerBase
    {

        private readonly ILogger<PictureItemsController> _logger;
        private MemoryCache _cache;
        public PictureItemsController(ILogger<PictureItemsController> logger,
        MemoryCacheService cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            // Array a;
            if (_cache.TryGetValue(key, out byte[] cacheEntry))
            {
                var contentTypeStr = "image/jpeg";
                return new FileContentResult(cacheEntry, contentTypeStr);
            }
            return Ok(new { status = true, message = "Student Posted Successfully"});
        }

        [HttpPost]
        public ActionResult post([FromForm] PictureFormModel pictureForm)
        {
            string id  = pictureForm.key;
            var picture = pictureForm.data;
            // Saving Image on Server
            if (picture.Length > 0) {
                using (var ms = new MemoryStream())
                {
                picture.CopyTo(ms);
                var fileBytes = ms.ToArray();
                // Key not in cache, so get data.

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Set cache entry size by extension method.
                    .SetSize(1)
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300));

                // Set cache entry size via property.
                // cacheEntryOptions.Size = 1;

                // Save data in cache.
                _cache.Set(picture.Name, fileBytes, cacheEntryOptions);
                } 
            }  
            return Ok(new { status = true, message = picture.Name});
        
        }
    }
}

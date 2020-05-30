using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

using phonix_api.Services;
using phonix_api.Models;
namespace phonix_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureItemsController : ControllerBase
    {

        private readonly ILogger<PictureItemsController> _logger;
        private readonly MemoryCache _cache;
        private readonly PhonixDbContext _dbContext;
        public PictureItemsController(ILogger<PictureItemsController> logger,
        MemoryCacheService cache,
        PhonixDbContext dbContext)
        {
            _logger = logger;
            _cache = cache;
            _dbContext = dbContext;
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
                // TODO Save the file on disk
                _cache.Set(picture.Name, fileBytes, cacheEntryOptions);
                PictureItem pictureItem = new PictureItem
                {
                    Path = picture.Name
                };
                _dbContext.PictureItems.Add(pictureItem);
                _dbContext.SaveChangesAsync();
                } 
                
            }  
            return Ok(new { status = true, message = picture.Name});
        
        }

        [HttpPost]
        public ActionResult put([FromForm] PictureFormModel pictureForm)
        {
            string id  = pictureForm.key;
            var picture = pictureForm.data;
            //TODO Implement put method to update the picture.
            throw new NotImplementedException();
            //  // Saving Image on Server
            // if (picture.Length > 0) {
            //     using (var ms = new MemoryStream())
            //     {
            //     picture.CopyTo(ms);
            //     var fileBytes = ms.ToArray();
            //     // Key not in cache, so get data.

            //     var cacheEntryOptions = new MemoryCacheEntryOptions()
            //         // Set cache entry size by extension method.
            //         .SetSize(1)
            //         // Keep in cache for this time, reset time if accessed.
            //         .SetSlidingExpiration(TimeSpan.FromSeconds(300));

            //     // Set cache entry size via property.
            //     // cacheEntryOptions.Size = 1;

            //     // Save data in cache.
            //     // TODO Save the file on disk
            //     _cache.Set(picture.Name, fileBytes, cacheEntryOptions);
            //     } 
            // }  
            // return Ok(new { status = true, message = picture.Name});
        }

        [HttpDelete("{key}")]
        public ActionResult delete(string key)
        {
            _cache.Remove(key);
            return Ok(new { status = true });
        }

        [HttpGet("list/{size}")]
        public ActionResult getList(int size)
        {
            throw new NotImplementedException();
        }
    }
}

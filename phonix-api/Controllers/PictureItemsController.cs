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
        private readonly FileStorageService _fileStorageService;
        public PictureItemsController(ILogger<PictureItemsController> logger,
        MemoryCacheService cache,
        PhonixDbContext dbContext,
        FileStorageService fileStorage)
        {
            _logger = logger;
            _cache = cache;
            _dbContext = dbContext;
            _fileStorageService = fileStorage;
        }

        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var contentTypeStr = "image/jpeg";
            if (_cache.TryGetValue(key, out byte[] cacheEntry))
            {
                return new FileContentResult(cacheEntry, contentTypeStr);
            }
            try
            {
                var item = _dbContext.PictureItems
                    .Single(o => o.Summary == key);
                _fileStorageService.Get(item.Path, out MemoryStream stream);
                if (stream != null)
                {
                    var fileBytes = stream.ToArray();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Set cache entry size by extension method.
                        .SetSize(1)
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(300));
                    // Set cache entry size via property.
                    cacheEntryOptions.Size = 1;
                    _cache.Set(key, fileBytes, cacheEntryOptions);
                    return new FileContentResult(fileBytes, contentTypeStr);
                }
           
            }catch(InvalidOperationException)
            {
                //Do Nothing
            }
            return NotFound(new { status = false, message = "Your requested is not existed."});
        }

        [HttpPost]
        public ActionResult Post([FromForm] PictureFormModel pictureForm)
        {
            string id  = pictureForm.key;
            var picture = pictureForm.data;
            // Saving Image on Server
            if (picture.Length > 0) {
                var ms = new MemoryStream();
                picture.CopyTo(ms);
                var fileBytes = ms.ToArray();
                // Key not in cache, so get data.

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Set cache entry size by extension method.
                    .SetSize(1)
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300));

                    // Set cache entry size via property.
                    cacheEntryOptions.Size = 1;
                // Save data.
                bool success = _fileStorageService.Create(ref ms, out string path, out string hashCode);
                if (success)
                {
                    _cache.Set(hashCode, fileBytes, cacheEntryOptions);
                    PictureItem pictureItem = new PictureItem
                    {
                        Path = path,
                        Summary = hashCode,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    };
                    _dbContext.PictureItems.Add(pictureItem);
                    _dbContext.SaveChangesAsync();
                    return Ok(new { status = true, url = hashCode });

                } else
                {
                    return BadRequest(new { status = false, message = "Alread Existed" });
                }
            }
            return BadRequest(new { status = false, message = "No Content Upload"});
        }

        [HttpPut]
        public ActionResult Put([FromForm] PictureFormModel pictureForm)
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
        public ActionResult Delete(string key)
        {
            _cache.Remove(key);
            return Ok(new { status = true });
        }

        [HttpGet("list/{size}")]
        public ActionResult GetList(int size)
        {
            throw new NotImplementedException();
        }
    }
}

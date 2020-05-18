using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace phonix_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureItemsController : ControllerBase
    {

        private readonly ILogger<PictureItemsController> _logger;

        public PictureItemsController(ILogger<PictureItemsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PictureItem> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new PictureItem
            {
                Date = DateTime.Now.AddDays(index),
                Owner = rng.Next(),
                Summary = "HelloString"
            })
            .ToArray();
        }
    }
}

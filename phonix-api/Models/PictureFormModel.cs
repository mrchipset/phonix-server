using System;
using Microsoft.AspNetCore.Http;

namespace phonix_api .Models                                                             
{
    public class PictureFormModel
    {
        // author key
        public string key {get; set;}
        // Image file
        public IFormFile data {get; set;}
        
    }
}

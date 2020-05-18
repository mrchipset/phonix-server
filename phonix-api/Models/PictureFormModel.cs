using System;
using Microsoft.AspNetCore.Http;

namespace phonix_api
{
    public class PictureFormModel
    {
        public string key {get; set;}
        public IFormFile data {get; set;}
        
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace phonix_api.Models
{
    public class PictureItem
    {
        [Key]
        public int id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate{ get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
        
    }
}

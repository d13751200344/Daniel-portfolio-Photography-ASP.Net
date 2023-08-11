using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Photo
    {
        // auto filled
        public int Id { get; set; }

        public int GalleryId { get; set; }

        public string? Title { get; set; }
        public string? Caption { get; set; }

        //[Display(Name = "Picture")]
        public string? Image { get; set; }

        public Gallery? Gallery { get; set; }
    }
}

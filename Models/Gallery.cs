using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Display(Name = "Gallery Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a Name.")]
        public string GalleryName { get; set; }

        public string? Description { get; set; }



        // navigation property
        public List<Photo>? Photos { get; set; } 
    }
}

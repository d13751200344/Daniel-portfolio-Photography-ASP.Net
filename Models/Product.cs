using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Photo uploading required.")]
        public string Photo { get; set; }



        //navigation property
        public Gallery? Gallery { get; set; }
    }
}

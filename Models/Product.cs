using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required()]
        public string Photo { get; set; }



        //navigation property
        public Gallery? Gallery { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Photography.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Display(Name ="Course Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a Course Name.")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide course description.")]
        public string CourseDescription { get; set; }

        [Required(ErrorMessage = "You must provide the course price.")]
        public decimal Price { get; set; }
    }
}

namespace Photography.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // foreign key (references Cart.Id)
        public int CartId { get; set; }

        // foreign key (references Product.Id)
        public int CourseId { get; set; }

        public decimal Price { get; set; }



        // Association reference
        //relationship to parent (many:1 = CartItem:Cart)
        public Cart? Cart { get; set; }

        //relationship to parent (many:1 = CartItem:Course)
        public Course? Course { get; set; }
    }
}
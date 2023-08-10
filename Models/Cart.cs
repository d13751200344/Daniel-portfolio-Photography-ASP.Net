namespace Photography.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // foreign key (references User.Id)
        public string UserId { get; set; }


        // To toggle the status of carts. When it's instantiated, it's active by default.
        public bool Active { get; set; } = true;


        /* Navigation property: relationship to parent (1:1) ; 
        ? means that the 'User' property can either hold a valid reference to a 'User' object or be null */
        public User? User { get; set; }

        //relationship to children (1:many)
        public List<CartItem>? CartItems { get; set; }

        //relationship to parent?child? (1:1)
        public Order? Order { get; set; }
    }
}

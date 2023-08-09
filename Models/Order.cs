namespace Photography.Models
{
    // create enum type
    public enum PaymentMethods
    {
        VISA,
        Mastercard,
        InteracDebit,
        PayPal,
        Stripe
    }

    public class Order
    {
        public int Id { get; set; }

        // foreign key (references User.Id)
        public string UserId { get; set; }

        // foreign key (references Cart.Id)
        public int CartId { get; set; }

        public decimal Total { get; set; }

        public string ShippingAddress { get; set; }

        public bool PaymentReceived { get; set; }

        // enum that create above
        public PaymentMethods PaymentMethod { get; set; }



        //relationship to parent (many:1 = Order:User)
        public User? User { get; set; }

        //relationship to parent?child? (1:1)
        public Cart? Cart { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace Photography.Models
{
    public class User : IdentityUser
    {
        // connect to mutiple cart & order
        public List<Cart>? Cart { get; set; }

        public List<Order>? Order { get; set; }
    }
}

namespace Amazon.API.Model.Domain
{
    public class Cart
    {


        //each cart has a unique identifier
        public Guid Id { get; set; }

        // User who owns the cart
        public string UserId { get; set; }

        // Navigation Property
        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();
    }
}

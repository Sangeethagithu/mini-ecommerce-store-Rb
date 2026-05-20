namespace Amazon.API.Model.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }

        // Quantity of product
        public int Quantity { get; set; }

        // Foreign Key → Product
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        // Foreign Key → Cart
        public Guid CartId { get; set; }

        public Cart Cart { get; set; }
    }
}
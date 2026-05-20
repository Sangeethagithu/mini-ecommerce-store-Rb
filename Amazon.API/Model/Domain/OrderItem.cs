namespace Amazon.API.Model.Domain
{
    public class OrderItem
    {

        public Guid Id { get; set; }

        // Quantity ordered
        public int Quantity { get; set; }

        // Product price at purchase time
        public decimal Price { get; set; }

        // Foreign Key → Product
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }
    }
}

namespace Amazon.API.Model.Domain
{
    public class Order
    {

        public Guid Id { get; set; }

        // User who placed order
        public string UserId { get; set; }

        // Total amount of order
        public decimal TotalAmount { get; set; }

        // Date and time of order
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

    }
}

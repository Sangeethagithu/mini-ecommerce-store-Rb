namespace Amazon.API.Model.Domain
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid CartId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }
    }
}
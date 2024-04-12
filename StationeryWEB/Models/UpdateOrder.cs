namespace StationeryWEB.Models
{
    public class UpdateOrder
    {
        public string OrderId { get; set; } = null!;
        public string productId { get; set; } = null!;

        public string CustomerId { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string OrderStatus { get; set; } = null!;

        public string DeliveryType { get; set; } = null!;
        public decimal DeliveryFee { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
    }
}

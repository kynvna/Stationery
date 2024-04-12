namespace StationeryAPI.ShoppingModels
{
    public class NewOrder
    {
        public string CustomerId { get; set; } = null!;
        public string productId { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public string OrderStatus { get; set; } = null!;
        public string DeliveryType { get; set; } = null!;
        public decimal DeliveryFee { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
    }
}

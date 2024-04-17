namespace StationeryAPI.ShoppingModels
{
    public class UpdateDelivery
    {
        public string DeliveryId { get; set; } = null!;

        public string OrderId { get; set; } = null!;

        public DateTime DeliveryDate { get; set; }

        public string DeliveryStatus { get; set; } = null!;

        public decimal? DeliveryFee { get; set; }

        public string? CarrierName { get; set; }
    }
}

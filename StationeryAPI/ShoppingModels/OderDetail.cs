namespace StationeryAPI.ShoppingModels
{
    public class OderDetail
    {
        public string OrderId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}

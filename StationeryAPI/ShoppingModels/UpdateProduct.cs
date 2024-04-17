namespace StationeryAPI.ShoppingModels
{
    public class UpdateProduct
    {
        public string ProName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? ProQuantity { get; set; }

        public string? ImageLink { get; set; }

        public bool ProStatus { get; set; }

        public string DealerId { get; set; }
        public string ProId { get; set; }
    }
}

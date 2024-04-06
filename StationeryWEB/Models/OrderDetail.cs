namespace StationeryWEB.Models
{
    public class OrderDetail
    {
        public string OrderId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}

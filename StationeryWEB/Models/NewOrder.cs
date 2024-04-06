namespace StationeryAPI.ShoppingModels
{
    public class NewOrder
    {
        public string CustomerId { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public string OrderStatus { get; set; } = null!;
    }
}

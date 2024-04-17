namespace StationeryAPI.ShoppingModels
{
    public class UpdateCustomerProduct
    {
        public string CustId { get; set; } = null!;


        public string? Fullname { get; set; }

        public string? Address { get; set; }
        public string? Tel { get; set; }
        public string ProId { get; set; } = null!;
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}

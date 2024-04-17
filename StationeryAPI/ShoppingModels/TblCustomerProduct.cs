namespace StationeryAPI.ShoppingModels
{
    public class TblCustomerProduct
    {
        public string CustomerId { get; set; }
        public virtual TblCustomer Customer { get; set; }
        public string ProductId { get; set; }
        public virtual TblProduct Product { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
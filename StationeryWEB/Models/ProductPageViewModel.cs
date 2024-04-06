using StationeryAPI.ShoppingModels;

namespace StationeryWEB.Models
{
    public class ProductPageViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<TblProduct> Products { get; set; }
    }
}

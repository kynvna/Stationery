
using StationeryAPI.ShoppingModels;

namespace StationeryWEB.Models
{
    public class OrderViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<TblOrder> Orders { get; set; }
    }

}

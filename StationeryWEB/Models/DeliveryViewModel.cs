using StationeryAPI.ShoppingModels;

namespace StationeryWEB.Models
{
    public class DeliveryViewModel
    {
        public int TotalDeliveries { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<TblDelivery> Deliveries { get; set; }


    }
}

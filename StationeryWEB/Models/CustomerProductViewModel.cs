using Newtonsoft.Json;

namespace StationeryWEB.Models
{
    public class CustomerProductViewModel
    {
        public int TotalCustomerProducts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        [JsonProperty("CustomerProducts")]
        public List<StationeryAPI.ShoppingModels.TblCustomerProduct> CustomerProducts { get; set; }
    }
}

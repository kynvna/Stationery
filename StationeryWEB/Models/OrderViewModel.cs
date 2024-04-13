using Newtonsoft.Json;
using System.Collections.Generic;

namespace StationeryWEB.Models
{
    //public class OrderViewModel
    //{
    //    public int TotalOrders { get; set; }
    //    public int TotalPages { get; set; }
    //    public int CurrentPage { get; set; }
    //    public int PageSize { get; set; }

    //    [JsonProperty("orders")]
    //    public OrderValues OrdersValues { get; set; }
    //}

    //public class OrderValues
    //{
    //    [JsonProperty("$values")]
    //    public List<StationeryAPI.ShoppingModels.TblOrder> Orders { get; set; }
    //}

    public class OrderViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        [JsonProperty("orders")]
        public List<StationeryAPI.ShoppingModels.TblOrder> Orders { get; set; }
    }
}

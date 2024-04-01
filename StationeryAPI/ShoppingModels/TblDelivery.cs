using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblDelivery
{
    public string DeliveryId { get; set; } = null!;

    public string ReviewId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public DateTime? DeliveryDate { get; set; }

    public string DeliveryStatus { get; set; } = null!;

    public decimal? DeliveryFee { get; set; }

    public string? CarrierName { get; set; }

    public virtual TblOrder Order { get; set; } = null!;
}

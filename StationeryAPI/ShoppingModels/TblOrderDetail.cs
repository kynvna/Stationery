using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblOrderDetail
{
    public string DetailId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual TblOrder Order { get; set; } = null!;

    public virtual TblProduct Product { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblCartItem
{
    public string ItemId { get; set; } = null!;

    public string CartId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public virtual TblCart Cart { get; set; } = null!;

    public virtual TblProduct Product { get; set; } = null!;
}

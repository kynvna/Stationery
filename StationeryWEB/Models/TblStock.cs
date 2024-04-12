using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblStock
{
    public string StockId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int StockQuantity { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual TblProduct Product { get; set; } = null!;
}

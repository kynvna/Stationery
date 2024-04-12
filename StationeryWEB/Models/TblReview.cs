using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblReview
{
    public string ReviewId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public decimal Rating { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string TimeCreated { get; set; } = null!;

    public virtual TblOrder Order { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblCart
{
    public string CartId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public DateTime DateCreate { get; set; }

    public DateTime? DateUpdate { get; set; }

    public virtual TblCustomer Customer { get; set; } = null!;

    public virtual ICollection<TblCartItem> TblCartItems { get; set; } = new List<TblCartItem>();
}

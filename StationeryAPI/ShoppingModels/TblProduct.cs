using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblProduct
{
    public string ProId { get; set; } = null!;
    public string dealerId { get; set; } 

    public string ProName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? ProQuantity { get; set; }

    public string? ImageLink { get; set; }

    public bool ProStatus { get; set; }

    public DateTime? TimeCreated { get; set; }

    public DateTime? TimeUpdated { get; set; }
    public string? CatId { get; set; }
    public virtual ICollection<TblCartItem> TblCartItems { get; set; } = new List<TblCartItem>();
    public virtual ICollection<TblOrder> Orders { get; set; }

    public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();

    public virtual TblStock? TblStock { get; set; }

    public virtual ICollection<TblCategory> Cats { get; set; } = new List<TblCategory>();
}

using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblProduct
{
    public string ProId { get; set; } = null!;

    public string ProName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? ProQuantity { get; set; }

    public string? ImageLink { get; set; }

    public bool ProStatus { get; set; }

    public DateTime? TimeCreated { get; set; }

    public DateTime? TimeUpdated { get; set; }

    public string DealerId { get; set; } = null!;

    public string? CatId { get; set; }



    public virtual TblCategory? Cat { get; set; }

    public virtual ICollection<TblCartItem> TblCartItems { get; set; } = new List<TblCartItem>();

    public virtual ICollection<TblOrder> TblOrders {
        get
        {
            ShoppingWebContext db = new ShoppingWebContext();
            var ls = db.TblOrders.Where(d => d.ProductId == ProId).ToList();
            return ls;
        }
    }

    public virtual TblStock? TblStock { get; set; }
}

using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblCategory
{
    public string CatId { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<TblProduct> Products { get; set; } = new List<TblProduct>();
}

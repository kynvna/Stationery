using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StationeryAPI.ShoppingModels;

public partial class TblOrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DetailId { get; set; } 

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual TblOrder Order { get; set; }

    public virtual TblProduct Product { get; set; }
}

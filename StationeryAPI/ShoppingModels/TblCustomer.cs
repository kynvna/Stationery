﻿using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblCustomer
{
    public string CustId { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<TblCustomerProduct> CustomerProducts { get; set; } = new List<TblCustomerProduct>();


    public string? Tel { get; set; }

    public bool Active { get; set; }

   // public virtual TblOrder Orders { get; set; }

    public virtual ICollection<TblCart> TblCarts { get; set; } = new List<TblCart>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}

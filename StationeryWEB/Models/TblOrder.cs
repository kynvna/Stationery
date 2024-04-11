using System;
using System.Collections.Generic;


namespace StationeryAPI.ShoppingModels;

public partial class TblOrder
{
 
    public string OrderId { get; set; } = null!;
    public string productId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string OrderStatus { get; set; } = null!;

    public string DeliveryType {  get; set; } = null!;
    public decimal DeliveryFee { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;

    public virtual TblProduct Product { get; set; }

    public virtual TblCustomer Customer { get; set; }

    public virtual ICollection<TblDelivery> TblDeliveries { get; set; } = new List<TblDelivery>();

    public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();

    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();
}

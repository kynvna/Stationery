using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace StationeryAPI.ShoppingModels;

public partial class TblOrder
{
    [Key]
    [Column("orderID")]
    public string OrderId { get; set; } = null!;
    [ForeignKey("Product")] // This ensures clarity on what it is referencing
    [Column("productId")]
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

    public virtual TblCustomer Customer { get; set; } = null!;

    public virtual ICollection<TblDelivery> TblDeliveries { get; set; } = new List<TblDelivery>();

    public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();
    //public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();
}

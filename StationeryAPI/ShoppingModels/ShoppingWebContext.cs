using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StationeryAPI.ShoppingModels;

public partial class ShoppingWebContext : DbContext
{
    public ShoppingWebContext()
    {
    }

    public ShoppingWebContext(DbContextOptions<ShoppingWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdEmpAccount> TblAdEmpAccounts { get; set; }

    public virtual DbSet<TblCart> TblCarts { get; set; }

    public virtual DbSet<TblCartItem> TblCartItems { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblDelivery> TblDeliveries { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblReview> TblReviews { get; set; }

    public virtual DbSet<TblStock> TblStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAP14404\\SQLEXPRESS;Initial Catalog=ShoppingWeb;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAdEmpAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_AdEmp_Account");

            entity.ToTable("tblAdEmpAccount");

            entity.Property(e => e.UserId)
                .HasMaxLength(32)
                .HasColumnName("userID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Passw)
                .HasMaxLength(100)
                .HasColumnName("passw");
            entity.Property(e => e.Role)
                .HasDefaultValue(1)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK_Cart");

            entity.ToTable("tblCart");

            entity.Property(e => e.CartId)
                .HasMaxLength(32)
                .HasColumnName("cartID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(32)
                .HasColumnName("customerID");
            entity.Property(e => e.DateCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_create");
            entity.Property(e => e.DateUpdate)
                .HasColumnType("datetime")
                .HasColumnName("date_update");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblCarts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Customer");
        });

        modelBuilder.Entity<TblCartItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_Cart_Item");

            entity.ToTable("tblCartItem");

            entity.Property(e => e.ItemId)
                .HasMaxLength(32)
                .HasColumnName("itemID");
            entity.Property(e => e.CartId)
                .HasMaxLength(32)
                .HasColumnName("cartID");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("discount");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId)
                .HasMaxLength(32)
                .HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.TblCartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Item_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.TblCartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Item_Product");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK_Category");

            entity.ToTable("tblCategory");

            entity.Property(e => e.CatId)
                .HasMaxLength(32)
                .HasColumnName("catID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CatName)
                .HasMaxLength(200)
                .HasColumnName("catName");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustId).HasName("PK_Customer");

            entity.ToTable("tblCustomer");

            entity.Property(e => e.CustId)
                .HasMaxLength(32)
                .HasColumnName("custID");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Passw)
                .HasMaxLength(100)
                .HasColumnName("passw");
            entity.Property(e => e.Tel)
                .HasMaxLength(12)
                .HasColumnName("tel");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblDelivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK_delivery");

            entity.ToTable("tblDelivery");

            entity.Property(e => e.DeliveryId)
                .HasMaxLength(32)
                .HasColumnName("deliveryID");
            entity.Property(e => e.CarrierName)
                .HasMaxLength(255)
                .HasColumnName("carrier_name");
            entity.Property(e => e.DeliveryDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("delivery_date");
            entity.Property(e => e.DeliveryFee)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("delivery_fee");
            entity.Property(e => e.DeliveryStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Dang Van Chuyen")
                .HasColumnName("delivery_status");
            entity.Property(e => e.OrderId)
                .HasMaxLength(32)
                .HasColumnName("orderID");
            entity.Property(e => e.ReviewId)
                .HasMaxLength(32)
                .HasColumnName("reviewID");

            entity.HasOne(d => d.Order).WithMany(p => p.TblDeliveries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_delivery_order");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Order");

            entity.ToTable("tblOrder");

            entity.Property(e => e.OrderId)
                .HasMaxLength(32)
                .HasColumnName("orderID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(32)
                .HasColumnName("customerID");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasDefaultValue("CREATED")
                .HasColumnName("order_status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<TblOrderDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK_Order_Detail");

            entity.ToTable("tblOrderDetail");

            entity.Property(e => e.DetailId)
                .HasMaxLength(32)
                .HasColumnName("detailID");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("discount");
            entity.Property(e => e.OrderId)
                .HasMaxLength(32)
                .HasColumnName("orderID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(32)
                .HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Order).WithMany(p => p.TblOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_DetailOrder");

            entity.HasOne(d => d.Product).WithMany(p => p.TblOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_DetailProduct");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK_Product");

            entity.ToTable("tblProduct");

            entity.Property(e => e.ProId)
                .HasMaxLength(32)
                .HasColumnName("proID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ImageLink)
                .HasMaxLength(500)
                .HasColumnName("image_link");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.ProName)
                .HasMaxLength(200)
                .HasColumnName("proName");
            entity.Property(e => e.ProQuantity).HasColumnName("proQuantity");
            entity.Property(e => e.ProStatus)
                .HasDefaultValue(true)
                .HasColumnName("proStatus");
            entity.Property(e => e.TimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("time_created");
            entity.Property(e => e.TimeUpdated)
                .HasColumnType("datetime")
                .HasColumnName("time_updated");

            entity.HasMany(d => d.Cats).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "TblCategoryProduct",
                    r => r.HasOne<TblCategory>().WithMany()
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cate_product_category"),
                    l => l.HasOne<TblProduct>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cate_product_product"),
                    j =>
                    {
                        j.HasKey("ProductId", "CatId").HasName("PK_cate_product");
                        j.ToTable("tblCategoryProduct");
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(32)
                            .HasColumnName("productID");
                        j.IndexerProperty<string>("CatId")
                            .HasMaxLength(32)
                            .HasColumnName("catID");
                    });
        });

        modelBuilder.Entity<TblReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK_product_reviews");

            entity.ToTable("tblReview");

            entity.Property(e => e.ReviewId)
                .HasMaxLength(32)
                .HasColumnName("reviewID");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .HasColumnName("content");
            entity.Property(e => e.OrderId)
                .HasMaxLength(32)
                .HasColumnName("orderID");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("rating");
            entity.Property(e => e.TimeCreated)
                .HasMaxLength(1)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("time_created");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Order).WithMany(p => p.TblReviews)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_reviews_oder");
        });

        modelBuilder.Entity<TblStock>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Stock");

            entity.ToTable("tblStock");

            entity.Property(e => e.ProductId)
                .HasMaxLength(32)
                .HasColumnName("productID");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime")
                .HasColumnName("last_updated");
            entity.Property(e => e.StockId)
                .HasMaxLength(32)
                .HasColumnName("stockID");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

            entity.HasOne(d => d.Product).WithOne(p => p.TblStock)
                .HasForeignKey<TblStock>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_StockToProduct");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

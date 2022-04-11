namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            CartDetails = new HashSet<CartDetail>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        public int? ProductPrice { get; set; }

        public int? Discount { get; set; }

        public int? SizeId { get; set; }

        public int? ColorId { get; set; }

        public int? PhotoId { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateAdded { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartDetail> CartDetails { get; set; }

        public virtual Category Category { get; set; }

        public virtual Color Color { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Photo Photo { get; set; }

        public virtual Size Size { get; set; }

        public virtual ProductSeller ProductSeller { get; set; }
    }
}

namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Size
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Size()
        {
            Products = new HashSet<Product>();
        }

        public int SizeId { get; set; }

        public int NumberSize { get; set; }

        [StringLength(50)]
        public string TextSize { get; set; }

        public bool? Status { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
        [DisplayName("MinSize")]
        public int? size1 { get; set; }
        [DisplayName("MaxSize")]
        public int? size2 { get; set; }

        public int? size3 { get; set; }

        public int? size4 { get; set; }

        public int? size5 { get; set; }

        public int? size6 { get; set; }

        public int? size7 { get; set; }

        public int? size8 { get; set; }

        public int? size9 { get; set; }

        public int? size10 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}

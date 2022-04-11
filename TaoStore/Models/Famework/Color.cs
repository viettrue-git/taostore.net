namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Color
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Color()
        {
            Products = new HashSet<Product>();
        }

        public int ColorId { get; set; }

        [StringLength(100)]
        public string ColorName { get; set; }

        public bool? Status { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(50)]
        public string name1 { get; set; }

        [StringLength(50)]
        public string name2 { get; set; }

        [StringLength(50)]
        public string name3 { get; set; }

        [StringLength(50)]
        public string name4 { get; set; }

        [StringLength(50)]
        public string name5 { get; set; }

        [StringLength(50)]
        public string name6 { get; set; }

        [StringLength(50)]
        public string name7 { get; set; }

        [StringLength(50)]
        public string name8 { get; set; }

        [StringLength(50)]
        public string name9 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}

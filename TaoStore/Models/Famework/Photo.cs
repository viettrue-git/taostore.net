namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Photo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Photo()
        {
            Products = new HashSet<Product>();
        }

        public int PhotoId { get; set; }

        [StringLength(100)]
        public string Photo1 { get; set; }

        [StringLength(100)]
        public string Photo2 { get; set; }

        [StringLength(100)]
        public string Photo3 { get; set; }

        public bool? Status { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}

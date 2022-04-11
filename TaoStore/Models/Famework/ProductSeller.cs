namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductSeller")]
    public partial class ProductSeller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int? Total { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdateEdit { get; set; }

        public virtual Product Product { get; set; }
    }
}

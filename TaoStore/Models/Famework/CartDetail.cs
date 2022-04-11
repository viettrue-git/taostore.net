namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CartDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int? ProductPrice { get; set; }

        public int? Discount { get; set; }

        public int? Quantity { get; set; }

        public int StatusCart { get; set; }
        public int Size { get; set; }

        public string Color { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }
    }
}

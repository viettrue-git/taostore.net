namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            // CategoryLogo = "~/Asset/Image/Category/not-available.jpg";
            Products = new HashSet<Product>();
        }
        public int CategoryId { get; set; }
        [DisplayName("Tên loại")]
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100")]
        public string CategoryName { get; set; }

        [DisplayName("Logo")]
        [StringLength(50, ErrorMessage = "Logo length can't be more than 50")]
        //[Required(ErrorMessage = "Vui lòng chọn file ảnh")]
        public string CategoryLogo { get; set; }

        [DisplayName("Tình trạng")]
        public bool? Status { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(50, ErrorMessage = "Note length can't be more than 500")]
        public string Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }


        [DisplayName("File Image")]
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}

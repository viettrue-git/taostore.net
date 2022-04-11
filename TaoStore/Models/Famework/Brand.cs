
namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Brand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int BrandId { get; set; }

        [Required(ErrorMessage = "vui lòng nhập tên thương hiệu")]
        [DisplayName("Tên thương hiệu")]
        [StringLength(100, ErrorMessage = "Tên thương hiệu không quá 100 kí tự!")]
        public string BrandName { get; set; }

        [StringLength(50)]
        //[Required(ErrorMessage ="Vui lòng chọn logo!")]
        [DisplayName("Nhãn hàng")]
        public string BrandLogo { get; set; }

        [DisplayName("Trạng thái")]
        public bool? Status { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500)]
        public string Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [DisplayName("File Image")]
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}

namespace Models.Famework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acount
    {
        public int AcountId { get; set; }

        [StringLength(100)]
        public string AcountName { get; set; }

        public int? AcountRole { get; set; }

        [Required(ErrorMessage = "please enter your email!")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage ="please enter your number phone!")]
        //[Phone]
        //[MaxLength(11)]
        public int Phone { get; set; }

        [StringLength(100)]
        public string Avatar { get; set; }

        [StringLength(50)]
        [MinLength(8,ErrorMessage = "Secure password should be longer than 8 characters!")]
        [Required]
        public string PassWord { get; set; }

        [NotMapped]
        [Required]
        [Compare("PassWord")]
        public string F_Password { get; set; }

    }
}

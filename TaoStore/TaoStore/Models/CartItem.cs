using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaoStore.Models
{
    [Serializable]
    public class CartItem
    {
        public Product product { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }

        public int Total
        {
            get
            {
                return (int)(Quantity * product.ProductPrice);
            }
        }
    }
}
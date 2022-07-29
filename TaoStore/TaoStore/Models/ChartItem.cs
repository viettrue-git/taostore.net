using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaoStore.Models
{
    [Serializable]
    public class ChartItem
    {
        public Product product { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }
}
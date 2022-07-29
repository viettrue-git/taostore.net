using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class SliderDao
    {
        private readonly OnlineShopDBContext context;
        public SliderDao()
        {
            context = new OnlineShopDBContext();
        }
    }
}

using Models.Famework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class BrandDao
    {
        private OnlineShopDBContext context;
        public BrandDao()
        {
            context = new OnlineShopDBContext();
        }
        public List<Brand> GetAll()
        {
            List<Brand> list = context.Brands.OrderByDescending(x => x.BrandName).ToList();
            return list;
        }
    }
}

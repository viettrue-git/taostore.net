using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Famework;

namespace Models.Dao
{
   public class CategoryDao
    {
        private OnlineShopDBContext context;
        public CategoryDao()
        {
            context = new OnlineShopDBContext();
        }
        public List<Category>  GetAll()
        {
            List<Category> list = context.Categories.OrderByDescending(x => x.CategoryName).ToList();
            return list;
        }
    }
}

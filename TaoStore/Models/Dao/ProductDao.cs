using Models.Famework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class ProductDao
    {
        private OnlineShopDBContext context;
        public ProductDao()
        {
            context = new OnlineShopDBContext();
        }
        public List<Product> ListProductNew()
        {
            List<Product> list = context.Products.OrderByDescending(x => x.DateAdded).ToList();
            return list;
        }
        public List<Product> ListProductBestSell()
        {
            List<Product> list = context.Products.OrderByDescending(x => x.ProductSeller.Total).ToList();
            return list;
        }
        public IEnumerable<Product> AllProduct(int page,int pageSize)
        {
            var list = context.Products.OrderByDescending(x => x.DateAdded).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public Product  FindProductById(int id)
        {
            var product = context.Products.Find(id);
            return product;
        }
        public List<Product> FetureCat(int id)
        {
            var product = context.Products.Find(id);
            var listfeture = context.Products.Where(x => x.CategoryId == product.CategoryId).Take(4).ToList();
            return listfeture;
        }
        public IEnumerable<Product> SearchProduct(string key)
        {
            List<Product> products = context.Products.Where(x => x.ProductName.Contains(key)).ToList();
            return products;
        }
        public IEnumerable<Product> ProductByCat(int id)
        {
            var list = context.Products.Where(x => x.CategoryId == id).ToList();
            return list;
        }
        public IEnumerable<Product> ProductByBrand(int id)
        {
            var list = context.Products.Where(x => x.BrandId == id).ToList();
            return list;
        }

    }
}

using Models.Famework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
     public class ProductModel
    {
        private OnlineShopDBContext context;
        public ProductModel()
        {
            context = new OnlineShopDBContext();
        }
        public IEnumerable<Product> GetAllProducts(int page, int pageSize)
        {
            var list = context.Products.OrderBy(x => x.DateAdded).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public Product Find(int idProuct)
        {
            var product = context.Products.Find(idProuct);
            return product;
        }
        public bool ChangeStatus(int idProduct)
        {
            try
            {
                Product product = context.Products.Find(idProduct);
                product.Status = !product.Status;
                context.SaveChanges();
                return (bool)!product.Status;
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool Delete(int idProduct)
        {
            Product product = context.Products.Find(idProduct);
            if (product == null)
            {
                return false;
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return true;
        }
        public Photo FindPhoto(int id)
        {
            var photo= context.Photos.Find(id);
            return photo;
        }

    }
}

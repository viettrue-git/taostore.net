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
    public class BrandModel
    {
        private OnlineShopDBContext context;
        public BrandModel()
        {
            context = new OnlineShopDBContext();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<Brand> AllBrands(int page,int pageSize)
        {
            var list = context.Brands.OrderBy(x => x.BrandName).ToList().ToPagedList(page, pageSize);
            return list;
        }
        /// <summary>
        /// Find brand by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Brand FindBrandById(int id)
        {
            var Cat = context.Brands.Find(id);
            return Cat;
        }
        public bool FindBrandByName(string name)
        {
            var brand = context.Brands.Where(x => x.BrandName == name).FirstOrDefault();
            if (brand == null) return false;
            else return true;
        }
        
        public bool ChangeStatus(int id)
        {
            try
            {
                Brand brand = context.Brands.Find(id);
                brand.Status = !brand.Status;
                context.SaveChanges();
                return (bool)!brand.Status;
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool Delete(int id)
        {
            Brand brand = context.Brands.Find(id);
            if (brand != null)
            {
                context.Brands.Remove(brand);
                context.SaveChanges();
                return true;
            }
            else return false;
        }
       
    }
}

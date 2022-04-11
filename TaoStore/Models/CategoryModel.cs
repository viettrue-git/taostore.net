using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Models.Famework;
using PagedList;
using PagedList.Mvc;

namespace Models
{
    public class CategoryModel
    {
        private OnlineShopDBContext context;
        public CategoryModel()
        {
            context = new OnlineShopDBContext();
        }
        public IEnumerable<Category> ListAllCategories(int page,int pageSize)
        {
            var list = context.Categories.OrderBy(x=>x.CategoryName).ToList().ToPagedList(page,pageSize);
            return list;
        }
        public Category Find(int CategoriId)
        {
            var Cat = context.Categories.Find(CategoriId);
            return Cat;
        }
        public bool Edit(Category category)
        {
            try
            {
               Category model = context.Categories.Where(x => x.CategoryId == category.CategoryId).FirstOrDefault();
                if (model == null)
                {
                    return false;
                }
                model.CategoryName = category.CategoryName;
                model.CategoryLogo = category.CategoryLogo;
                model.Status = category.Status;
                model.Note = category.Note;
                context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public bool ChangeStastus(int id)
        {
              
            try
            {
                Category category = context.Categories.Find(id);
                category.Status = !category.Status;
                context.SaveChanges();
                return (bool)!category.Status;
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool Delete(int id)
        {
            var category = context.Categories.Find(id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

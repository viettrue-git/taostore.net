using Models.Famework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class CustomerModel
    {
        private OnlineShopDBContext context;
        public CustomerModel()
        {
            context = new OnlineShopDBContext();
        }
        public IEnumerable<Customer> AllCustomers(int page, int pageSize)
        {
            var list = context.Customers.OrderBy(x => x.Name).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public int InsertWithFaceBook(Customer customer)
        {
            var result = context.Customers.Where(x => x.Email == customer.Email).FirstOrDefault();
            if (result == null)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer.CustomerId;
            }
            else
            {
                return result.CustomerId;
            }
        }
    }
}

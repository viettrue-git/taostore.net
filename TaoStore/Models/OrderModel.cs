using Models.Famework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderModel
    {
        private OnlineShopDBContext context;
        public OrderModel()
        {
            context = new OnlineShopDBContext();
        }
        public IEnumerable<Order> ListOrder(int page, int pageSize)
        {
            var list = context.Orders.OrderByDescending(x => x.OrderDate).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public Order Find(int id)
        {
            Order order = context.Orders.Find(id);
            return order;
        }
        public int ChangeStatus(int id)
        {
            var order = Find(id);
            if (order.StatusOrder < 2)
            {
                order.StatusOrder += 1;
            }
            context.SaveChanges();
            return order.StatusOrder;
        }
        public IEnumerable<Order> ListOrderWait(int page, int pageSize)
        {
            var list = context.Orders.Where(x=>x.StatusOrder==0).OrderBy(x => x.OrderDate).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public IEnumerable<Order> ListOrderLoad(int page, int pageSize)
        {
            var list = context.Orders.Where(x => x.StatusOrder == 1).OrderBy(x => x.OrderDate).ToList().ToPagedList(page, pageSize);
            return list;
        }
        public IEnumerable<Order> ListOrderSuccess(int page, int pageSize)
        {
            var list = context.Orders.Where(x => x.StatusOrder > 1).OrderBy(x => x.OrderDate).ToList().ToPagedList(page, pageSize);
            return list;
        }
    }
}

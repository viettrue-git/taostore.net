using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Famework;


namespace Models
{
    public class AcountModel
    {
        private OnlineShopDBContext context;
        public AcountModel()
        {
            context = new OnlineShopDBContext();
           
        }
        public bool Login(string userName,string passWord)
        {
            var res = context.Acounts.Count(x => x.Email == userName && x.PassWord == passWord);
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Acount GetAcount(string email)
        {
            Acount acount = context.Acounts.Where(x => x.Email == email).FirstOrDefault();
            return acount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaoStore.Code
{
    [Serializable] // tuan tu hoa ra nhi phan
    public class UserSession
    {
        public string UserName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TaoStore.Code
{
    public class CheckPay
    {
        public bool checkInfor(string name,string email,string phone,string adress)
        {
            if (name == "" || email ==""|| adress == "" || phone =="")
            {
                return false;
            }
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var val = regex.IsMatch(email);
            // !string.IsNullOrEmpty(email) && new EmailAddressAttribute().IsValid(email)
            if (val==false)
            {
                return false;
            }
            if (phone.Length <= 9)
            {
                return false;
            }
            return true;
        }
    }
}
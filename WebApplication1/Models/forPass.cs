using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class foPas
    {
        public int id { get; }
        public string email { get; set; }
        public string resetToken { get; set; }
        public string ExpiryDate { get; set; }
    }

}
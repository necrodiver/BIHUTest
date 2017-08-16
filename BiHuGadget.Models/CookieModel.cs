using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    public class CookieModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public TimeSpan ExpireTime { get; set; }
    }
}

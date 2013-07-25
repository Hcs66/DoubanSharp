using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanErrorMsessage : DoubanModelBase
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public string Request { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public abstract class DoubanSearchBase : IDoubanModel
    {
        public string Count { get; set; }
        public string Start { get; set; }
        public string Total { get; set; }
        public string RawSource { get; set; }
    }
}

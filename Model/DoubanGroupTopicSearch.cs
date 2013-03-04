using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanGroupTopicSearch : DoubanSearchBase
    {
        public List<DoubanGroupTopic> Topics { get; set; }
        public bool HasMore { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanGroupTopicReview : DoubanModelBase
    {
        public string Text { get; set; }
        public DoubanPeople Author { get; set; }
        public string Time { get; set; }
    }
}

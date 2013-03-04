using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanGroupTopicReviewSearch : DoubanSearchBase
    {
        public List<DoubanGroupTopicReview> Comments { get; set; }
    }
}

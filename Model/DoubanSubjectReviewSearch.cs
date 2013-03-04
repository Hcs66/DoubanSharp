using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanSubjectReviewSearch : DoubanSearchBase
    {
        public List<DoubanSubjectReview> ReviewList { get; set; }
        public string ResultTitle;
    }
}

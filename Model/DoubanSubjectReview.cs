using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanSubjectReview : DoubanModelBase
    {
        public DoubanRating Rating { get; set; }
        public string Votes { get; set; }
        public string Useless { get; set; }
        public string Comments { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DoubanAuthor Author { get; set; }
        public string Updated { get; set; }
        public string Published { get; set; }
        [IgnoreDataMember]
        public DoubanSubjectBase Subject { get; set; }
        public string Image { get; set; }
        public string SubjectId { get; set; }
        public string SubjectTitle{get;set;}
    }
}

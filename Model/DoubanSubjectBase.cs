using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DoubanSharp.Model
{
    [DataContract]
    public abstract class DoubanSubjectBase : DoubanModelBase
    {
        [DataMember]
        public string AltTitle { get; set; }
        [DataMember]
        public List<DoubanTag> Tags { get; set; }
        [DataMember]
        public string Summary { get; set; }
        [DataMember]
        public DoubanRating Rating { get; set; }
        [DataMember]
        public string Image { get; set; }
        [IgnoreDataMember]
        public Dictionary<string, List<string>> Attrs { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
    }
}

using System.Collections.Generic;
namespace DoubanSharp.Model
{
    public class DoubanMusic : DoubanSubjectBase
    {
        public string MobileLink { get; set; }
        public List<DoubanAuthor> Author { get; set; }
    }
}

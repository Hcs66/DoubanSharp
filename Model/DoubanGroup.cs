using System.Collections.Generic;
namespace DoubanSharp.Model
{
    public class DoubanGroup : DoubanModelBase
    {
        public string Domain { get; set; }
        public string UId { get; set; }
        public string MemberRole { get; set; }
        public DoubanPeople Owner { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
        public string MemberCount { get; set; }
        public string Created { get; set; }
        public string JoinType { get; set; }
        public List<DoubanPeople> Admins { get; set; }
        public string Avatar { get; set; }
    }

}

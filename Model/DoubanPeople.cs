using System.Runtime.Serialization;
namespace DoubanSharp.Model
{
    public class DoubanPeople : DoubanModelBase
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Relation { get; set; }
        public string Created { get; set; }
        public string LocId { get; set; }
        public string LocName { get; set; }
        public string Desc { get; set; }
    }
}

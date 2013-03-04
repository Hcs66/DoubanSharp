namespace DoubanSharp.Model
{
    public class DoubanGroupTopic : DoubanModelBase
    {
        public string Updated { get; set; }
        public DoubanGroup Group { get; set; }
        public DoubanPeople Author { get; set; }
        public string Created { get; set; }
        public string Content { get; set; }
        public string Photos { get; set; }
        public string CommentsCount { get; set; }
    }
}

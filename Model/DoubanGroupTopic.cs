using System.Collections.Generic;
namespace DoubanSharp.Model
{
    public class DoubanGroupTopic : DoubanModelBase
    {
        public string Updated { get; set; }
        public DoubanGroup Group { get; set; }
        public DoubanPeople Author { get; set; }
        public string Created { get; set; }
        public string Content { get; set; }
        public List<DoubanGroupTopicImage> Photos { get; set; }
        public string CommentsCount { get; set; }
    }

    public class DoubanGroupTopicImage : DoubanModelBase
    {
        public string Layout { get; set; }
        public string TopicId { get; set; }
        public string SeqId { get; set; }
        public string AuthorId { get; set; }
        public string CreationDate { get; set; }
    }
}

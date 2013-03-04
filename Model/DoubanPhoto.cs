namespace DoubanSharp.Model
{
    public class DoubanPhoto : DoubanModelBase
    {
        public string AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public string Icon { get; set; }
        public string Thumb { get; set; }
        public string Cover { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }
        public string Created { get; set; }
        public string Privacy { get; set; }
        public string Position { get; set; }
        public string PrevPhoto { get; set; }
        public string NextPhoto { get; set; }
        public string LikedCount { get; set; }
        public string RecsCount { get; set; }
        public string CommentsCount { get; set; }
        public string Author { get; set; }
        public string Liked { get; set; }
    }
}

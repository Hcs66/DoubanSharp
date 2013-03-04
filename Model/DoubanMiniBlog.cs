using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanSharp.Model
{
    public class DoubanMiniBlog : DoubanModelBase
    {
        public DoubanMiniBlogUser User { get; set; }
        public DoubanMiniBlogSource Source { get; set; }
        public string Text { get; set; }
        public string ResharedCount { get; set; }
        public string LikeCount { get; set; }
        public string CommentCount { get; set; }
        public string CanReply { get; set; }
        public string Liked { get; set; }
        public string CreatedAt { get; set; }
        public string ResharedStatus { get; set; }
        public string Category { get; set; }
        public List<DoubanMiniBlogAttachment> Attachments { get; set; }
    }

    public class DoubanMiniBlogUser
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string ScreenName { get; set; }
        public string SmallAvatar { get; set; }
        public string LargeAvatar { get; set; }
    }

    public class DoubanMiniBlogAttachment
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public string Caption { get; set; }
        public string Type { get; set; }
        public string Properties { get; set; }
    }

    public class DoubanMiniBlogSource
    {
        public string Href { get; set; }
        public string Title { get; set; }
    }
}

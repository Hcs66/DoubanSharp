using System.Collections.Generic;
namespace DoubanSharp.Model
{
    public class DoubanEvent : DoubanModelBase
    {
        public string Desc { get; set; }
        public List<DoubanTag> Tags { get; set; }
        public string Created { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string RelatedUrl { get; set; }
        public string ShuoTopic { get; set; }
        public string CascadeInvite { get; set; }
        public string GroupId { get; set; }
        public string AlbumId { get; set; }
        public string ParticipantCount { get; set; }
        public string PhotoCount { get; set; }
        public string LikedCount { get; set; }
        public string RecsCount { get; set; }
        public string Icon { get; set; }
        public string Thumb { get; set; }
        public string Cover { get; set; }
        public string Image { get; set; }
        public string Owner { get; set; }
        public string Liked { get; set; }
        public string Joined { get; set; }
    }
}

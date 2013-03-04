using System.Collections.Generic;
namespace DoubanSharp.Model
{
    public class DoubanBook : DoubanSubjectBase
    {
        public List<string> Author { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string OriginTitle { get; set; }
        public string Subtitle { get; set; }
        public string Url { get; set; }
        public DoubanBookImage Images { get; set; }
        public List<string> Translator { get; set; }
        public string Publisher { get; set; }
        public string Pubdate { get; set; }
        public string Binding { get; set; }
        public string Price { get; set; }
        public string Pages { get; set; }
        public string AuthorIntro { get; set; }
    }

    public class DoubanBookImage
    {
        public string Small { get; set; }
        public string Large { get; set; }
        public string Medium { get; set; }
    }
}

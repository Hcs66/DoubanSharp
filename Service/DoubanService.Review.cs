using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using DoubanSharp.Model;
using HtmlAgilityPack;
using SocialEbola.Lib.HapHelper;
using RestSharp;

namespace DoubanSharp.Service
{
    public partial class DoubanService
    {
        private static string m_SubjectReviewFeedUrl = "http://www.douban.com/feed/subject/{0}/reviews";
        private static string m_SubjectReviewUrl = "http://www.douban.com/review/{0}/";
        private static Regex m_ImgSrcRegex = new Regex("src=\"([^=]+)\"");
        private static Regex m_SubjectId = new Regex("/subject/([\\d]+)/");
        private static Regex m_HtmlTagRegex = new Regex(@"</?[a-z][a-z0-9]*[^<>]*>");
        private static Regex m_ReviewContentRegex = new Regex("<span property=\"v:description\">([\\s\\S]+?)</span>");
        private static Regex m_ReviewItemRegex = new Regex("<span property=\"v:itemreviewed\">([\\s\\S]+?)</span>");
        private static Regex m_ReviewTitleRegex = new Regex("<h1>([\\s\\S]+?)</h1>");
        private static Regex m_ReviewImageRegex = new Regex("<img class=\"pil\">([\\s\\S]+?)</img>");
        private static Regex m_ReviewerRegex = new Regex("<span property=\"v:reviewer\">([\\s\\S]+?)</span>");
        private static Regex m_ReviewDateRegex = new Regex("<span property=\"v:dtreviewed\">([\\s\\S]+?)</span>");
        private static Regex m_ReviewRatingRegex = new Regex("<span property=\"v:rating\">([\\s\\S]+?)</span>");

        public void GetReview(string reviewId, Action<DoubanSubjectReview, DoubanResponse> action)
        {
            var feedsRequest = (HttpWebRequest)WebRequest.Create(string.Format(m_SubjectReviewUrl, reviewId));
            feedsRequest.BeginGetResponse(r =>
            {
                DoubanResponse doubanResp = new DoubanResponse();
                try
                {
                    var httpRequest = (HttpWebRequest)r.AsyncState;
                    var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                    using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        DoubanSubjectReview review = new DoubanSubjectReview();
                        string content = reader.ReadToEnd();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(content);
                        HtmlNode root = doc.DocumentNode;
                        List<HtmlNode> spanNodes = root.Descendants("span").Where(n => n.HasAttributes && n.Attributes.Contains("property")).ToList();
                        //标题
                        review.Title = m_ReviewTitleRegex.Match(content).Groups[1].Value;
                        review.Title = m_HtmlTagRegex.Replace(review.Title, "").Replace("\n", "").Replace(" ", "");
                        //评论时间
                        review.Updated = m_ReviewDateRegex.Match(content).Groups[1].Value;
                        //评分
                        review.Rating = new DoubanRating() { Average = spanNodes.Single(i => i.Attributes["property"].Value == "v:rating").InnerText };
                        //项目名称
                        review.SubjectTitle = m_ReviewItemRegex.Match(content).Groups[1].Value.Replace("\n", "").Replace(" ", "");
                        //作者
                        review.Author = new DoubanAuthor()
                        {
                            Name = m_ReviewerRegex.Match(content).Groups[1].Value
                        };
                        IEnumerable<HtmlNode> imgs = doc.DocumentNode.Descendants("img").Where(n => n.HasAttributes &&
                            n.Attributes.Contains("class") && n.Attributes["class"].Value == "pil");
                        if (imgs.Count() > 0)
                        {
                            review.Author.Icon = imgs.ToList()[0].Attributes["src"].Value;
                        }
                        else//处理电影评论
                        {
                            HtmlNode imgNode = doc.DocumentNode.DescendantNodes().SingleOrDefault(n => n.HasAttributes &&
                                                          n.Attributes.Contains("class") && n.Attributes["class"].Value == "main-avatar");
                            if (imgNode != null && imgNode.HasChildNodes)
                            {
                                review.Author.Icon = imgNode.Element("img").Attributes["src"].Value;
                            }
                        }
                        //评论内容,去除html标记
                        string reviewContent = "";
                        HtmlNode desc = doc.GetElementbyId("link-report");
                        if (desc != null)
                        {
                            reviewContent = desc.InnerText;
                        }
                        review.Content = m_HtmlTagRegex.Replace(reviewContent, "");    
                        doubanResp.RestResponse = new RestResponse() { StatusCode = httpResponse.StatusCode };
                        action(review, doubanResp);
                    }
                }
                catch (Exception ex)
                {
                    doubanResp.RestResponse.ErrorException = ex;
                    doubanResp.RestResponse.StatusCode = HttpStatusCode.BadRequest;
                    action(new DoubanSubjectReview(), doubanResp);
                }
            }, feedsRequest);

        }

        public void SearchSubjectReviews(string subjectId, string start, string count,
            Action<DoubanSubjectReviewSearch, DoubanResponse> action)
        {
            var feedsRequest = (HttpWebRequest)WebRequest.Create(string.Format(m_SubjectReviewFeedUrl, subjectId));
            feedsRequest.BeginGetResponse(r =>
            {
                var httpRequest = (HttpWebRequest)r.AsyncState;
                var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    XmlReader xmlReader = XmlReader.Create(reader);
                    SyndicationFeed feeds = SyndicationFeed.Load(xmlReader);
                    var feedItems = from feed in feeds.Items
                                    select new DoubanSubjectReview()
                                    {
                                        Id = feed.Id.Substring(feed.Id.TrimEnd('/').LastIndexOf("/") + 1).TrimEnd('/'),
                                        Title = feed.Title.Text,
                                        Summary = feed.Summary.Text,
                                        Updated = feed.PublishDate.LocalDateTime.ToShortDateString(),
                                        RawSource = feed.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")[0],
                                        Author = new DoubanAuthor() { Name = feed.ElementExtensions.ReadElementExtensions<string>("creator", "http://purl.org/dc/elements/1.1/")[0] }
                                    };
                    //匹配图片和项目Id
                    List<DoubanSubjectReview> reviewList = feedItems.ToList<DoubanSubjectReview>();
                    foreach (var review in reviewList)
                    {
                        if (m_ImgSrcRegex.IsMatch(review.RawSource))
                        {
                            review.Image = m_ImgSrcRegex.Match(review.RawSource).Groups[1].Value;
                        }
                    }
                    DoubanSubjectReviewSearch result = new DoubanSubjectReviewSearch()
                    {
                        ReviewList = reviewList,
                        ResultTitle = feeds.Title.Text,
                        Total = feeds.Items.Count().ToString()
                    };
                    action(result, new DoubanResponse() { RestResponse = new RestResponse() { StatusCode = httpResponse.StatusCode } });
                }

            }, feedsRequest);
        }
    }
}

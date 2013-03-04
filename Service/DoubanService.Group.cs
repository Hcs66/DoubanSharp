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
using RestSharp.Deserializers;

namespace DoubanSharp.Service
{
    public partial class DoubanService
    {
        private static string m_MyGroupTopicUrl = "http://www.douban.com/group/?start={0}";

        public void SearchMyGroupTopics(Action<DoubanGroupTopicSearch, DoubanResponse> action, string start, string count)
        {
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/user_topics?start={0}&count={1}", start, count), action);
        }

        public void SearchMyCreateGroupTopics(Action<DoubanGroupTopicSearch, DoubanResponse> action,
            string start, string count)
        {
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/my_topics?start={0}&count={1}", start, count), action);
        }

        public void SearchMyReplyGroupTopics(Action<DoubanGroupTopicSearch, DoubanResponse> action,
            string start, string count)
        {
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/my_replied_topics?start={0}&count={1}", start, count), action);
        }

        public void GetGroup(string uid, Action<DoubanGroup, DoubanResponse> action)
        {
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/{0}", uid), action);
        }

        public void GetGroupTopic(string tid, Action<DoubanGroupTopic, DoubanResponse> action)
        {
            //ExecuteRequest(action, "/v2/group/topic/{tid}", "?tid=", tid);
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/topic/{0}", tid), action);
        }

        public void SearchGroupTopics(string uid, string start, string count, Action<DoubanGroupTopicSearch, DoubanResponse> action)
        {
            //ExecuteRequest(action, string.Format("/v2/group/{0}/topics", uid), "?start=", start, "&count=", count);
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/{0}/topics?start={1}&count={2}", uid, start, count), action);
        }

        public void SearchGroupTopicReviews(string tid, string start, string count, Action<DoubanGroupTopicReviewSearch, DoubanResponse> action)
        {
            ExecuteRequestWithWebRequest(string.Format(Globals.API_HOST + "v2/group/topic/{0}/comments?start={1}&count={2}", tid, start, count), action);
        }
    }
}

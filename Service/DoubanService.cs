using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Linq;
using DoubanSharp.Model;
using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;

//测试git
namespace DoubanSharp.Service
{
    /// <summary>
    /// Douban接口服务
    /// </summary>
    public partial class DoubanService
    {
        private string _consumerKey;
        private string _consumerSecret;
        private string _redirectUrl;
        private OAuthAccessToken _accessToken = new OAuthAccessToken();
        private bool _hasAuthenticated;
        private RestClient _apiClient;
        private RestClient _oauthClient;
        private JsonDeserializer _jsonDeserializer;
        private string _doubanCookie = "";

        public DoubanService()
        {
            _apiClient = new RestClient(Globals.API_HOST);
            _oauthClient = new RestClient(Globals.AUTH_HOST);
            _jsonDeserializer = new JsonDeserializer();
        }

        public DoubanService(string consumerKey, string consumerSecret, string redirectUrl)
            : this()
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _redirectUrl = redirectUrl;
        }

        #region 属性

        public string DoubanUserId
        {
            get
            {
                return _accessToken.DoubanUserId;
            }
        }

        public bool HasAuthenticated
        {
            get
            {
                return _hasAuthenticated;
            }
        }

        public string DoubanCookie
        {
            get
            {
                return _doubanCookie;
            }
            set
            {
                _doubanCookie = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <returns></returns>
        public Uri GetAuthorizationUri()
        {
            return new Uri(Globals.AUTH_HOST + string.Format("service/auth2/auth?client_id={0}&redirect_uri={1}&response_type=code", this._consumerKey, _redirectUrl));
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="requestToken"></param>
        /// <param name="action"></param>
        public void Authenticate(string authorizationCode, Action<OAuthAccessToken, DoubanResponse> action)
        {
            var request = new RestRequest("service/auth2/token", Method.POST);
            request.AddParameter("client_id", _consumerKey);
            request.AddParameter("client_secret", _consumerSecret);
            request.AddParameter("redirect_uri", _redirectUrl);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            _oauthClient.ExecuteAsync(request,
                (respone) =>
                {
                    OAuthAccessToken token = _jsonDeserializer.Deserialize<OAuthAccessToken>(respone);
                    if (!string.IsNullOrEmpty(token.AccessToken))
                    {
                        respone.StatusCode = HttpStatusCode.OK;
                        _hasAuthenticated = true;
                        _accessToken = token;
                    }
                    action(token, new DoubanResponse() { RestResponse = respone });
                }
            );
        }

        public void AuthenticateWith(OAuthAccessToken accessToken)
        {
            _accessToken = accessToken;
            _hasAuthenticated = true;
        }

        /// <summary>
        /// 重置token
        /// </summary>
        public void ResetAuthenticate()
        {
            _accessToken = new OAuthAccessToken();
            _hasAuthenticated = false;
            _doubanCookie = "";
        }

        private void ExecuteRequest(Method method, Action<DoubanResponse> action, string path, DoubanModelBase entry = null)
        {
            var request = PrepareRequest(path, method);
            if (entry != null)
            {
                request.AddObject(entry);
            }
            ExecuteRequestImpl(request, action);
        }

        private RestRequest PrepareRequest(string path, Method method = Method.GET)
        {
            RestRequest request = new RestRequest(path, method);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("appkey", _consumerKey);
            return request;
        }

        private void ExecuteRequest(Method method, Action<DoubanResponse> action, string path, params string[] segments)
        {
            ExecuteRequest(method, action, ResolveUrlSegments(path, segments.ToList()));
        }

        private void ExecuteRequest(Method method, Action<DoubanResponse> action, string path, DoubanModelBase entry, params string[] segments)
        {
            ExecuteRequest(method, action, ResolveUrlSegments(path, segments.ToList()), entry);
        }

        private void ExecuteRequestImpl(RestRequest request, Action<DoubanResponse> action)
        {
            if (!string.IsNullOrEmpty(_accessToken.AccessToken))
            {
                _apiClient.Authenticator = new DoubanOAuth2AuthorizationRequestHeaderAuthenticator(_accessToken.AccessToken);
            }
            _apiClient.ExecuteAsync(request,
                (resp) =>
                {
                    ////accesstoken过期处理
                    //if (resp.StatusCode == HttpStatusCode.BadRequest)
                    //{
                    //    if (!string.IsNullOrEmpty(resp.Content))
                    //    {
                    //        try
                    //        {
                    //            DoubanErrorMsessage msg = _jsonDeserializer.Deserialize<DoubanErrorMsessage>(resp);
                    //            msg.RawSource = resp.Content;
                    //            if (msg.Code == "106")
                    //            {
                    //                //通过refreshtoken获取新的accesstoken
                    //                RefreshToken(action);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            resp.StatusCode = HttpStatusCode.BadRequest;
                    //            resp.ErrorException = ex;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    action(new DoubanResponse() { RestResponse = resp });
                    //}
                });
        }

        private void RefreshToken(Action<DoubanResponse> action)
        {
            var request = new RestRequest("service/auth2/token", Method.POST);
            request.AddParameter("client_id", _consumerKey);
            request.AddParameter("client_secret", _consumerSecret);
            request.AddParameter("redirect_uri", _redirectUrl);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", _accessToken.RefreshToken);
            _oauthClient.ExecuteAsync(request,
                (respone) =>
                {
                    OAuthAccessToken token = _jsonDeserializer.Deserialize<OAuthAccessToken>(respone);
                    if (!string.IsNullOrEmpty(token.AccessToken))
                    {
                        respone.StatusCode = HttpStatusCode.OK;
                        _hasAuthenticated = true;
                        _accessToken = token;
                    }
                    action(new DoubanResponse() { RestResponse = respone });
                }
            );
        }

        private void ExecuteRequest<T>(Action<T, DoubanResponse> action, string path) where T : IDoubanModel
        {
            ExecuteRequestImpl(PrepareRequest(path), action);
        }

        private void ExecuteRequest<T>(Action<T, DoubanResponse> action, string path, params string[] segments) where T : IDoubanModel
        {
            ExecuteRequest(action, ResolveUrlSegments(path, segments.ToList()));
        }

        private void ExecuteRequest<T>(Method method, Action<T, DoubanResponse> action, string path) where T : IDoubanModel
        {
            ExecuteRequestImpl(PrepareRequest(path, method), action);
        }

        private void ExecuteRequest<T>(Method method, Action<T, DoubanResponse> action, string path, params string[] segments) where T : IDoubanModel
        {
            ExecuteRequest(method, action, ResolveUrlSegments(path, segments.ToList()));
        }

        private void ExecuteRequestImpl<T>(RestRequest request, Action<T, DoubanResponse> action) where T : IDoubanModel
        {
            if (!string.IsNullOrEmpty(_accessToken.AccessToken))
            {
                _apiClient.Authenticator = new DoubanOAuth2AuthorizationRequestHeaderAuthenticator(_accessToken.AccessToken);
            }
            _apiClient.ExecuteAsync(request,
                    (resp) =>
                    {
                        T entity = default(T);
                        if (resp.StatusCode == HttpStatusCode.OK
                            || resp.StatusCode == HttpStatusCode.Created
                            || resp.StatusCode == HttpStatusCode.Accepted)
                        {
                            if (!string.IsNullOrEmpty(resp.Content))
                            {
                                try
                                {
                                    resp.Content = resp.Content.StartsWith("[") ? "{\"entitylist\":" + resp.Content + "}" : resp.Content;
                                    entity = _jsonDeserializer.Deserialize<T>(resp);
                                    entity.RawSource = resp.Content;
                                }
                                catch (Exception ex)
                                {
                                    resp.StatusCode = HttpStatusCode.BadRequest;
                                    resp.ErrorException = ex;
                                }
                            }
                        }
                        action(entity, new DoubanResponse() { RestResponse = resp });
                    });
        }

        private string ResolveUrlSegments(string path, List<string> segments)
        {
            if (segments == null) throw new ArgumentNullException("segments");

            path = PathHelpers.ReplaceUriTemplateTokens(segments, path);

            PathHelpers.EscapeDataContainingUrlSegments(segments);

            segments.Insert(0, path);

            return string.Concat(segments.ToArray()).ToString(CultureInfo.InvariantCulture);
        }

        private void ExecuteRequestWithWebRequest<T>(string url, Action<T, DoubanResponse> action)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            CookieContainer cc = new CookieContainer();
            CookieCollection ccl = new CookieCollection();
            //用户登录cookie
            ccl.Add(new Cookie() { Name = "dbcl2", Value = this._doubanCookie, Domain = "douban.com", Path = "/" });
            cc.Add(new Uri("http://www.douban.com/"), ccl);
            httpWebRequest.CookieContainer = cc;
            httpWebRequest.BeginGetResponse
                (r =>
                {
                    DoubanResponse doubanResp = new DoubanResponse();
                    try
                    {
                        var httpRequest = (HttpWebRequest)r.AsyncState;
                        T entity = default(T);
                        var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                        using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RestResponse restResponse = new RestResponse();
                            restResponse.Content = reader.ReadToEnd();
                            restResponse.StatusCode = httpResponse.StatusCode;
                            doubanResp.RestResponse = restResponse;

                            JsonDeserializer json = new JsonDeserializer();
                            entity = json.Deserialize<T>(restResponse);

                            action(entity, doubanResp);
                        }
                    }
                    catch (Exception ex)
                    {
                        doubanResp.RestResponse.ErrorException = ex;
                        doubanResp.RestResponse.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
                , httpWebRequest);
        }
        #endregion
    }
}

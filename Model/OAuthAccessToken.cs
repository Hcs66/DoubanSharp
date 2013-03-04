
namespace DoubanSharp.Model
{
    public class OAuthAccessToken
    {
        public string   AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string DoubanUserId { get; set; }
    }
}

namespace DoubanSharp.Model
{
    public class DoubanMail : DoubanModelBase
    {
        public string Status { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Published { get; set; }
        public string Content { get; set; }
    }
}

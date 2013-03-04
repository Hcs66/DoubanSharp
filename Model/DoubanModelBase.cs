using System;
using System.ComponentModel;
using System.Runtime.Serialization;
namespace DoubanSharp.Model
{
    [DataContract]
    public abstract class DoubanModelBase : IDoubanModel
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Alt { get; set; }
        [IgnoreDataMember]
        public string DoubanObjectId
        {
            get
            {
                if (!string.IsNullOrEmpty(Id) && !char.IsNumber(Id, 0))
                {
                    return Id.Substring(Id.LastIndexOf("/") + 1);
                }
                return this.Id;
            }
        }
        [IgnoreDataMember]
        public string RawSource { get; set; }
    }
}

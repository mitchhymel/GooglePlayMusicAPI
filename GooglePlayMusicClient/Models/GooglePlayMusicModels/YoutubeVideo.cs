using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class YoutubeVideo
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "thumbnails")]
        public List<Thumbnail> Thumbnails { get; set; }
    }
}

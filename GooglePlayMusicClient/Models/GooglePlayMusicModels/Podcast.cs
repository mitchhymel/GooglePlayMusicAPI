using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Podcast
    {
        [DataMember(Name = "seriesId")]
        public string SeriesId { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "author")]
        public string Author { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "art")]
        public List<Image> Art { get; set; }
        [DataMember(Name = "copyright")]
        public string Copyright { get; set; }
        [DataMember(Name = "explicitType")]
        public ExplicitType ExplicitType{ get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "continuationToken")]
        public string ContinuationToken { get; set; }
        [DataMember(Name = "totalNumEpisodes")]
        public int TotalNumEpisodes { get; set; }
    }
}

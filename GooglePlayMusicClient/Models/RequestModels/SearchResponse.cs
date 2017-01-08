using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI
{
    [DataContract]
    public class SearchResponse
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "clusterOrder")]
        public List<string> ClusterOrder { get; set; }
        [DataMember(Name = "suggestedQuery")]
        public string SuggestedQuery { get; set; }
        [DataMember(Name = "entries")]
        public List<SearchEntry> Entries { get; set; }
    }

    [DataContract]
    public class SearchEntry
    {
        [DataMember(Name = "score")]
        public float Score { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "best_result")]
        public bool BestResult { get; set; }
        [DataMember(Name = "navigational_result")]
        public bool NavigationalResult { get; set; }
        [DataMember(Name = "navigational_confidence")]
        public int NavigationalConfidence { get; set; }
        [DataMember(Name = "artist")]
        public Artist Artist { get; set; }
        [DataMember(Name = "album")]
        public Album Album { get; set; }
        [DataMember(Name = "track")]
        public Track Track { get; set; }
        [DataMember(Name = "playlist")]
        public Playlist Playlist { get; set; }
        [DataMember(Name = "series")]
        public Series Series { get; set; }
        [DataMember(Name = "station")]
        public Station Station { get; set; }
        [DataMember(Name = "situation")]
        public Situation Situation { get; set; }
        [DataMember(Name = "youtube_video")]
        public YoutubeVideo YoutubeVideo { get; set; }
    }
}

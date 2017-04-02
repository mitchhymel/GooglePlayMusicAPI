using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class SearchEntry
    {
        [DataMember(Name = "score")]
        public float Score { get; set; }
        [DataMember(Name = "type")]
        public SearchEntryType Type { get; set; }
        [DataMember(Name = "best_result")]
        public bool BestResult { get; set; }
        [DataMember(Name = "navigational_result")]
        public bool NavigationalResult { get; set; }
        [DataMember(Name = "navigational_confidence")]
        public float NavigationalConfidence { get; set; }
        [DataMember(Name = "artist")]
        public Artist Artist { get; set; }
        [DataMember(Name = "album")]
        public Album Album { get; set; }
        [DataMember(Name = "track")]
        public Track Track { get; set; }
        [DataMember(Name = "playlist")]
        public Playlist Playlist { get; set; }
        [DataMember(Name = "series")]
        public Podcast Series { get; set; }
        [DataMember(Name = "station")]
        public Station Station { get; set; }
        [DataMember(Name = "situation")]
        public Situation Situation { get; set; }
        [DataMember(Name = "youtube_video")]
        public YoutubeVideo YoutubeVideo { get; set; }
    }

    [DataContract]
    public enum SearchEntryType
    {
        Track = 1,
        Artist = 2,
        Album = 3,
        Playlist = 4,
        Series = 5,
        Station = 6,
        Situation = 7,
        YoutubeVideo = 8,
        Podcast = 9
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using GooglePlayMusicAPI.Models.GooglePlayMusicModels;

namespace GooglePlayMusicAPI.Models.RequestModels
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
}


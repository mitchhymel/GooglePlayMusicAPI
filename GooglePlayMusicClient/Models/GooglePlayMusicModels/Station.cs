using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Station
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "seed")]
        public RadioSeed Seed { get; set; }
        [DataMember(Name = "stationSeeds")]
        public List<RadioSeed> StationSeeds { get; set; }
        [DataMember(Name = "imageUrls")]
        public List<Image> ImageUrls { get; set; }
        [DataMember(Name = "compositeArtRefs")]
        public List<Image> CompositeArtRefs { get; set; }
        [DataMember(Name = "contentTypes")]
        public List<SearchEntryType> ContentTypes { get; set; }
        [DataMember(Name = "byline")]
        public string ByLine { get; set; }
        //[DataMember(Name = "skipEventHistory")]
        //public List<object> SkipEventHistory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class RadioSeed
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "curatedStationId")]
        public string CuratedStationId { get; set; }
        [DataMember(Name = "seedType")]
        public int SeedType { get; set; }
    }
}

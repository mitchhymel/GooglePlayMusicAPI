using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Situation
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }
        [DataMember(Name = "wideImageUrl")]
        public string WideImageUrl { get; set; }
        [DataMember(Name = "stations")]
        public List<Station> Stations { get; set; }
    }
}

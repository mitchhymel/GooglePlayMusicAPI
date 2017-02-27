using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Album
    {
        [DataMember(Name = "albumArtist")]
        public string AlbumArtist { get; set; }
        [DataMember(Name = "albumArtRef")]
        public string AlbumArtRef { get; set; }
        [DataMember(Name = "albumId")]
        public string AlbumId { get; set; }
        [DataMember(Name = "artist")]
        public string Artist { get; set; }
        [DataMember(Name = "artistId")]
        public List<string> ArtistId { get; set; }
        [DataMember(Name = "tracks")]
        public List<Track> Tracks { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "description_attribution")]
        public Attribution DescriptionAttribution { get; set; }
        [DataMember(Name = "explicitType")]
        public ExplicitType ExplicitType { get; set; }
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "year")]
        public string Year { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI
{
    [DataContract]
    public class Artist
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "artistArtRef")]
        public string ArtistArtRef { get; set; }
        [DataMember(Name = "artistArtRefs")]
        public List<Image> ArtistArtRefs { get; set; }
        [DataMember(Name = "artistBio")]
        public string ArtistBio { get; set; }
        [DataMember(Name = "artistId")]
        public string ArtistId { get; set; }
        [DataMember(Name = "albums")]
        public List<Album> Albums { get; set; }
        [DataMember(Name = "topTracks")]
        public List<Track> TopTracks { get; set; }
        [DataMember(Name = "total_albums")]
        public int TotalAlbums { get; set; }
        [DataMember(Name = "artist_bio_attribution")]
        public Attribution ArtistBioAttribution { get; set; }
    }
}

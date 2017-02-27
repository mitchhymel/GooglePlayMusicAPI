using GooglePlayMusicAPI.Models.GooglePlayMusicModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI.Models.RequestModels
{
    public class SearchResult
    {
        public List<Track> Tracks;
        public List<Artist> Artists;
        public List<Album> Albums;
        public List<Playlist> Playlists;
        public List<Podcast> Podcasts;
        public List<Station> Stations;
        public List<Situation> Situations;
        public List<YoutubeVideo> YoutubeVideos;

        public SearchResult(SearchResponse response)
        {
            Tracks = new List<Track>();
            Artists = new List<Artist>();
            Albums = new List<Album>();
            Playlists = new List<Playlist>();
            Podcasts = new List<Podcast>();
            Stations = new List<Station>();
            Situations = new List<Situation>();
            YoutubeVideos = new List<YoutubeVideo>();

            if (response.Entries != null)
            {
                Tracks.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Track).Select(e => e.Track));
                Artists.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Artist).Select(e => e.Artist));
                Albums.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Album).Select(e => e.Album));
                Playlists.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Playlist).Select(e => e.Playlist));
                Podcasts.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Podcast).Select(e => e.Series));
                Stations.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Station).Select(e => e.Station));
                Situations.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.Situation).Select(e => e.Situation));
                YoutubeVideos.AddRange(response.Entries.Where(e => e.Type == SearchEntryType.YoutubeVideo).Select(e => e.YoutubeVideo));
            }
            
        }

    }
}

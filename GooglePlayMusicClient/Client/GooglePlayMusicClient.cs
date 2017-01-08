using Mono.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI
{
    public class GooglePlayMusicClient
    {
        private IRequestClient requestClient;

        // old client id 565126123933-f27ojtmm7veeb51f8floos7s9vk80i5k
        // Client id from gmusicapi
        private static string ClientId = "565126123933-f27ojtmm7veeb51f8floos7s9vk80i5k";

        private static string SJ_URL_BASE = "https://mclients.googleapis.com/sj/v2.5/";
        private static string SJ_URL_TRACKS = SJ_URL_BASE + "trackfeed";
        private static string SJ_URL_PLAYLISTS_FEED = SJ_URL_BASE + "playlistfeed";
        private static string SJ_URL_PLAYLISTS_BATCH = SJ_URL_BASE + "playlistbatch";
        private static string SJ_URL_PLAYLISTS_ENTRY_FEED = SJ_URL_BASE + "plentryfeed";
        private static string SJ_URL_PLAYLIST_ENTRIES_BATCH = SJ_URL_BASE + "plentriesbatch";
        private static string SJ_URL_SEARCH = SJ_URL_BASE + "query";
        private static string SJ_URL_TRACK = SJ_URL_BASE + "fetchtrack";
        private static string SJ_URL_ALBUM = SJ_URL_BASE + "fetchalbum";

        private static string SJ_URL_STREAM = "https://mclients.googleapis.com/music/";
        private static string SJ_URL_STREAM_TRACK = SJ_URL_STREAM + "mplay";
        private static string SJ_URL_DEVICE_MANAGEMENT = SJ_URL_BASE + "devicemanagementinfo";

        public enum ShareState { PUBLIC, PRIVATE}
        public enum SearchEntryType
        {
            SONG = 1,
            ARTIST = 2,
            ALBUM = 4,
            PLAYLIST = 8,
            STATION = 16,
            SITUATION = 32,
            VIDEO = 64,
            PODCAST = 128
        }

        public GooglePlayMusicClient()
        {
            requestClient = new RequestClient(ClientId);
        }

        public GooglePlayMusicClient(IRequestClient inRequestClient)
        {
            requestClient = inRequestClient;
        }

        #region Account functions

        public Boolean LoggedIn()
        {
            return requestClient.IsLoggedIn();
        }

        /// <summary>
        /// Log in to Google Play Music using OAuth
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>boolean indicating success or failure</returns>
        public async Task<bool> LoginAsync(string email, string password)
        {
            return await requestClient.LoginAsync(email, password);
        }

        #endregion

        #region Library and Track functions

        /// <summary>
        /// Gets all tracks in the library
        /// </summary>
        /// <param name="tracksToGet">Number of tracks to fetch</param>
        /// <returns>List of all the songs in the library</returns>
        public async Task<List<Track>> GetLibraryAsync(int tracksToGet = 0)
        {
            return await requestClient.PerformIncrementalPostAsync<Track>(SJ_URL_TRACKS, tracksToGet);
        }

        /// <summary>
        /// Gets track information
        /// </summary>
        /// <param name="trackId">Id of the track</param>
        /// <returns>Track information</returns>
        public async Task<Track> GetTrackAsync(string trackId)
        {
            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["nid"] = trackId;

            return await requestClient.PerformGetAsync<Track>(SJ_URL_TRACK, additionalParams);
        }

        /// <summary>
        /// Gets album information
        /// </summary>
        /// <param name="albumId">Id of the album</param>
        /// <param name="includeTracks">boolean indicating if all track information should be fetched as well</param>
        /// <returns>Album information</returns>
        public async Task<Album> GetAlbumAsync(string albumId, bool includeTracks = true)
        {
            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["nid"] = albumId;
            additionalParams["include-tracks"] = includeTracks.ToString();

            return await requestClient.PerformGetAsync<Album>(SJ_URL_ALBUM, additionalParams);
        }

        /// <summary>
        /// Searches for the provided query
        /// </summary>
        /// <param name="searchQuery">Query to search for</param>
        /// <param name="types">Bitwise combination of SearchEntryTypes to search for</param>
        /// <param name="maxResults">Max results</param>
        /// <returns></returns>
        public async Task<SearchResponse> SearchAsync(string searchQuery, SearchEntryType types = 0, int maxResults = 50)
        {
            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["q"] = searchQuery;
            additionalParams["ct"] = GetSearchEntryTypeFromValue(types);
            additionalParams["max-results"] = maxResults.ToString();

            return await requestClient.PerformGetAsync<SearchResponse>(SJ_URL_SEARCH, additionalParams);
        }

        /// <summary>
        /// Get a list of devices associated with the account
        /// </summary>
        /// <returns></returns>
        public async Task<List<Device>> GetDevicesAsync()
        {
            IncrementalResponse<Device> response = await requestClient.PerformGetAsync<IncrementalResponse<Device>>(SJ_URL_DEVICE_MANAGEMENT);
            return response.Data.Items;
        }

        public async Task<string> GetStreamUrlAsync(string deviceId, string trackId, string quality = "hi")
        {
            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["device_id"] = deviceId;
            additionalParams["mjck"] = trackId;
            additionalParams["opt"] = quality;
            additionalParams["slt"] = ""; // not sure what this is
            additionalParams["sig"] = ""; // not sure waht this is
            additionalParams["net"] = "mob"; // mobile?

            return await requestClient.PerformGetAsync<string>(SJ_URL_STREAM_TRACK, additionalParams);
        }

        #endregion

        #region Playlist functions

        /// <summary>
        /// Gets all playlists
        /// </summary>
        /// <param name="playlistsToGet">Number of playlists to get, default is all of them</param>
        /// <returns>List of all playlists in the library</returns>
        public async Task<List<Playlist>> GetPlaylistsAsync(int playlistsToGet = 0)
        {
            return await requestClient.PerformIncrementalPostAsync<Playlist>(SJ_URL_PLAYLISTS_FEED, playlistsToGet);
        }

        /// <summary>
        /// Gets all playlist entries
        /// </summary>
        /// <param name="entriesToGet">Numer of playlist entries to get, default is all of them</param>
        /// <returns></returns>
        public async Task<List<PlaylistEntry>> GetPlaylistEntriesAsync(int entriesToGet = 0)
        {
            return await requestClient.PerformIncrementalPostAsync<PlaylistEntry>(SJ_URL_PLAYLISTS_ENTRY_FEED, entriesToGet);
        }
      
        /// <summary>
        /// Gets all playlists and entries and matches the playlists.songs with the entries
        /// </summary>
        /// <returns></returns>
        public async Task<List<Playlist>> GetPlaylistsWithEntriesAsync()
        {
            List<Playlist> playlists = await GetPlaylistsAsync();
            List<PlaylistEntry> entries = await GetPlaylistEntriesAsync();

            foreach (Playlist playlist in playlists)
            {
                playlist.Songs = entries.Where(s => s.PlaylistID == playlist.ID).ToList();
                playlist.Songs.OrderBy(s => s.AbsolutePosition);
            }
            return playlists;
        }

        /// <summary>
        /// Creates a playlist
        /// </summary>
        /// <param name="name">Name of the playlist to be created</param>
        /// <param name="description">Description of the playlist to be created</param>
        /// <param name="shareState">If the playlist will be PUBLIC or PRIVATE</param>
        /// <returns></returns>
        public async Task<MutatePlaylistResponse> CreatePlaylistAsync(string name, string description = null, ShareState shareState = ShareState.PRIVATE)
        {
            JObject requestData = new JObject()
            { { "mutations" , new JArray()
                { new JObject()
                    { { "create" , new JObject()
                        {
                            {"name", name},
                            {"deleted", false},
                            {"creationTimestamp", "-1"},
                            {"lastModifiedTimestamp","0"},
                            {"type", "USER_GENERATED"},
                            {"shareState", shareState == ShareState.PRIVATE ? "PRIVATE" : "PUBLIC" },
                            {"description", description}
                        }
                    } }
                }
            } };

            return await requestClient.PerformPostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
        }

        /// <summary>
        /// Deletes a playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist</param>
        /// <returns></returns>
        public async Task<MutateResponse> DeletePlaylistAsync(string playlistId)
        {
            JObject requestData = new JObject()
            { { "mutations" , new JArray()
                {
                    new JObject() { { "delete", playlistId} }
                }
            } };

            return await requestClient.PerformPostAsync<MutateResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
        }

        /// <summary>
        /// Renames a playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist to be modified</param>
        /// <param name="name">New name for the playlist (or null to make no changes)</param>
        /// <param name="description">New description for the playlist (or null to make no changes)</param>
        /// <param name="shareState">New share state (PUBLIC or PRIVATE) (or null to make no changes)</param>
        /// <returns></returns>
        public async Task<MutateResponse> UpdatePlaylistAsync(string playlistId, string name, string description = null, ShareState? shareState = null)
        {
            JObject requestData = new JObject()
            { { "mutations" , new JArray()
                { new JObject()
                    { { "update" , new JObject()
                        {
                            {"name", name},
                            {"id", playlistId},
                            {"description", description},
                            {"sharestate", shareState == null ? null : shareState.ToString() }
                        }
                    } }
                }
            } };

            return await requestClient.PerformPostAsync<MutateResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
        }

        /// <summary>
        /// Adds the provided songs to the playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist to add songs to</param>
        /// <param name="songs">List of songs to add</param>
        /// <returns></returns>
        public async Task<MutatePlaylistResponse> AddToPlaylistAsync(string playlistId, List<Track> songs)
        {
            Guid prev_uid = Guid.NewGuid();
            Guid current_uid = Guid.NewGuid();
            Guid next_uid = Guid.NewGuid();

            // This function is taken more or less completely from def build_plentry_adds() in
            // the unofficial google music API
            JArray songsToAdd = new JArray();

            int i = 0;
            foreach (Track song in songs)
            {
                JObject songJObject = new JObject
                    {
                    { "clientId", current_uid.ToString() },
                    { "creationTimestamp", -1 },
                    { "deleted", false },
                    { "lastModifiedTimestamp", 0},
                    { "playlistId", playlistId },
                    { "source", 1 },
                    {"trackId", song.ID }
                    };

                if (song.ID.First() == 'T')
                    songJObject["source"] = 2;

                if (i > 0)
                    songJObject["precedingEntryId"] = prev_uid;

                if (i < songs.Count - 1)
                    songJObject["followingEntryId"] = next_uid;

                JObject createJObject = new JObject { { "create", songJObject } };

                songsToAdd.Add(createJObject);
                prev_uid = current_uid;
                current_uid = next_uid;
                next_uid = Guid.NewGuid();
                i++;
            }

            JObject requestData = new JObject
            {{
                 "mutations", songsToAdd
             }};

            return await requestClient.PerformPostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLIST_ENTRIES_BATCH, requestData);
        }

        /// <summary>
        /// Removes the list of playlist entries from their playlist(s)
        /// </summary>
        /// <param name="songs">List of entries to remove</param>
        /// <returns></returns>
        public async Task<MutatePlaylistResponse> RemoveFromPlaylistAsync(List<PlaylistEntry> songs)
        {
            JArray songsToDelete = new JArray();
            foreach (PlaylistEntry entry in songs)
            {
                songsToDelete.Add(new JObject
                {
                {"delete", entry.ID}
                });
            }

            JObject requestData = new JObject
            {{
                 "mutations", songsToDelete
             }};

            return await requestClient.PerformPostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLIST_ENTRIES_BATCH, requestData);
        }


        #endregion

        #region Helper Functions

        private string GetSearchEntryTypeFromValue(SearchEntryType val)
        {
            string result = "1%2C2%2C3%2C4%2C5%2C6%2C7%2C8%2C9";
            if (val != 0)
            {
                List<string> strTypes = new List<string>();
                foreach (SearchEntryType type in Enum.GetValues(typeof(SearchEntryType)))
                {
                    if (val.HasFlag(type))
                    {
                        double pow = Math.Log((double)type, (double)2) + 1;
                        strTypes.Add(pow.ToString());
                    }
                }

                result = String.Join("%2C", strTypes.ToArray());
            }

            return result;
        }

        #endregion
    }
}

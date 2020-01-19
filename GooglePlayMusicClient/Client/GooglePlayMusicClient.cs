﻿using GooglePlayMusicAPI.Client;
using GooglePlayMusicAPI.Models.GooglePlayMusicModels;
using GooglePlayMusicAPI.Models.RequestModels;
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI
{
    public class GooglePlayMusicClient
    {
        private IRequestClient requestClient;

        private static string SJ_URL_BASE = "https://mclients.googleapis.com/sj/v2.5/";
        private static string SJ_URL_TRACKS = SJ_URL_BASE + "trackfeed";
        private static string SJ_URL_PLAYLISTS_FEED = SJ_URL_BASE + "playlistfeed";
        private static string SJ_URL_PLAYLISTS_BATCH = SJ_URL_BASE + "playlistbatch";
        private static string SJ_URL_PLAYLISTS_ENTRY_FEED = SJ_URL_BASE + "plentryfeed";
        private static string SJ_URL_PLAYLIST_ENTRIES_BATCH = SJ_URL_BASE + "plentriesbatch";
        private static string SJ_URL_SEARCH = SJ_URL_BASE + "query";
        private static string SJ_URL_TRACK = SJ_URL_BASE + "fetchtrack";
        private static string SJ_URL_TRACK_BATCH = SJ_URL_BASE + "trackbatch";
        private static string SJ_URL_ALBUM = SJ_URL_BASE + "fetchalbum";
        private static string SJ_URL_ARTIST = SJ_URL_BASE + "fetchartist";
        private static string SJ_URL_DEVICE_MANAGEMENT = SJ_URL_BASE + "devicemanagementinfo";
        private static string SJ_URL_CONFIG = SJ_URL_BASE + "config";

        private static string SJ_URL_STREAM = "https://mclients.googleapis.com/music/";
        private static string SJ_URL_STREAM_TRACK = SJ_URL_STREAM + "mplay";

        public bool IsSubscribed { get; set; }

        public GooglePlayMusicClient(string applicationName)
        {
            requestClient = new OAuth2Client(applicationName);
        }

        public GooglePlayMusicClient(IRequestClient inRequestClient)
        {
            requestClient = inRequestClient;
        }

        public static GooglePlayMusicClient GetClientWithEmailPasswordLogin()
        {
            return new GooglePlayMusicClient(new MasterLoginClient(MasterLoginClient.GMUSICAPI_CLIENTID));
        }

        #region Account functions

        public bool LoggedIn()
        {
            return requestClient.IsLoggedIn();
        }

        /// <summary>
        /// Log in to Google Play Music using OAuth2 and set configuration
        /// </summary>
        /// <returns>boolean indicating success or failure</returns>
        public async Task<bool> LoginAsync()
        {
            bool loggedIn = await requestClient.LoginAsync();
            if (loggedIn)
            {
                List<ConfigListEntry> configList = await GetAccountConfig();
                string subscribeVal = configList.Where(x => x.Key == "isNautilusUser").First().Value;
                IsSubscribed = Boolean.Parse(subscribeVal);
            }

            return loggedIn;
        }

        /// <summary>
        /// Log in to google play with email and password then set configuration
        /// 
        /// Generally shouldn't need to use this and should always prefer Login with OAuth2.
        /// If you do need to use this, then you must create the GooglePlayMusicClient with a 
        /// MasterLoginClient since the request client used by default doesn't support this.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> LoginWithEmailPasswordAsync(string email, string password)
        {
            bool loggedIn = await requestClient.LoginWithEmailPasswordAsync(email, password);
            if (loggedIn)
            {
                List<ConfigListEntry> configList = await GetAccountConfig();
                string subscribeVal = configList.Where(x => x.Key == "isNautilusUser").First().Value;
                IsSubscribed = Boolean.Parse(subscribeVal);
            }

            return loggedIn;
        }

        /// <summary>
        /// Gets all of the configuration values for the account (e.g. if subscribed to google play all access)
        /// </summary>
        /// <returns></returns>
        public async Task<List<ConfigListEntry>> GetAccountConfig()
        {
            ConfigListResponse response = await GetAsync<ConfigListResponse>(SJ_URL_CONFIG);
            return response.Data.Entries;
        }

        /// <summary>
        /// Get a list of devices associated with the account
        /// </summary>
        /// <returns></returns>
        public async Task<List<Device>> GetDevicesAsync()
        {
            IncrementalResponse<Device> response = await GetAsync<IncrementalResponse<Device>>(SJ_URL_DEVICE_MANAGEMENT);
            return response.Data.Items;
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
            return await IncrementalPostAsync<Track>(SJ_URL_TRACKS, tracksToGet);
        }

        /// <summary>
        /// Gets track information
        /// </summary>
        /// <param name="trackId">Id of the track</param>
        /// <returns>Track information</returns>
        public async Task<Track> GetTrackAsync(string trackId)
        {
            NameValueCollection additionalParams = new NameValueCollection();

            if (trackId.StartsWith("T"))
            {
                additionalParams["storeId"] = trackId;
            }

            additionalParams["nid"] = trackId;

            return await GetAsync<Track>(SJ_URL_TRACK, additionalParams);
        }

        public async Task<List<Track>> RateTracksAsync(List<Track> tracks, Rating rating)
        {
            foreach (Track track in tracks)
            {
                track.Rating = rating;
            }

            return await PostAsync<List<Track>>(SJ_URL_TRACK_BATCH, null /*request data*/);
        }

        /// <summary>
        /// Gets artist information
        /// </summary>
        /// <param name="artistId">Id of the artist</param>
        /// <param name="includeAlbums">Fetches the artist's albums if true</param>
        /// <param name="maxTopTracks">The max number of the artist's top tracks to fetch</param>
        /// <param name="maxRelatedArtists">The max number of related artists to fetch</param>
        /// <returns>Artist information</returns>
        public async Task<Artist> GetArtistAsync(string artistId, bool includeAlbums = true, int maxTopTracks = 5, int maxRelatedArtists = 5)
        {
            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["nid"] = artistId;
            additionalParams["include-albums"] = includeAlbums.ToString();
            additionalParams["num-top-tracks"] = maxTopTracks.ToString();
            additionalParams["num-related-artists"] = maxRelatedArtists.ToString();

            return await GetAsync<Artist>(SJ_URL_ARTIST, additionalParams);
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

            return await GetAsync<Album>(SJ_URL_ALBUM, additionalParams);
        }

        /// <summary>
        /// Searches for the provided query
        /// </summary>
        /// <param name="searchQuery">Query to search for</param>
        /// <param name="types">Bitwise combination of SearchEntryTypes to search for</param>
        /// <param name="maxResults">Max results</param>
        /// <returns></returns>
        public async Task<SearchResult> SearchAsync(string searchQuery, SearchEntryType[] types = null, int maxResults = 0)
        {
            if (maxResults > 100)
            {
                // Google's API will only return 10 results if you set max-results to a value over 100
                // https://github.com/simon-weber/gmusicapi/issues/520
                // I'm making the assumption here that if maxResults is greather than 100, you probably want
                // as many results as possible.
                maxResults = 0;
            }

            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["q"] = searchQuery;
            additionalParams["ct"] = GetSearchEntryTypeFromValue(types);
            if (maxResults != 0)
            {
                additionalParams["max-results"] = maxResults.ToString();
            }

            SearchResponse response = await GetAsync<SearchResponse>(SJ_URL_SEARCH, additionalParams);
            return new SearchResult(response);
        }

        /// <summary>
        /// Gets a stream url for the provided track
        /// The stream url expires after 1 minute and only one instance of the stream is allowed
        /// </summary>
        /// <param name="deviceId">A device id</param>
        /// <param name="trackId">Id or StoreId of the track</param>
        /// <param name="quality">Quality of the stream</param>
        /// <returns></returns>
        public async Task<string> GetStreamUrlAsync(string deviceId, string trackId, string quality = "hi")
        {
            // Android device ids are now sent in base 10
            string deviceIdToSend = deviceId;
            if (IsAndroidDeviceId(deviceIdToSend))
            {
                if (deviceIdToSend.Contains("0x"))
                {
                    deviceIdToSend = deviceIdToSend.Replace("0x", "");
                }

                deviceIdToSend = Convert.ToInt64(deviceIdToSend, 16).ToString();
            }

            NameValueCollection additionalHeaders = new NameValueCollection();
            additionalHeaders["X-Device-ID"] = deviceIdToSend;

            NameValueCollection additionalParams = new NameValueCollection();
            additionalParams["opt"] = quality;
            additionalParams["pt"] = "e";
            additionalParams["net"] = "mob"; // mobile?
            additionalParams.Add(GetSaltAndSig(trackId));

            //  if track is AllAccess, use mjck, else use songid
            if (trackId.First() == 'T')
            {
                additionalParams["mjck"] = trackId;
            }
            else
            {
                additionalParams["songid"] = trackId;
            }


            string finalUrl = BuildRequestUrl(SJ_URL_STREAM_TRACK, additionalParams);
            return await requestClient.PerformGetStringAsync(finalUrl, additionalHeaders);
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
            return await IncrementalPostAsync<Playlist>(SJ_URL_PLAYLISTS_FEED, playlistsToGet);
        }

        /// <summary>
        /// Gets all playlist entries
        /// </summary>
        /// <param name="entriesToGet">Numer of playlist entries to get, default is all of them</param>
        /// <returns></returns>
        public async Task<List<PlaylistEntry>> GetPlaylistEntriesAsync(int entriesToGet = 0)
        {
            return await IncrementalPostAsync<PlaylistEntry>(SJ_URL_PLAYLISTS_ENTRY_FEED, entriesToGet);
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
                List<PlaylistEntry> thisPlaylistSongs = entries.Where(s => s.PlaylistID == playlist.Id).ToList();
                playlist.Songs = thisPlaylistSongs.OrderBy(s => long.Parse(s.AbsolutePosition)).ToList();
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
        public async Task<MutatePlaylistResponse> CreatePlaylistAsync(string name, string description = null, PlaylistShareState shareState = PlaylistShareState.Private)
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
                            {"shareState", shareState == PlaylistShareState.Private ? "PRIVATE" : "PUBLIC" },
                            {"description", description}
                        }
                    } }
                }
            } };

            return await PostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
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

            return await PostAsync<MutateResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
        }

        /// <summary>
        /// Renames a playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist to be modified</param>
        /// <param name="name">New name for the playlist (or null to make no changes)</param>
        /// <param name="description">New description for the playlist (or null to make no changes)</param>
        /// <param name="shareState">New share state (PUBLIC or PRIVATE) (or null to make no changes)</param>
        /// <returns></returns>
        public async Task<MutateResponse> UpdatePlaylistAsync(string playlistId, string name, string description = null, PlaylistShareState? shareState = null)
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

            return await PostAsync<MutateResponse>(SJ_URL_PLAYLISTS_BATCH, requestData);
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
                // song.Id can be null if it's not in user's library
                string trackId = song.Id;
                if (trackId == null)
                {
                    trackId = song.StoreID;
                }

                JObject songJObject = new JObject
                    {
                    { "clientId", current_uid.ToString() },
                    { "creationTimestamp", -1 },
                    { "deleted", false },
                    { "lastModifiedTimestamp", 0},
                    { "playlistId", playlistId },
                    { "source", 1 },
                    {"trackId", trackId }
                    };

                if (trackId.First() == 'T')
                {
                    songJObject["source"] = 2;
                }

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

            return await PostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLIST_ENTRIES_BATCH, requestData);
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
                string trackId = entry.ID;
                if (trackId == null)
                {
                    trackId = entry.TrackID;
                }

                songsToDelete.Add(new JObject
                {
                {"delete", trackId}
                });
            }

            JObject requestData = new JObject
            {{
                 "mutations", songsToDelete
             }};

            return await PostAsync<MutatePlaylistResponse>(SJ_URL_PLAYLIST_ENTRIES_BATCH, requestData);
        }


        #endregion

        #region Http Helper functions
        private async Task<T> GetAsync<T>(string url, NameValueCollection additionalParams = null, NameValueCollection additionalHeaders = null)
        {
            string finalUrl = BuildRequestUrl(url, additionalParams);
            return await requestClient.PerformGetAsync<T>(finalUrl, additionalHeaders);
        }

        private async Task<T> PostAsync<T>(string url, JObject requestData, NameValueCollection additionalParams = null)
        {
            string finalUrl = BuildRequestUrl(url, additionalParams);
            return await requestClient.PerformPostAsync<T>(finalUrl, requestData);
        }

        private async Task<List<T>> IncrementalPostAsync<T>(string url, int itemsToGet, NameValueCollection additionalParams = null)
        {
            string finalUrl = BuildRequestUrl(url, additionalParams);
            return await requestClient.PerformIncrementalPostAsync<T>(finalUrl, itemsToGet);
        }

        private string BuildRequestUrl(string urlBase, NameValueCollection additionalParams)
        {
            var builder = new UriBuilder(urlBase);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["alt"] = "json";
            query["dv"] = "0";
            query["hl"] = "en_US";
            query["tier"] = IsSubscribed ? "aa" : "fr";

            if (additionalParams != null)
            {
                query.Add(additionalParams);
            }

            builder.Query = query.ToString();

            string combinedUrl = builder.ToString();

            return combinedUrl;
        }

        #endregion

        #region Helper Functions
        /// <summary>
        /// Generate salt and signature based on method from
        /// https://github.com/simon-weber/gmusicapi/blob/develop/gmusicapi/protocol/webclient.py
        /// </summary>
        /// <param name="trackId">Id of track</param>
        /// <returns>NameValueCollection containing slt and sig</returns>
        private NameValueCollection GetSaltAndSig(string trackId)
        {
            NameValueCollection result = new NameValueCollection();

            Random rand = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string salt = new string(Enumerable.Repeat(chars, 13).Select(s => s[rand.Next(s.Length)]).ToArray());
            result["slt"] = salt;
            string saltedTrack = trackId + salt;
            
            string key = GetKey();
            byte[] keyBin = Encoding.ASCII.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(keyBin);
            byte[] binToHash = Encoding.ASCII.GetBytes(saltedTrack);
            byte[] hashedBin = hmac.ComputeHash(binToHash);
            string hashedStr = Convert.ToBase64String(hashedBin);
            string urlSafeSig = hashedStr.Replace('+', '-').Replace('/', '_').Replace('=', '.');
            string sig = urlSafeSig.Substring(0, urlSafeSig.Length - 1);
            result["sig"] = sig;

            return result;
        }

        /// <summary>
        ///  Calculate the key based on the method from:
        ///  https://github.com/simon-weber/gmusicapi/blob/develop/gmusicapi/protocol/mobileclient.py
        ///  McStreamCall
        /// </summary>
        /// <returns>string representing the key to use in hash to find sig</returns>
        private string GetKey()
        {
            // Calculate the key based on the method from:
            // https://github.com/simon-weber/gmusicapi/blob/develop/gmusicapi/protocol/mobileclient.py
            // McStreamCall
            string s1Base64 = "VzeC4H4h+T2f0VI180nVX8x+Mb5HiTtGnKgH52Otj8ZCGDz9jRWyHb6QXK0JskSiOgzQfwTY5xgLLSdUSreaLVMsVVWfxfa8Rw==";
            string s2Base64 = "ZAPnhUkYwQ6y5DdQxWThbvhJHN8msQ1rqJw0ggKdufQjelrKuiGGJI30aswkgCWTDyHkTGK9ynlqTkJ5L4CiGGUabGeo8M6JTQ==";
            byte[] s1Bytes = Convert.FromBase64String(s1Base64);
            byte[] s2Bytes = Convert.FromBase64String(s2Base64);

            byte[] s1Ands2 = new byte[s1Bytes.Length];
            for (int i = 0; i < s1Bytes.Length; i++)
            {
                s1Ands2[i] = (byte)(s1Bytes[i] ^ s2Bytes[i]);
            }

            return Encoding.ASCII.GetString(s1Ands2);
        }

        private string GetSearchEntryTypeFromValue(SearchEntryType[] types)
        {
            if (types == null)
            {
                return "1%2C2%2C3%2C4%2C5%2C6%2C7%2C8%2C9";
            }

            int i = 0;
            string result = "";
            foreach (SearchEntryType type in types)
            {
                if (i == 0)
                {
                    result += (int)type;
                }
                else
                {
                    result += "%2C" + (int)type;
                }
                i++;
            }

            return result;
        }

        private bool IsAndroidDeviceId(string deviceId)
        {
            Regex rgx = new Regex("^0x[a-z0-9]*$");
            return deviceId.Length == 18 && rgx.IsMatch(deviceId);
        }

        #endregion
    }
}

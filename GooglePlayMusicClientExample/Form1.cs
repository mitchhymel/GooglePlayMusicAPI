using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GooglePlayMusicAPI;
using System.Diagnostics;
using GooglePlayMusicAPI.Models.GooglePlayMusicModels;
using GooglePlayMusicAPI.Models.RequestModels;

namespace GooglePlayMusicClientExample
{
    public partial class Form1 : Form
    {
        private List<Playlist> AllPlaylists = new List<Playlist>();
        private List<Track> AllSongs = new List<Track>();
        private List<PlaylistEntry> AllEntries = new List<PlaylistEntry>();
        private List<Device> AllDevices = new List<Device>();

        GooglePlayMusicClient gpmClient = new GooglePlayMusicClient();

        public Form1()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            bool loggedIn = await gpmClient.LoginAsync(Secret.USERNAME, Secret.PASSWORD);
            this.statusLabel.Text = String.Format("Login Status: {0}", loggedIn);
        }

        private async void getTracksButton_Click(object sender, EventArgs e)
        {
            try
            {
                List<Track> library = await gpmClient.GetLibraryAsync();
                AllSongs = library.OrderBy(x => x.Artist).ToList();
                songListBox.Items.Clear();
                songListBox.Items.AddRange(AllSongs.ToArray());
                songTotalLabel.Text = String.Format("Total songs: {0}", AllSongs.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Get playlists
        // Get all the playlists tied to this account and display them

        private async void getPlaylistsButton_Click(object sender, EventArgs e)
        {
            List<Playlist> playlists = await gpmClient.GetPlaylistsWithEntriesAsync();
            AllPlaylists = playlists.OrderBy(x => x.Name).ToList();
            playlistListBox.Items.Clear();
            playlistListBox.Items.AddRange(AllPlaylists.ToArray());
        }

        #endregion

        #region Create playlists
        private async void createPlaylistButton_Click(object sender, EventArgs e)
        {
            await gpmClient.CreatePlaylistAsync(createPlaylistName.Text);
            getPlaylistsButton.PerformClick();
        }

        #endregion

        #region Delete playlist
        // Deletes the playlist currently selected by the user
        private async void deletePlaylistButton_Click(object sender, EventArgs e)
        {
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            await gpmClient.DeletePlaylistAsync(selectedPlaylist.Id);
            getPlaylistsButton.PerformClick();
        }

        #endregion

        #region Modify playlists
        private async void addSongsButton_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection songsSelected = songListBox.SelectedItems;
            List<Track> songsList = new List<Track>();
            foreach (Track song in songsSelected)
                songsList.Add(song);

            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;

            await gpmClient.AddToPlaylistAsync(selectedPlaylist.Id, songsList);
            getPlaylistsButton.PerformClick();

        }

        // This could delete multiple selections at once, but it doesn't
        private async void deleteSong_Click(object sender, EventArgs e)
        {
            Track selectedSong = (Track)playlistSongsBox.SelectedItem;
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            PlaylistEntry songEntry = selectedPlaylist.Songs.First(s => s.TrackID == selectedSong.Id);
            await gpmClient.RemoveFromPlaylistAsync(new List<PlaylistEntry> { songEntry });
        }

        #endregion

        #region Update playlist songs
        private async void playlistListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            playlistSongsBox.Items.Clear();
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            foreach (PlaylistEntry song in selectedPlaylist.Songs)
            {
                if (!song.Deleted)
                {
                    Track thisSong = null;
                    if (song.IsAllAccessTrack())
                    {
                        thisSong = AllSongs.FirstOrDefault(s => s.StoreID == song.TrackID);
                    }
                    else
                    {
                        thisSong = AllSongs.FirstOrDefault(s => s.Id == song.TrackID);
                    }

                    if (thisSong != null)
                    {
                        playlistSongsBox.Items.Add(thisSong);
                    }
                }
            }
        }

        #endregion

        private async void renameButton_Click(object sender, EventArgs e)
        {
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            await gpmClient.UpdatePlaylistAsync(selectedPlaylist.Id, renamePlaylistTextBox.Text, description:"test test new description test");
            getPlaylistsButton.PerformClick();
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // SearchResult result = await gpmClient.SearchAsync("Fallout", 20, SearchEntryType.Artist);
                // Artist art = await gpmClient.GetArtistAsync("A6gc34bsyqpyzttc7apngoabxba");
                Album album = await gpmClient.GetAlbumAsync("Ba4nta73ydbdh5tgpte66skpyhu");

                //if (AllDevices.Count == 0)
                //{
                //    AllDevices = await gpmClient.GetDevicesAsync();
                //}

                //Device androidDevice = AllDevices.Where(d => d.Type == Device.DeviceType.IOS).FirstOrDefault();
                //if (androidDevice == null)
                //{
                //    Debug.WriteLine("No android devices found");
                //    return;
                //}


                //ListBox.SelectedObjectCollection songsSelected = songListBox.SelectedItems;
                //Track song = (Track)songsSelected[0];
                //if (song == null)
                //{
                //    Debug.WriteLine("No track selected");
                //}

                //string url = await gpmClient.GetStreamUrlAsync(androidDevice.Id, song.ID);
                //string path = song.ToString() + ".mp3";

                //Debug.WriteLine(path);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1000, 700);
        }
    }
}

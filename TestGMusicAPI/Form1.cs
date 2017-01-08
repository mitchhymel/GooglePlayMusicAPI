using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GooglePlayMusicAPI;

namespace TestGMusicAPI
{
    public partial class Form1 : Form
    {
        private List<Playlist> AllPlaylists = new List<Playlist>();
        private List<Track> AllSongs = new List<Track>();
        private List<PlaylistEntry> AllEntries = new List<PlaylistEntry>();

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
            List<Track> library = await gpmClient.GetLibraryAsync();
            AllSongs = library;
            foreach(Track song in library)
            {
                songListBox.Items.Add(song);
            }

            songTotalLabel.Text = String.Format("Total songs: {0}", library.Count);
        }

        #region Get playlists
        // Get all the playlists tied to this account and display them

        private async void getPlaylistsButton_Click(object sender, EventArgs e)
        {
            List<Playlist> playlists = await gpmClient.GetPlaylistsWithEntriesAsync();
            AllPlaylists = playlists;
            playlistListBox.Items.Clear();
            foreach (Playlist playlist in playlists)
            {
                playlistListBox.Items.Add(playlist);
            }
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
            await gpmClient.DeletePlaylistAsync(selectedPlaylist.ID);
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

            await gpmClient.AddToPlaylistAsync(selectedPlaylist.ID, songsList);
            getPlaylistsButton.PerformClick();

        }

        // This could delete multiple selections at once, but it doesn't
        private async void deleteSong_Click(object sender, EventArgs e)
        {
            Track selectedSong = (Track)playlistSongsBox.SelectedItem;
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            PlaylistEntry songEntry = selectedPlaylist.Songs.First(s => s.TrackID == selectedSong.ID);
            await gpmClient.RemoveFromPlaylistAsync(new List<PlaylistEntry> { songEntry });
        }

        #endregion

        #region Update playlist songs
        private void playlistListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            playlistSongsBox.Items.Clear();
            Playlist selectedPlaylist = (Playlist)playlistListBox.SelectedItem;
            foreach (PlaylistEntry song in selectedPlaylist.Songs)
            {
                if (!song.Deleted)
                {
                    Track thisSong = AllSongs.FirstOrDefault(s => s.ID == song.TrackID);
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
            await gpmClient.UpdatePlaylistAsync(selectedPlaylist.ID, renamePlaylistTextBox.Text, description:"test test new description test");
            getPlaylistsButton.PerformClick();
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            SearchResponse response = await gpmClient.SearchAsync("pmtoday", GooglePlayMusicClient.SearchEntryType.ARTIST | GooglePlayMusicClient.SearchEntryType.SONG);
        }
    }
}

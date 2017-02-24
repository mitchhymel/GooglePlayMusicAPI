namespace GooglePlayMusicClientExample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.loginButton = new System.Windows.Forms.Button();
            this.getTracksButton = new System.Windows.Forms.Button();
            this.getPlaylistsButton = new System.Windows.Forms.Button();
            this.createPlaylistName = new System.Windows.Forms.TextBox();
            this.createPlaylistButton = new System.Windows.Forms.Button();
            this.deletePlaylistButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.addSongsButton = new System.Windows.Forms.Button();
            this.songListBox = new System.Windows.Forms.ListBox();
            this.playlistListBox = new System.Windows.Forms.ListBox();
            this.playlistSongsBox = new System.Windows.Forms.ListBox();
            this.songTotalLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renamePlaylistTextBox = new System.Windows.Forms.TextBox();
            this.renameButton = new System.Windows.Forms.Button();
            this.deleteSong = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(70, 23);
            this.loginButton.Margin = new System.Windows.Forms.Padding(6);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(150, 44);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // getTracksButton
            // 
            this.getTracksButton.Location = new System.Drawing.Point(232, 23);
            this.getTracksButton.Margin = new System.Windows.Forms.Padding(6);
            this.getTracksButton.Name = "getTracksButton";
            this.getTracksButton.Size = new System.Drawing.Size(150, 44);
            this.getTracksButton.TabIndex = 1;
            this.getTracksButton.Text = "Get Tracks";
            this.getTracksButton.UseVisualStyleBackColor = true;
            this.getTracksButton.Click += new System.EventHandler(this.getTracksButton_Click);
            // 
            // getPlaylistsButton
            // 
            this.getPlaylistsButton.Location = new System.Drawing.Point(70, 79);
            this.getPlaylistsButton.Margin = new System.Windows.Forms.Padding(6);
            this.getPlaylistsButton.Name = "getPlaylistsButton";
            this.getPlaylistsButton.Size = new System.Drawing.Size(150, 44);
            this.getPlaylistsButton.TabIndex = 2;
            this.getPlaylistsButton.Text = "Get Playlists";
            this.getPlaylistsButton.UseVisualStyleBackColor = true;
            this.getPlaylistsButton.Click += new System.EventHandler(this.getPlaylistsButton_Click);
            // 
            // createPlaylistName
            // 
            this.createPlaylistName.Location = new System.Drawing.Point(30, 135);
            this.createPlaylistName.Margin = new System.Windows.Forms.Padding(6);
            this.createPlaylistName.Name = "createPlaylistName";
            this.createPlaylistName.Size = new System.Drawing.Size(334, 31);
            this.createPlaylistName.TabIndex = 3;
            // 
            // createPlaylistButton
            // 
            this.createPlaylistButton.Location = new System.Drawing.Point(398, 131);
            this.createPlaylistButton.Margin = new System.Windows.Forms.Padding(6);
            this.createPlaylistButton.Name = "createPlaylistButton";
            this.createPlaylistButton.Size = new System.Drawing.Size(128, 44);
            this.createPlaylistButton.TabIndex = 4;
            this.createPlaylistButton.Text = "Create Playlist";
            this.createPlaylistButton.UseVisualStyleBackColor = true;
            this.createPlaylistButton.Click += new System.EventHandler(this.createPlaylistButton_Click);
            // 
            // deletePlaylistButton
            // 
            this.deletePlaylistButton.Location = new System.Drawing.Point(24, 1037);
            this.deletePlaylistButton.Margin = new System.Windows.Forms.Padding(6);
            this.deletePlaylistButton.Name = "deletePlaylistButton";
            this.deletePlaylistButton.Size = new System.Drawing.Size(102, 44);
            this.deletePlaylistButton.TabIndex = 6;
            this.deletePlaylistButton.Text = "Delete";
            this.deletePlaylistButton.UseVisualStyleBackColor = true;
            this.deletePlaylistButton.Click += new System.EventHandler(this.deletePlaylistButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(18, 1117);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(79, 25);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Status:";
            // 
            // addSongsButton
            // 
            this.addSongsButton.Location = new System.Drawing.Point(1196, 308);
            this.addSongsButton.Margin = new System.Windows.Forms.Padding(6);
            this.addSongsButton.Name = "addSongsButton";
            this.addSongsButton.Size = new System.Drawing.Size(92, 244);
            this.addSongsButton.TabIndex = 8;
            this.addSongsButton.Text = "<--\r\nAdd Songs\r\n<--";
            this.addSongsButton.UseVisualStyleBackColor = true;
            this.addSongsButton.Click += new System.EventHandler(this.addSongsButton_Click);
            // 
            // songListBox
            // 
            this.songListBox.FormattingEnabled = true;
            this.songListBox.ItemHeight = 25;
            this.songListBox.Location = new System.Drawing.Point(1300, 23);
            this.songListBox.Margin = new System.Windows.Forms.Padding(6);
            this.songListBox.Name = "songListBox";
            this.songListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.songListBox.Size = new System.Drawing.Size(634, 1004);
            this.songListBox.TabIndex = 10;
            // 
            // playlistListBox
            // 
            this.playlistListBox.FormattingEnabled = true;
            this.playlistListBox.ItemHeight = 25;
            this.playlistListBox.Location = new System.Drawing.Point(24, 192);
            this.playlistListBox.Margin = new System.Windows.Forms.Padding(6);
            this.playlistListBox.Name = "playlistListBox";
            this.playlistListBox.Size = new System.Drawing.Size(474, 829);
            this.playlistListBox.TabIndex = 11;
            this.playlistListBox.SelectedIndexChanged += new System.EventHandler(this.playlistListBox_SelectedIndexChanged);
            // 
            // playlistSongsBox
            // 
            this.playlistSongsBox.FormattingEnabled = true;
            this.playlistSongsBox.ItemHeight = 25;
            this.playlistSongsBox.Location = new System.Drawing.Point(538, 23);
            this.playlistSongsBox.Margin = new System.Windows.Forms.Padding(6);
            this.playlistSongsBox.Name = "playlistSongsBox";
            this.playlistSongsBox.Size = new System.Drawing.Size(642, 1004);
            this.playlistSongsBox.TabIndex = 12;
            // 
            // songTotalLabel
            // 
            this.songTotalLabel.AutoSize = true;
            this.songTotalLabel.Location = new System.Drawing.Point(1294, 1117);
            this.songTotalLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.songTotalLabel.Name = "songTotalLabel";
            this.songTotalLabel.Size = new System.Drawing.Size(130, 25);
            this.songTotalLabel.TabIndex = 13;
            this.songTotalLabel.Text = "Total songs:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Location = new System.Drawing.Point(0, 1190);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1946, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(330, 42);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(329, 38);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // renamePlaylistTextBox
            // 
            this.renamePlaylistTextBox.Location = new System.Drawing.Point(140, 1037);
            this.renamePlaylistTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.renamePlaylistTextBox.Name = "renamePlaylistTextBox";
            this.renamePlaylistTextBox.Size = new System.Drawing.Size(196, 31);
            this.renamePlaylistTextBox.TabIndex = 16;
            // 
            // renameButton
            // 
            this.renameButton.Location = new System.Drawing.Point(352, 1037);
            this.renameButton.Margin = new System.Windows.Forms.Padding(6);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(150, 44);
            this.renameButton.TabIndex = 17;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
            // 
            // deleteSong
            // 
            this.deleteSong.Location = new System.Drawing.Point(1196, 563);
            this.deleteSong.Margin = new System.Windows.Forms.Padding(6);
            this.deleteSong.Name = "deleteSong";
            this.deleteSong.Size = new System.Drawing.Size(92, 206);
            this.deleteSong.TabIndex = 18;
            this.deleteSong.Text = "-->\r\nRmv\r\nSongs\r\n-->";
            this.deleteSong.UseVisualStyleBackColor = true;
            this.deleteSong.Click += new System.EventHandler(this.deleteSong_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(232, 79);
            this.searchButton.Margin = new System.Windows.Forms.Padding(6);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(150, 44);
            this.searchButton.TabIndex = 19;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1946, 1212);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.renamePlaylistTextBox);
            this.Controls.Add(this.deleteSong);
            this.Controls.Add(this.renameButton);
            this.Controls.Add(this.songTotalLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.playlistSongsBox);
            this.Controls.Add(this.playlistListBox);
            this.Controls.Add(this.songListBox);
            this.Controls.Add(this.deletePlaylistButton);
            this.Controls.Add(this.createPlaylistButton);
            this.Controls.Add(this.createPlaylistName);
            this.Controls.Add(this.getPlaylistsButton);
            this.Controls.Add(this.getTracksButton);
            this.Controls.Add(this.addSongsButton);
            this.Controls.Add(this.loginButton);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button getTracksButton;
        private System.Windows.Forms.Button getPlaylistsButton;
        private System.Windows.Forms.TextBox createPlaylistName;
        private System.Windows.Forms.Button createPlaylistButton;
        private System.Windows.Forms.Button deletePlaylistButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button addSongsButton;
        private System.Windows.Forms.ListBox songListBox;
        private System.Windows.Forms.ListBox playlistListBox;
        private System.Windows.Forms.ListBox playlistSongsBox;
        private System.Windows.Forms.Label songTotalLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox renamePlaylistTextBox;
        private System.Windows.Forms.Button renameButton;
        private System.Windows.Forms.Button deleteSong;
        private System.Windows.Forms.Button searchButton;
    }
}


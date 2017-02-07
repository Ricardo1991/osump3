using osu_mp3.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace osu_mp3
{
    public partial class OsuMp3 : Form
    {
        private List<Song> songs = new List<Song>();
        private List<Song> songs2 = new List<Song>();

        private List<int> shuffleHistory = new List<int>();

        private int ind = 0;
        private int folders;
        private int counter = 0;
        private string[] subdirectoryEntries;

        private string osuDir;

        public bool SongEnded = true;
        public bool Shuffle = false;

        private Random rnd = new Random();

        private bool refreshing = true;
        private bool exporting = false;

        public OsuMp3()
        {
            InitializeComponent();

            loader1.WorkerReportsProgress = true;
            loader1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            loader1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            loader1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            exporter1.WorkerReportsProgress = true;
            exporter1.DoWork += new DoWorkEventHandler(exporter1_DoWork);
            exporter1.ProgressChanged += new ProgressChangedEventHandler(exporter1_ProgressChanged);
            exporter1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(exporter1_RunWorkerCompleted);

            exporter2.WorkerReportsProgress = true;
            exporter2.DoWork += new DoWorkEventHandler(exporter2_DoWork);
            exporter2.ProgressChanged += new ProgressChangedEventHandler(exporter2_ProgressChanged);
            exporter2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(exporter2_RunWorkerCompleted);

            osuDir = @Settings.Default.dir;

            subdirectoryEntries = Directory.GetDirectories(@osuDir);

            progressBar.Visible = true;

            loader1.RunWorkerAsync();
        }

        private void playSong(int ind)
        {
            GC.Collect();

            string songDir;

            if (Shuffle == true)
                shuffleHistory.Add(ind); //Add this song to the shuffle history

            foreach (Song song in songs)
            {
                if (song.index == ind)
                {
                    songDir = song.getmp3Dir();
                    mediaplayer.URL = songDir;

                    artist.Text = song.getartist();
                    name.Text = song.getname();
                    source.Text = song.getSource();
                    if (song.hasPicture == true)
                        songImage.ImageLocation = song.getimagefile();
                    else
                        songImage.Image = osu_mp3.Properties.Resources.sliderpoint10;
                    break;
                }
            }

            mediaplayer.Ctlcontrols.play();
        }

        private void playnextSong(int id)
        {
            mediaplayer.Ctlcontrols.stop();

            if (Shuffle == false)
            {
                if (songListBox.SelectedIndex + 1 == songListBox.Items.Count)
                {
                    songListBox.SelectedIndex = 0;
                    id = (int)songListBox.SelectedValue;
                }
                else
                {
                    songListBox.SelectedIndex = songListBox.SelectedIndex + 1;
                    id = (int)songListBox.SelectedValue;
                }
            }
            else
            {
                if (songListBox.Items.Count > 1)
                {
                    bool isNew = true;
                    int previousSong = songListBox.SelectedIndex;

                    if (shuffleHistory.Count >= songListBox.Items.Count)                                       //If all songs were played, clear and start over
                    {
                        shuffleHistory.Clear();
                    }
                    do
                    {
                        songListBox.SelectedIndex = rnd.Next(songListBox.Items.Count);                       //Prevent the same song for being played twice
                        isNew = true;

                        foreach (int id2 in shuffleHistory)
                        {
                            if (id2 == (int)songListBox.SelectedValue || songListBox.SelectedIndex == previousSong) //Dont play the same song twice in a row
                                isNew = false;
                        }
                    }
                    while (isNew == false);

                    id = (int)songListBox.SelectedValue;
                }
                else
                {
                    songListBox.SelectedIndex = songListBox.SelectedIndex;
                    id = (int)songListBox.SelectedValue;
                }
            }

            playSong(id);
        }

        private void listBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int id = (int)this.songListBox.SelectedValue;

            if (id != ListBox.NoMatches)
            {
                playSong(id);
            }
        }

        private void Player_MediaError(object pMediaObject)
        {
            MessageBox.Show("Cannot play media file.");
            mediaplayer.Ctlcontrols.stop();
        }

        // On worker thread so do our thing!
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            folders = subdirectoryEntries.Length;
            refreshing = true;

            foreach (string directory in subdirectoryEntries)
            {
                if (loader1.CancellationPending == true)
                {
                    refreshing = false;
                    songs.Clear();
                    return;
                }
                    
                float percentage = (counter / folders);
                loader1.ReportProgress((int)(percentage * 100));

                Song s = new Song(directory, counter);

                if (s.beatTotal >= 1){
                    songs.Add(s);
                    counter++;
                } 
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //int total = 0;
            progressBar.Visible = false;

            songListBox.DataSource = null;
            songListBox.DataSource = songs;

            songListBox.DisplayMember = "fullname";
            songListBox.ValueMember = "index";
            labelTotalSongs.Text = counter.ToString() + " music(s) loaded in";

            refreshing = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            songs.Clear();
            counter = 0;
            loader1.RunWorkerAsync();
        }

        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // the code here will be executed if the user presses Open in
                // the dialog.

                if (loader1.IsBusy)
                    loader1.CancelAsync();

                songs.Clear();
                counter = 0;

                osuDir = this.folderBrowserDialog1.SelectedPath;
                subdirectoryEntries = Directory.GetDirectories(@osuDir);

                progressBar.Visible = true;
                loader1.RunWorkerAsync();

                Settings.Default.dir = @osuDir;
                Settings.Default.Save();
            }
        }

        private void copyAndTag(Song song)
        {
            string sourcePath = System.IO.Path.Combine(Settings.Default.dir, song.Folder);
            string fileName = song.Mp3file;
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string newFileName = song.ColectionID + " - " + song.getname() + fileExtension;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                newFileName = newFileName.Replace(c, '-');
            }

            string folder = song.Folder;

            string sourceFile = song.getmp3Dir();

            string artistFolder = song.getartist();

            artistFolder = artistFolder.Replace("*", "");
            artistFolder = artistFolder.Replace(":", "");

            foreach (char b in Path.GetInvalidPathChars())
            {
                artistFolder = artistFolder.Replace(b.ToString(), "");
            }

            string targetPath = @Settings.Default.exportDir + "\\" + artistFolder;

            targetPath = targetPath.Replace("*", "");

            foreach (char b in Path.GetInvalidPathChars())
            {
                targetPath = targetPath.Replace(b.ToString(), "");
            }

            string destFile = System.IO.Path.Combine(targetPath, newFileName);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            try
            {
                File.Copy(sourceFile, destFile, true);

                TagLib.File f = TagLib.File.Create(destFile);
                f.Tag.Performers = new[] { song.getartist() };
                f.Tag.AlbumArtists = new[] { song.getartist() };
                f.Tag.Title = song.getname();
                f.Tag.Album = song.getSource();
                f.Save();
            }
            catch (FileNotFoundException)
            {
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)       //SEARCH BAR CHANGED
        {
            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                songs2.Clear();
                this.songListBox.DataSource = null;
                this.songListBox.DataSource = songs2;

                string[] searchResults;

                searchResults = searchTextBox.Text.ToLower().Split(new char[] { ' ' });

                foreach (Song song in songs)
                {
                    if (song.getartist().ToLower().Contains(searchResults[0]) ||
                        song.getname().ToLower().Contains(searchResults[0]) ||
                        song.getTags().ToLower().Contains(searchResults[0]) ||
                        song.getSource().ToLower().Contains(searchResults[0]))
                    {
                        songs2.Add(song);
                    }

                    foreach (string query in searchResults)
                    {
                        if (query != searchResults[0])
                        {
                            if (!song.getartist().ToLower().Contains(query) &&
                                !song.getname().ToLower().Contains(query) &&
                                !song.getTags().ToLower().Contains(query) &&
                                !song.getSource().ToLower().Contains(query))
                            {
                                songs2.Remove(song);
                            }
                        }
                    }
                }
            }
            else
            {
                this.songListBox.DataSource = null;
                this.songListBox.DataSource = songs;
            }
            this.songListBox.DisplayMember = "fullname";
            this.songListBox.ValueMember = "index";
        }

        private void CheckSong_Tick(object sender, EventArgs e)
        {
            if (SongEnded)
            {
                playnextSong(ind);
                SongEnded = false;
                CheckSong.Stop();
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (mediaplayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                SongEnded = true;
                CheckSong.Start();
            }
        }

        private void b_next_Click(object sender, EventArgs e)
        {
            playnextSong(ind);
        }

        private void b_previous_Click(object sender, EventArgs e)
        {
            mediaplayer.Ctlcontrols.stop();

            if (songListBox.SelectedIndex - 1 < 0)
            {
                songListBox.SelectedIndex = (int)songListBox.Items.Count - 1;
                ind = (int)songListBox.SelectedValue;
            }
            else
            {
                songListBox.SelectedIndex = songListBox.SelectedIndex - 1;
                ind = (int)songListBox.SelectedValue;
            }

            playSong(ind);
        }

        private void b_shuffle_Click(object sender, EventArgs e)        //Shuffle Button Toggle
        {
            shuffleHistory.Clear();
            if (Shuffle == false)
            {
                this.b_shuffle.BackColor = System.Drawing.Color.MediumSeaGreen;
                Shuffle = true;
                b_previous.Enabled = false;
            }
            else
            {
                this.b_shuffle.BackColor = System.Drawing.Color.IndianRed;
                Shuffle = false;
                b_previous.Enabled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.MediaNextTrack)
            {
                playnextSong(ind);
            }
            if (e.KeyCode == Keys.MediaPreviousTrack)
            {
                playPrevious();
            }
        }

        private void axWindowsMediaPlayer1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.MediaNextTrack)
            {
                playnextSong(ind);
            }
            if (e.KeyCode == Keys.MediaPreviousTrack)
            {
                playPrevious();
            }
        }

        private void playPrevious()
        {
            if (Shuffle == false)
            {
                mediaplayer.Ctlcontrols.stop();

                if (songListBox.SelectedIndex - 1 < 0)
                {
                    songListBox.SelectedIndex = (int)songListBox.Items.Count - 1;
                    ind = (int)songListBox.SelectedValue;
                }
                else
                {
                    songListBox.SelectedIndex = songListBox.SelectedIndex - 1;
                    ind = (int)songListBox.SelectedValue;
                }

                playSong(ind);
            }
        }

        private void exportSelected(object sender, EventArgs e)
        {
            ind = (int)songListBox.SelectedValue;

            foreach (Song song in songs)
            {
                if (song.index == ind)
                {
                    copyAndTag(song);
                    break;
                }
            }
        }

        private void exportSearchResult(object sender, EventArgs e)
        {
            if (!exporting && !refreshing)
                exporter2.RunWorkerAsync();

            progressBar.Visible = true;
        }

        private void exportAll(object sender, EventArgs e)
        {
            if (!exporting && !refreshing)
                exporter1.RunWorkerAsync();

            progressBar.Visible = true;
        }

        private void exporter1_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            int total = songs.Count;

            exporting = true;

            foreach (Song song in songs)
            {
                copyAndTag(song);
                count++;
                exporter1.ReportProgress((int)((count / total) * 100));
            }
        }

        private void exporter2_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            int total = songs2.Count;

            exporting = true;

            foreach (Song song in songs2)
            {
                copyAndTag(song);
                count++;
                exporter2.ReportProgress((int)((count / total) * 100));
            }
        }

        private void exporter1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void exporter2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void exporter1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Visible = false;
            exporting = false;
        }

        private void exporter2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Visible = false;
            exporting = false;
        }

        private void changeExportDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string dir = this.folderBrowserDialog1.SelectedPath;
                Settings.Default.exportDir = @dir;
                Settings.Default.Save();
            }
        }
    }
}
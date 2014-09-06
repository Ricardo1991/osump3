using osu_mp3.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Runtime.InteropServices;

namespace osu_mp3
{
    public partial class Form1 : Form
    {
        List<Song> songs = new List<Song>();
        List<Song> songs2 = new List<Song>();

        List<int> shuffleHistory= new List<int>();

        public WMPLib.IWMPPlaylist p;
        public WMPLib.IWMPMedia temp;

        int ind = 0;
        int folders;
        int counter = 0;
        string[] subdirectoryEntries;
        
        string osuDir;

        public bool SongEnded = true;
        public bool Shuffle = false;

        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            

            osuDir = @Settings.Default.dir;

            subdirectoryEntries = Directory.GetDirectories(@osuDir);

            progressBar.Visible = true;
         
            backgroundWorker1.RunWorkerAsync();
        }

        private void playSong(int ind)
        {
            GC.Collect();

            string songDir;

            if (Shuffle == true)
                shuffleHistory.Add(ind); //Add this song to the shuffle history

            foreach (Song song in songs)
            { 
                if(song.index == ind)
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
                if (songListBox.SelectedIndex + 1 == (int)songListBox.Items.Count)
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
            else {
                if (songListBox.Items.Count > 1)
                {
                    bool isNew = true;
                    int previousSong = songListBox.SelectedIndex;

                    if (shuffleHistory.Count >= songListBox.Items.Count)                                       //If all songs were played, clear and start over
                    {
                        shuffleHistory.Clear();
                    }
                    do{
                        songListBox.SelectedIndex = rnd.Next((int)songListBox.Items.Count);                       //Prevent the same song for being played twice
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

            if (id != System.Windows.Forms.ListBox.NoMatches)
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
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            folders = subdirectoryEntries.Length;

            foreach (string directory in subdirectoryEntries)
            {
                float percentage = ((float)counter / (float)folders);
                backgroundWorker1.ReportProgress((int)(percentage * 100));
                songs.Add(new Song(directory, counter));

                counter++;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //int total = 0;
            progressBar.Visible = false;

            this.songListBox.DataSource = null;
            this.songListBox.DataSource = songs;

            this.songListBox.DisplayMember = "fullname";
            this.songListBox.ValueMember = "index";
            labelTotalSongs.Text = counter.ToString() + " music(s) loaded in";
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            songs.Clear();
            counter = 0;
            backgroundWorker1.RunWorkerAsync();
        }

        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // the code here will be executed if the user presses Open in
                // the dialog.

                songs.Clear();
                counter = 0;

                osuDir = this.folderBrowserDialog1.SelectedPath;
                subdirectoryEntries = Directory.GetDirectories(@osuDir);

                progressBar.Visible = true;
                backgroundWorker1.RunWorkerAsync();

                Settings.Default.dir = @osuDir;
                Settings.Default.Save();
            }
        }


        private void copyAndTag(Song song)
        {
            string sourcePath = System.IO.Path.Combine(Settings.Default.dir , song.Folder);
            string fileName = song.Mp3file;
            string fileExtension=System.IO.Path.GetExtension(fileName);
            string newFileName = song.ColectionID +" - "+ song.getname() + fileExtension;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                newFileName = newFileName.Replace(c, '-');


            }

            string folder = song.Folder;

            string sourceFile = song.getmp3Dir();

            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\osump3 files";
            string destFile = System.IO.Path.Combine(targetPath, newFileName);



            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            System.IO.File.Copy(sourceFile, destFile, true);

            TagLib.File f = TagLib.File.Create(destFile);
            f.Tag.Performers = null;
            f.Tag.Performers = new[] { song.getartist() };
            f.Tag.Title = song.getname();
            f.Save();
            
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)       //SEARCH BAR CHANGED
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                songs2.Clear();
                this.songListBox.DataSource = null;
                this.songListBox.DataSource = songs2;

                foreach (Song song in songs)
                {
                    if (song.getartist().ToLower().Contains(searchTextBox.Text.ToLower()) || 
                        song.getname().ToLower().Contains(searchTextBox.Text.ToLower()) || 
                        song.getTags().ToLower().Contains(searchTextBox.Text.ToLower()) || 
                        song.getSource().ToLower().Contains(searchTextBox.Text.ToLower()))
                    {
                        songs2.Add(song);
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
                songListBox.SelectedIndex = (int)songListBox.Items.Count-1;
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
            else {

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
        }

        private void axWindowsMediaPlayer1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.MediaNextTrack)
            {
                playnextSong(ind);

            }
            if (e.KeyCode == Keys.MediaPreviousTrack)
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
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}

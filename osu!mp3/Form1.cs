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
        //[DllImport("user32.dll")]
        //public static extern
        //    bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        //[DllImport("user32")]
        //public static extern
        //    bool GetMessage(ref Message lpMsg, IntPtr handle, uint mMsgFilterInMain, uint mMsgFilterMax);

        //public const int MOD_ALT = 0x0001;
        //public const int MOD_CONTROL = 0x0002;
        //public const int MOD_SHIFT = 0x004;
        //public const int MOD_NOREPEAT = 0x400;
        //public const int WM_HOTKEY = 0x312;
        //public const int NEXT = 0xB0;
        //public const int PREV = 0xB1;
        //public const int PLAY = 0xB3;

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

            //RegisterHotKey(IntPtr.Zero, 1,  0, NEXT);

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            

            osuDir = @Settings.Default.dir;

            subdirectoryEntries = Directory.GetDirectories(@osuDir);

            toolStripProgressBar1.Visible = true;

            p = axWindowsMediaPlayer1.playlistCollection.newPlaylist("osu");
            
            backgroundWorker1.RunWorkerAsync();

            axWindowsMediaPlayer1.currentPlaylist = p;
        }

        private void playSong(int ind)
        {
            GC.Collect();

            string SongURL;

            if (Shuffle == true)
                shuffleHistory.Add(ind);//Add this song to the shuffle history

            foreach (Song song in songs)
            { 
                if(song.index == ind)
                {
                    SongURL = song.getmp3file();
                    axWindowsMediaPlayer1.URL = SongURL;

                    artist.Text = song.getartist();
                    name.Text = song.getname();
                    source.Text = song.getSource();
                    if (song.hasPicture == true)
                        pictureBox1.ImageLocation = song.getimagefile();
                    else
                        pictureBox1.Image = osu_mp3.Properties.Resources.sliderpoint10;
                    break;
                }
            }
            
            axWindowsMediaPlayer1.Ctlcontrols.play();

            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsReady)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void playnextSong(int ind)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();

            if (Shuffle == false)
            {
                if (listBox1.SelectedIndex + 1 == (int)listBox1.Items.Count)
                {
                    listBox1.SelectedIndex = 0;
                    ind = (int)listBox1.SelectedValue;
                }
                else
                {
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                    ind = (int)listBox1.SelectedValue;
                }

            }
            else {
                if (listBox1.Items.Count > 1)
                {
                    bool isNew = true;
                    int previousSong = listBox1.SelectedIndex;

                    if (shuffleHistory.Count >= listBox1.Items.Count)                                       //If all songs were played, clear and start over
                    {
                        shuffleHistory.Clear();
                    }
                    do{
                        listBox1.SelectedIndex = rnd.Next((int)listBox1.Items.Count);                       //Prevent the same song for being played twice
                        isNew = true;

                        foreach (int id in shuffleHistory)
                        {
                            if (id == (int)listBox1.SelectedValue || listBox1.SelectedIndex == previousSong) //Dont play the same song twice in a row
                                isNew = false;
                        }


                    }
                    while (isNew == false);

                    ind = (int)listBox1.SelectedValue;
                }
                else
                {
                    listBox1.SelectedIndex = listBox1.SelectedIndex;
                    ind = (int)listBox1.SelectedValue;
                }
                
            }

            playSong(ind);

        }

        private void listBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ind = (int)this.listBox1.SelectedValue;

            if (ind != System.Windows.Forms.ListBox.NoMatches)
            {
                playSong(ind);
            }
        }

        private void Player_MediaError(object pMediaObject)
        {
            MessageBox.Show("Cannot play media file.");
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        // On worker thread so do our thing!
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            folders = subdirectoryEntries.Length;
            using (StreamWriter newTask = new StreamWriter("output.txt", false))
            {
                newTask.WriteLine("** Beatmap ID listing **\n");

            }
            foreach (string direct in subdirectoryEntries)
            {
                float percentage = ((float)counter / (float)folders);
                backgroundWorker1.ReportProgress((int)(percentage * 100));
                songs.Add(new Song(@direct, counter));

                counter++;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int total = 0;
            toolStripProgressBar1.Visible = false;

            this.listBox1.DataSource = null;
            this.listBox1.DataSource = songs;

            this.listBox1.DisplayMember = "fullname";
            this.listBox1.ValueMember = "index";
            toolStripLabel1.Text = counter.ToString() + " music(s) loaded in";

            foreach (Song s in songs)
            {
                total = total + s.beattotal;
            }
            labelTotalBeats.Text = total.ToString() + " beatmap(s)";
            
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = true;
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

                toolStripProgressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync();

                Settings.Default.dir = @osuDir;
                Settings.Default.Save();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)       //SEARCH BAR CHANGED
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                songs2.Clear();
                this.listBox1.DataSource = null;
                this.listBox1.DataSource = songs2;

                foreach (Song song in songs)
                {
                    if (song.getartist().ToLower().Contains(textBox1.Text.ToLower()) || 
                        song.getname().ToLower().Contains(textBox1.Text.ToLower()) || 
                        song.getTags().ToLower().Contains(textBox1.Text.ToLower()) || 
                        song.getSource().ToLower().Contains(textBox1.Text.ToLower()))
                    {
                        songs2.Add(song);
                    }
                }
            }
            else
            {
                this.listBox1.DataSource = null;
                this.listBox1.DataSource = songs;

            }
            this.listBox1.DisplayMember = "fullname";
            this.listBox1.ValueMember = "index";
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
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
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
            axWindowsMediaPlayer1.Ctlcontrols.stop();

            if (listBox1.SelectedIndex - 1 < 0)
            {
                listBox1.SelectedIndex = (int)listBox1.Items.Count-1;
                ind = (int)listBox1.SelectedValue;
            }
            else
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
                ind = (int)listBox1.SelectedValue;
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
                    axWindowsMediaPlayer1.Ctlcontrols.stop();

                    if (listBox1.SelectedIndex - 1 < 0)
                    {
                        listBox1.SelectedIndex = (int)listBox1.Items.Count - 1;
                        ind = (int)listBox1.SelectedValue;
                    }
                    else
                    {
                        listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
                        ind = (int)listBox1.SelectedValue;
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
                    axWindowsMediaPlayer1.Ctlcontrols.stop();

                    if (listBox1.SelectedIndex - 1 < 0)
                    {
                        listBox1.SelectedIndex = (int)listBox1.Items.Count - 1;
                        ind = (int)listBox1.SelectedValue;
                    }
                    else
                    {
                        listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
                        ind = (int)listBox1.SelectedValue;
                    }

                    playSong(ind);
                }
            }
        }

    }
}

namespace osu_mp3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.l_Name = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.l_Artist = new System.Windows.Forms.Label();
            this.artist = new System.Windows.Forms.Label();
            this.songListBox = new System.Windows.Forms.ListBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.buttonChangeDir = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.labelTotalSongs = new System.Windows.Forms.ToolStripLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.source = new System.Windows.Forms.Label();
            this.CheckSong = new System.Windows.Forms.Timer(this.components);
            this.b_next = new System.Windows.Forms.Button();
            this.b_previous = new System.Windows.Forms.Button();
            this.b_shuffle = new System.Windows.Forms.Button();
            this.songImage = new System.Windows.Forms.PictureBox();
            this.mediaplayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // l_Name
            // 
            this.l_Name.AutoSize = true;
            this.l_Name.BackColor = System.Drawing.Color.Transparent;
            this.l_Name.Location = new System.Drawing.Point(9, 35);
            this.l_Name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.l_Name.Name = "l_Name";
            this.l_Name.Size = new System.Drawing.Size(66, 13);
            this.l_Name.TabIndex = 0;
            this.l_Name.Text = "Song Name:";
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.BackColor = System.Drawing.Color.Transparent;
            this.name.Location = new System.Drawing.Point(82, 35);
            this.name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(0, 13);
            this.name.TabIndex = 1;
            // 
            // l_Artist
            // 
            this.l_Artist.AutoSize = true;
            this.l_Artist.BackColor = System.Drawing.Color.Transparent;
            this.l_Artist.Location = new System.Drawing.Point(9, 52);
            this.l_Artist.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.l_Artist.Name = "l_Artist";
            this.l_Artist.Size = new System.Drawing.Size(33, 13);
            this.l_Artist.TabIndex = 2;
            this.l_Artist.Text = "Artist:";
            // 
            // artist
            // 
            this.artist.AutoSize = true;
            this.artist.BackColor = System.Drawing.Color.Transparent;
            this.artist.Location = new System.Drawing.Point(82, 52);
            this.artist.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.artist.Name = "artist";
            this.artist.Size = new System.Drawing.Size(0, 13);
            this.artist.TabIndex = 3;
            // 
            // songListBox
            // 
            this.songListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.songListBox.FormattingEnabled = true;
            this.songListBox.HorizontalScrollbar = true;
            this.songListBox.Location = new System.Drawing.Point(326, 49);
            this.songListBox.Margin = new System.Windows.Forms.Padding(2);
            this.songListBox.Name = "songListBox";
            this.songListBox.Size = new System.Drawing.Size(245, 238);
            this.songListBox.TabIndex = 6;
            this.songListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.songListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick_1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOptions,
            this.buttonExport,
            this.toolStripSeparator1,
            this.progressBar,
            this.labelTotalSongs});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(579, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuOptions
            // 
            this.menuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonChangeDir,
            this.buttonRefresh});
            this.menuOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(62, 22);
            this.menuOptions.Text = "Options";
            // 
            // buttonChangeDir
            // 
            this.buttonChangeDir.Name = "buttonChangeDir";
            this.buttonChangeDir.Size = new System.Drawing.Size(166, 22);
            this.buttonChangeDir.Text = "Change Directory";
            this.buttonChangeDir.Click += new System.EventHandler(this.changeDirectoryToolStripMenuItem_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(166, 22);
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonExport.Image = ((System.Drawing.Image)(resources.GetObject("buttonExport.Image")));
            this.buttonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(121, 22);
            this.buttonExport.Text = "Export Selected Song";
            this.buttonExport.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 22);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // labelTotalSongs
            // 
            this.labelTotalSongs.Name = "labelTotalSongs";
            this.labelTotalSongs.Size = new System.Drawing.Size(0, 22);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.SelectedPath = "C:\\Program Files (x86)\\osu!\\Songs";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.searchTextBox.Location = new System.Drawing.Point(326, 25);
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(245, 20);
            this.searchTextBox.TabIndex = 14;
            this.searchTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(9, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Source:";
            // 
            // source
            // 
            this.source.AutoSize = true;
            this.source.BackColor = System.Drawing.Color.Transparent;
            this.source.Location = new System.Drawing.Point(82, 69);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(0, 13);
            this.source.TabIndex = 16;
            // 
            // CheckSong
            // 
            this.CheckSong.Tick += new System.EventHandler(this.CheckSong_Tick);
            // 
            // b_next
            // 
            this.b_next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_next.BackColor = System.Drawing.Color.Transparent;
            this.b_next.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.b_next.Location = new System.Drawing.Point(85, 304);
            this.b_next.Name = "b_next";
            this.b_next.Size = new System.Drawing.Size(19, 23);
            this.b_next.TabIndex = 18;
            this.b_next.Text = ">";
            this.b_next.UseVisualStyleBackColor = false;
            this.b_next.Click += new System.EventHandler(this.b_next_Click);
            // 
            // b_previous
            // 
            this.b_previous.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_previous.Location = new System.Drawing.Point(60, 304);
            this.b_previous.Name = "b_previous";
            this.b_previous.Size = new System.Drawing.Size(19, 23);
            this.b_previous.TabIndex = 19;
            this.b_previous.Text = "<";
            this.b_previous.UseVisualStyleBackColor = true;
            this.b_previous.Click += new System.EventHandler(this.b_previous_Click);
            // 
            // b_shuffle
            // 
            this.b_shuffle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_shuffle.BackColor = System.Drawing.Color.IndianRed;
            this.b_shuffle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_shuffle.Location = new System.Drawing.Point(221, 303);
            this.b_shuffle.Name = "b_shuffle";
            this.b_shuffle.Size = new System.Drawing.Size(51, 23);
            this.b_shuffle.TabIndex = 20;
            this.b_shuffle.Text = "Shuffle";
            this.b_shuffle.UseVisualStyleBackColor = false;
            this.b_shuffle.Click += new System.EventHandler(this.b_shuffle_Click);
            // 
            // songImage
            // 
            this.songImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.songImage.InitialImage = ((System.Drawing.Image)(resources.GetObject("songImage.InitialImage")));
            this.songImage.Location = new System.Drawing.Point(6, 86);
            this.songImage.Name = "songImage";
            this.songImage.Size = new System.Drawing.Size(315, 201);
            this.songImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.songImage.TabIndex = 21;
            this.songImage.TabStop = false;
            this.songImage.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // mediaplayer
            // 
            this.mediaplayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mediaplayer.Enabled = true;
            this.mediaplayer.Location = new System.Drawing.Point(0, 289);
            this.mediaplayer.Name = "mediaplayer";
            this.mediaplayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaplayer.OcxState")));
            this.mediaplayer.Size = new System.Drawing.Size(579, 45);
            this.mediaplayer.TabIndex = 17;
            this.mediaplayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange);
            this.mediaplayer.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.axWindowsMediaPlayer1_PreviewKeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(579, 334);
            this.Controls.Add(this.songImage);
            this.Controls.Add(this.b_shuffle);
            this.Controls.Add(this.b_previous);
            this.Controls.Add(this.b_next);
            this.Controls.Add(this.mediaplayer);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.songListBox);
            this.Controls.Add(this.source);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.artist);
            this.Controls.Add(this.l_Artist);
            this.Controls.Add(this.name);
            this.Controls.Add(this.l_Name);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(595, 373);
            this.Name = "Form1";
            this.Text = "osu!mp3";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.songImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaplayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label l_Name;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label l_Artist;
        private System.Windows.Forms.Label artist;
        private System.Windows.Forms.ListBox songListBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton menuOptions;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem buttonChangeDir;
        private System.Windows.Forms.ToolStripMenuItem buttonRefresh;
        private System.Windows.Forms.ToolStripLabel labelTotalSongs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label source;
        private AxWMPLib.AxWindowsMediaPlayer mediaplayer;
        private System.Windows.Forms.Timer CheckSong;
        private System.Windows.Forms.Button b_next;
        private System.Windows.Forms.Button b_previous;
        private System.Windows.Forms.Button b_shuffle;
        private System.Windows.Forms.PictureBox songImage;
        private System.Windows.Forms.ToolStripButton buttonExport;
    }
}


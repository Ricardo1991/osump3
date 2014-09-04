using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace osu_mp3
{
    class Song
    {
        string name = "";   //name of the song
        string artist = ""; //artist name
        string dir;         //osu song beatmap folder

        public string Folder
        {
            get { return dir; }
            set { dir = value; }
        }
        string mp3file = ""; //name of mp3 file

        public string Mp3file
        {
            get { return mp3file; }
            set { mp3file = value; }
        }
        string tags = " ";     //search tags
        string source = " "; //Song Source
        int colectionID;    //BeatMapColectonID

        public int ColectionID
        {
            get { return colectionID; }
            set { colectionID = value; }
        }
        int beatID;         //BeatMap ID

        public int beattotal { get; set; }

        public bool hasPicture { get; set; }
        public string fullname { get; set; }
        public string imagefile { get; set; } //name of the BG image
        public int index { get; set; }

        public string getTags()
        {
            return tags;
        }
        public string getSource()
        {
            return source;
        }
        public string getimagefile()
        {
            return Folder + @"\" + imagefile;
        }
        public string getmp3Dir()
        {
            return Folder + @"\"  + Mp3file;
        }
        public string getartist()
        {
            return artist;
        }
        public string getname()
        {
            return name;
        }

        bool found = false;
        List<int> sameSongBeatmaps = new List<int>() ;

        public Song(string dir, int counter)
        {
            this.dir = dir;
            index = counter;
            hasPicture = false;

            string[] filePaths = Directory.GetFiles(@dir);

            foreach (string file in filePaths)
            {
                if (System.IO.Path.GetExtension(file) == ".osu")
                {
                    if (found == false)
                    {
                        setSongInfo(file);
                        found = true;
                        beattotal++;

                        if (colectionID == 0)
                        {
                            colectionID = getFolderID();
                        
                        }

                        /**/
                        //using (StreamWriter newTask = new StreamWriter("output.txt", true))
                        //{
                        //    newTask.WriteLine("\n[" + ColectionID + "] " + artist + " - " + name);
                        //    newTask.WriteLine("\t\t* " + beatID);
                        //}
                    }
                    else
                    {
                        //addnewID(file);
                        //beattotal++;
                        /**/
                        //using (StreamWriter newTask = new StreamWriter("output.txt", true))
                        //{
                        //    newTask.WriteLine("\t\t* "+beatID);
                        //}
                    }
                    
                }
            
            }
            fullname = artist + " - " + name;
        }

        void addnewID(string file)
        { 
            string line;
            int d = 2;
            char[] delimit1 = {' '};
            char[] delimit2 = {':'};
            char[] delimit3 = {'"'};

            StreamReader sr = new StreamReader(@file);
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("BeatmapID:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    sameSongBeatmaps.Add(Convert.ToInt32(words[1]));
                    beatID = Convert.ToInt32(words[1]);
                }
            }
        }

        int getFolderID()
        {
            char[] delimit1 = { ' ' };
            char[] delimit2 = { '\\' };
            string[] guru = Folder.Split(delimit2);

            string[] numb = guru[guru.Length-1].Split(delimit1, 2);
            
            int bah;

            try {
                bah = Convert.ToInt32(numb[0]);
            }
            catch { bah = 0; }
        
            return bah;
        }

        void setSongInfo(string file)
        {
            string line;
            int d = 2;
            char[] delimit1 = {' '};
            char[] delimit2 = {':'};
            char[] delimit3 = {'"'};

            StreamReader sr = new StreamReader(@file);
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("AudioFilename: ") == true)
                {
                    string[] words = line.Split(delimit1, d);
                    Mp3file = words[1];
                }

                if (line.Contains("Title:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    name = words[1];
                }

                if (line.Contains("Artist:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    artist = words[1];
                }
                if (line.Contains("Tags:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    tags = words[1];
                } 
                if (line.Contains("Source:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    source = words[1];
                }
                if (line.Contains("0,0,") == true &&  (line.Contains(".jpg") == true || line.Contains(".png") == true) )
                { 
                    string[] words = line.Split(delimit3, 3);
                    imagefile = words[1];
                    hasPicture = true;
                }

                if (line.Contains("BeatmapSetID:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    ColectionID = Convert.ToInt32(words[1]);
                }
                if (line.Contains("BeatmapID:") == true)
                {
                    string[] words = line.Split(delimit2, d);
                    sameSongBeatmaps.Add(Convert.ToInt32(words[1]));
                    beatID = Convert.ToInt32(words[1]);
                }
                
            }
        }
    }
}

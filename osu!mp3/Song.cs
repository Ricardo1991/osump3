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
        string name = "";           //name of the song
        string artist = "";         //artist name
        string dir;                 //osu song beatmap folder

        string mp3file = "";        //name of mp3 file
        string tags = " ";          //search tags
        string source = " ";        //Song Source
        int colectionID;            //BeatMapColectonID
        int beatID;                 //BeatMap ID

        public string Folder
        {
            get { return dir; }
            set { dir = value; }
        }
        public string Mp3file
        {
            get { return mp3file; }
            set { mp3file = value; }
        }
        public int ColectionID
        {
            get { return colectionID; }
            set { colectionID = value; }
        }
        

        public int beatTotal { get; set; }

        public bool hasPicture { get; set; }
        public string fullName { get; set; }
        public string imageFile { get; set; } //name of the BG image
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
            return Folder + @"\" + imageFile;
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

        List<int> sameSongBeatmaps = new List<int>() ;

        public Song(string dir, int counter)
        {
            this.dir = dir;
            index = counter;
            hasPicture = false;
            bool found = false;

            string[] filePaths = Directory.GetFiles(@dir);

            foreach (string file in filePaths)
            {
                if (System.IO.Path.GetExtension(file) == ".osu")
                {
                    if(!found)
                    {
                        setSongInfo(file);
                        beatTotal++;

                        if (colectionID == 0)
                            colectionID = getFolderID();

                    }
                    else
                    {
                        addNewBeatMapID(file);
                    }

                }
            }
            fullName = artist + " - " + name;
        }

        void addNewBeatMapID(string file)
        { 
            string line;

            StreamReader sr = new StreamReader(@file);
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("BeatmapID:") == true)
                {
                    sameSongBeatmaps.Add(Convert.ToInt32(line.Split(new char[] { ':' }, 2)[1]));
                    sr.Close();
                    return;
                }
            }
            sr.Close();
            return;
        }

        int getFolderID()
        {
            string[] parts = Folder.Split(new char[] { '\\' });

            string[] musicFolder = parts[parts.Length - 1].Split(new char[] { ' ' }, 2);
            
            int id;

            try {
                id = Convert.ToInt32(musicFolder[0]);
            }
            catch { id = 0; }
        
            return id;
        }

        void setSongInfo(string file)
        {
            string line;
            
            StreamReader sr = new StreamReader(@file);

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("AudioFilename: ")){
                    Mp3file = line.Split(new char[] { ' ' }, 2)[1];
                }

                else if (line.Contains("Title:")){
                    name = line.Split(new char[] { ':' }, 2)[1];
                }

                else if (line.Contains("Artist:")){
                    artist = line.Split(new char[] { ':' }, 2)[1];
                }

                else if (line.Contains("Tags:")){
                    tags = line.Split(new char[] { ':' }, 2)[1];
                }

                else if (line.Contains("Source:")){
                    source = line.Split(new char[] { ':' }, 2)[1];
                }

                else if (line.Contains("0,0,") && (line.Contains(".jpg") || line.Contains(".png"))){ 
                    imageFile = line.Split(new char[] { '"' }, 3)[1];
                    hasPicture = true;
                }

                else if (line.Contains("BeatmapSetID:")){
                    ColectionID = Convert.ToInt32(line.Split(new char[] { ':' }, 2)[1]);
                }

                else if (line.Contains("BeatmapID:")){
                    beatID = Convert.ToInt32(line.Split(new char[] { ':' }, 2)[1]);
                    sameSongBeatmaps.Add(beatID);
                }
                
            }
            sr.Close();
        }
    }
}

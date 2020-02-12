using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerRenameUWP
{
    class DataPack
    {
        internal RegularExpression regex = new RegularExpression();
        internal List<FileInfo> files = new List<FileInfo>();
        internal string sortAttrib = "";
        internal bool sortRevStatus = false;

        public void addFile(string path, string name)
        {
            FileInfo file = new FileInfo(path, name);
            files.Add(file);
            updateFiles();
        }

        public void deleteFile(List<int> indexs)
        {
            // sort indexes from large -> small
            indexs.Sort();
            indexs.Reverse();
            foreach(int i in indexs)
            {
                files.Remove(files[i]);
            }
            updateFiles();
        }

        public void clearFiles()
        {
            files.Clear();
        }

        public bool updateFiles()
        {
            regex.updateExp();
            bool isReady = true;
            if (regex.blocks.Count > 0 && files.Count > 0)
            {
                foreach (var f in files)
                {
                    isReady = isReady && f.updateAttribs(regex);
                }
            }
            else
            {
                isReady = false;
            }
            sortAndMarkFiles();
            return isReady;
        }

        public void sortAndMarkFiles()
        {
            if(sortAttrib != "" && files[0].attribs.ContainsKey(sortAttrib))
            {
                files.Sort(delegate (FileInfo f1, FileInfo f2) { return String.Compare(f1.attribs[sortAttrib], f2.attribs[sortAttrib]); });
                if (sortRevStatus)
                {
                    files.Reverse();
                }
            }
            for (int i = 0; i < files.Count; i++)
            {
                files[i].index = i + 1;
            }
        }
    }

    class FileInfo
    {
        internal string path;
        internal string name;
        internal Dictionary<string, string> attribs = new Dictionary<string, string>();
        internal bool isReady = false;
        internal int index = -1;

        internal FileInfo(string path, string name)
        {
            this.path = path;
            this.name = name;
            this.attribs[""] = name;
        }

        internal bool updateAttribs(RegularExpression regex)
        {
            isReady = false;
            var updated = new Dictionary<string, string>();
            if (Regex.IsMatch(name,regex.expression))
            {
                var groups = Regex.Matches(name, regex.expression)[0].Groups;
                
                // note: group[0] is always the whole string
                for (int i = 1; i < groups.Count; i++)
                {
                    string attrib = groups[i].ToString();
                    if (regex.blocks[i - 1].process != null)
                    {
                        attrib = regex.blocks[i - 1].process(attrib);
                    }
                    updated[regex.blocks[i - 1].name] = attrib;
                }
                isReady = true;
            }
            else
            {
                updated[""] = name;   
            }
            this.attribs = updated;
            return isReady;
        }
    }
}

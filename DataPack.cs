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

        public void addFile(string path, string name)
        {
            FileInfo file = new FileInfo(path, name);
            files.Add(file);
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
        }

        public void clearFiles()
        {
            files.Clear();
        }
    }

    class FileInfo
    {
        internal string path;
        internal string name;
        internal List<string> attribs = new List<string>();
        internal bool isReady = false;

        internal FileInfo(string path, string name)
        {
            this.path = path;
            this.name = name;
        }

        internal bool updateAttribs(RegularExpression regex)
        {
            isReady = false;
            var updated = new List<string>();
            if (Regex.IsMatch(name,regex.expression))
            {
                var groups = Regex.Matches(name, regex.expression);
                for (int i = 0; i < groups.Count; i++)
                {
                    string attrib = groups[i].ToString();
                    if (regex.blocks[i].process != null)
                    {
                        attrib = regex.blocks[i].process(attrib);
                    }
                    updated.Add(attrib);
                }
                this.attribs = updated;
                isReady = true;
            }
            return isReady;
        }
    }
}

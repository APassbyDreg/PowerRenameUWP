﻿using System;
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

        public bool updateFiles()
        {
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
                return false;
            }
            return isReady;
        }
    }

    class FileInfo
    {
        internal string path;
        internal string name;
        internal Dictionary<string, string> attribs = new Dictionary<string, string>();
        internal bool isReady = false;

        internal FileInfo(string path, string name)
        {
            this.path = path;
            this.name = name;
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
                this.attribs = updated;
                isReady = true;
            }
            return isReady;
        }
    }
}

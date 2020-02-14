using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerRenameUWP
{
    delegate string Process(string s);

    class RegularExpression
    {
        internal string expression = "";
        internal ObservableCollection<RegexBlock> blocks = new ObservableCollection<RegexBlock>();

        public void updateExp()
        {
            string newexp = "";
            for (int i=0; i < blocks.Count; i++)
            {
                newexp += "(" + blocks[i].exp + ")";
            }
            expression = "^" + newexp + "$";
        }

        internal void add(string exp, string name, string comment, Process process)
        {
            blocks.Add(new RegexBlock(exp, name, comment, process));
        }

        internal void delete(int index)
        {
            blocks.Remove(blocks[index]);
        }

        internal void clear()
        {
            blocks.Clear();
        }
    }


    /*
    * one block of regex expression
         */
    internal class RegexBlock : Processes
    {
        internal string exp;
        internal string name;
        internal string comment;
        internal Process process;

        // initialize parent class
        public RegexBlock(string exp, string name, string comment, Process process) : base() 
        {
            this.exp = exp;
            this.name = name;
            this.comment = comment;
            this.process = process;
        }
    }
}

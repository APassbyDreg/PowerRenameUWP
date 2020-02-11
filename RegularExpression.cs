using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PowerRenameUWP
{
    delegate string Process(string s);

    class RegularExpression
    {
        internal string expression = "";
        internal ObservableCollection<RegexBlock> blocks = new ObservableCollection<RegexBlock>();

        private void updateExp()
        {
            string newexp = "";
            for (int i=0; i < blocks.Count; i++)
            {
                newexp += "(" + blocks[i].exp + ")";
            }     
        }

        internal void add(string exp, string name, string comment, Process process)
        {
            blocks.Add(new RegexBlock(exp, name, comment, process));
            updateExp();
        }

        internal void delete(int index)
        {
            blocks.Remove(blocks[index]);
            updateExp();
        }

        internal void edit(int index, string exp, string name, string comment, Process process)
        {
            blocks[index].exp = exp;
            blocks[index].name = name;
            blocks[index].comment = comment;
            blocks[index].process = process;
            updateExp();
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

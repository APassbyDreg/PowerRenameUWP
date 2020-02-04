using System.Collections.Generic;

namespace PowerRenameUWP
{
    class RegexExpression
    {
        public string expression = "";
        private List<RegexBlock> blocks = new List<RegexBlock>;

        public RegexExpression(string exp)
        {
            expression = exp;
        }

        internal int updateExp()
        {
            string newexp = "";
            for(int i=0; i<blocks.Count; i++)
            {
                newexp += "(" + blocks[i].exp + ")";
            }
            return 0;            
        }
    }


    /*
    * one block of regex expression
         */
    class RegexBlock
    {
        internal string exp;
        internal string name;
        internal string comment;

        public RegexBlock(string exp, string name, string comment)
        {
            this.exp = exp;
            this.name = name;
            this.comment = comment;
        }
    }
}

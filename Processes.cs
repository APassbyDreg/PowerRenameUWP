using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerRenameUWP
{
    class Processes
    {
        public static Dictionary<string, Process> actions = new Dictionary<string, Process>();

        public Processes()
        {
            actions["do nothing"] = null;
        }

        /*
         * add after progress code as follows and update constructor:
         * 
         * internal static string ActionName(string s)
         * {
         *     // do something
         *     return "some string";
         * }
         */
    }
}

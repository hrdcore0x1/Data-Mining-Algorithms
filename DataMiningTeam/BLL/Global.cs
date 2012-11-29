using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam
{
    static class GlobalClass
    {
        public static string program = String.Empty;
        public static string GlobalVar
        {
            get { return program; }
            set { program = value; }
        }
    }
}

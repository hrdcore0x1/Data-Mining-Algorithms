using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    interface i_dto
    {
        string tid
        {
            get;
            set;
        }

        List<string> items
        {
            get;
            set;
        }
    }
}

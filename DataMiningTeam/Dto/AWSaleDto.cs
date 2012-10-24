using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataMiningTeam.Dto
{
    public class AWSaleDto : DataMiningTeam.Dto.i_dto
    {
        private string _tid;  //transaction id
        private List<string> _items;

        public string tid
        {
            get { return _tid; }
            set { _tid = value; }
        }

        public List<string> items
        {
            get { return _items; }
            set { _items = value; }
        }
    }//class
}//namespace


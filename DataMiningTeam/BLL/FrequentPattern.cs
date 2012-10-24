using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class FrequentPattern
    {
        private List<string> _items;
        private int _support;

        public FrequentPattern(List<string> items, int support)
        {
            _items = items;
            _support = support;
        }

        public List<string> items
        {
            get { return _items; }
            set { _items = value; }
        }

        public int support
        {
            get { return support; }
            set { _support = value; }
        }
    }
}

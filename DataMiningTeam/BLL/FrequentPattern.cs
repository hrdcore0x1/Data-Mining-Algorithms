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

        public override string ToString()
        {
            string fp = "{";
            foreach (string i in items)
            {
                fp += i + ", ";
            }
            fp = fp.Substring(0, fp.Length - 2) + "}: " + _support;
            return fp;
        }

    }
}

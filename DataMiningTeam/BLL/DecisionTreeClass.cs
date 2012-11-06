using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class DecisionTreeClass
    {
        private string _value;
        private int _count;

        public DecisionTreeClass(string value)
        {
            _value = value;
            _count = 1;
        }

        public int count
        {
            get { return _count; }
            set { _count = value; }
        }

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void increment()
        {
            _count++;
        }
    }
}

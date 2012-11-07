using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class DecisionTreeNode
    {
        private List<DecisionTreeNode> _children;
        private int _split_index;
        private string _split_value;
        private string _decision;

        public DecisionTreeNode(int split_index, string split_value)
        {
            _children = new List<DecisionTreeNode>();
            _split_index = split_index;
            _split_value = split_value;
            _decision = string.Empty;
        }

        public List<DecisionTreeNode> children
        {
            get { return _children; }
            set { _children = value; }
        }

        public int split_index
        {
            get { return _split_index; }
            set { _split_index = value; }
        }

        public string split_value
        {
            get { return _split_value; }
            set { _split_value = value; }
        }

        public string decision
        {
            get { return _decision; }
            set { _decision = value; }
        }
    }
}

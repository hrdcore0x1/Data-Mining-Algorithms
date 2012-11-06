using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class DecisionTreeNode
    {
        List<DecisionTreeNode> _children;
        public DecisionTreeNode()
        {
            _children = new List<DecisionTreeNode>();
        }

        public List<DecisionTreeNode> children
        {
            get { return _children; }
            set { _children = value; }
        }
        

    }
}

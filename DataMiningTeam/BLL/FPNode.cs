using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class FPNode
    {
        private string _item;
        private int _support;
        private FPNode _parent;
        private List<FPNode> _children;

        public FPNode(string item, int support)
        {
            _item = item;
            _support = support;
            _parent = null;
            _children = new List<FPNode>();
        }

        public FPNode(string item, int support, FPNode parent)
        {
            _item = item;
            _support = support;
            _parent = parent;
            _children = new List<FPNode>();
        }

        public string item
        {
            get { return _item; }
            set { _item = value; }
        }

        public int support
        {
            get { return _support; }
            set { _support = value; }
        }

        public FPNode parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public List<FPNode> children
        {
            get { return _children; }
            set { _children = value; }
        }


    }
}

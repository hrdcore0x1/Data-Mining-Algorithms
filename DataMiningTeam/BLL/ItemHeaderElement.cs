using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class ItemHeaderElement
    {
        private string _itemID;
        private int _support;
        private List<FPNode> _nodeLinks;

        public ItemHeaderElement(string itemID, int support)
        {
            _nodeLinks = new List<FPNode>();
            _itemID = itemID;
            _support = support;
        }

        public ItemHeaderElement(string itemID, int support, FPNode nodeLink)
        {
            _nodeLinks = new List<FPNode>();
            _nodeLinks.Add(nodeLink);
            _itemID = itemID;
            _support = support;
        }

        public ItemHeaderElement(string itemID, int support, List<FPNode> nodeLinks)
        {
            _nodeLinks = nodeLinks;
            _itemID = itemID;
            _support = support;
        }

        public string itemID
        {
            get { return _itemID; }
            set { _itemID = value; }
        }

        public int support
        {
            get { return _support; }
            set { _support = support; }
        }

        public List<FPNode> nodeLinks
        {
            get { return _nodeLinks; }
            set { _nodeLinks = value; }
        }

        public void addNodeLink(FPNode node)
        {
            _nodeLinks.Add(node);
        }

        public override string ToString()
        {
            return _itemID + " | " + _support + " | " + _nodeLinks.Count;
        }
    }
}

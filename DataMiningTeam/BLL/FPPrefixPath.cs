using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.BLL
{
    class FPPrefixPath
    {
        private List<FPNode> _prefixpath;
        private FPNode _suffix;
        private int _support;

        public FPPrefixPath(List<FPNode> prefixpath, FPNode suffix)
        {
            _prefixpath = prefixpath;
            _suffix = suffix;
            _support = suffix.support;
        }

        public List<FPNode> prefixpath
        {
            get { return _prefixpath; }
            set { _prefixpath = value; }
        }

        public FPNode suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        public int support
        {
            get { return _support; }
            set { _support = value; }
        }

        public override string ToString()
        {
            string prefixes = string.Empty;
            foreach (FPNode f in _prefixpath)
            {
                prefixes += f.item + ", ";

            }
            if (prefixes.Length > 2) prefixes = prefixes.Substring(0, prefixes.Length - 2);
            else prefixes = "NULL";
            prefixes = "{" + prefixes + "}:" + _support;
            return "SUFFIX: " + _suffix.item + " --> " + prefixes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    public class VerticalItemSetDto : ItemSetDto, IEquatable<VerticalItemSetDto>
    {
        //Properties/Variables *****************************************
        private List<string> _tids = new List<string>();

        //Constructors *************************************************
        public VerticalItemSetDto() { }

        public VerticalItemSetDto(List<string> Itemset):base(Itemset){}

        public VerticalItemSetDto(VerticalItemSetDto Old)
        {
            this.Itemset = Old.Itemset;
            this.SetCount = Old.SetCount;
            this._tids = Old._tids;
        }//copy constructor

        public VerticalItemSetDto(List<string> Itemset, List<string> Tids, int Count)
            : base(Itemset, Count)
        {
            this._tids = Tids;
        }//full constructor

        //Methods ******************************************************
        public void Increment(string Tid)
        {
            if (!_tids.Contains(Tid))
            {
                Increment();
                _tids.Add(Tid);
            }//if
        }//Increment

        public bool Equals(VerticalItemSetDto other)
        {
            bool eq = true;

            foreach (string s in other.Itemset)
            {
                if (!this.Itemset.Contains(s))
                {
                    eq = false;
                }//if
            }//foreach

            if (this.Itemset.Count != other.Itemset.Count)
            {
                eq = false;
            }//if

            return eq;
        }//equals

        public new string toString()
        {
            string s = "{";

            foreach (string i in this.Itemset)
            {
                s = s + i + ", ";
            }//foreach

            s = s.Substring(0, s.Length - 2) + "} {";

            foreach (string t in _tids)
            {
                s = s + t + ", ";
            }//foreach

            s = s.Substring(0, s.Length - 2) + "}";

            return s;
        }//toString

        public void GetCount(List<TransactionDto> Transactions)
        {
            bool inThere = true;

            //look at each transaction and see if c = the itemset
            foreach (TransactionDto t in Transactions)
            {
                foreach (string item in this.Itemset)
                {
                    if (!t.items.Contains(item))
                    {
                        inThere = false;
                        break;
                    }//if
                }//foreach

                if (inThere)
                {
                    Increment(t.tid);
                }//if

                inThere = true;
            }//foreach
        }//GetCount

        //Get/Set ***********************************************************
        public List<string> Tids
        {
            get { return _tids; }
            set { _tids = value; }
        }
    }//class
}//namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    public class ItemSetDto:IEquatable<ItemSetDto>,IComparable<ItemSetDto>
    {
        //Properties/Variables ********************************************
        private List<string> _itemset;
        private int _setCount;

        //Constructors ****************************************************
        public ItemSetDto() { }

        public ItemSetDto(List<string> ItemSet)
        {
            _itemset = ItemSet;
            _setCount = 0;
        }//initial Itemset constructor

        public ItemSetDto(ItemSetDto Old)
        {
            this._itemset = Old._itemset;
            this._setCount = Old._setCount;
        }//copy constructor

        public ItemSetDto(List<string> ItemSet, int SetCount)
        {
            this._itemset = ItemSet;
            this._setCount = SetCount;
        }//full constructor

        //Methods *********************************************************
        public bool Equals(ItemSetDto other)
        {
            bool eq = true;

            foreach (string s in other.Itemset)
            {
                if (!this._itemset.Contains(s))
                {
                    eq = false;
                }//if
            }//foreach

            if (this._itemset.Count != other._itemset.Count)
            {
                eq = false;
            }//if

            return eq;
        }//Equals

        public int CompareTo(ItemSetDto other)
        {
            return other._setCount - this._setCount;
        }//CompareTo

        public void Increment()
        {
            _setCount++;
        }//Increment

        public string toString()
        {
            string s = "{";

            foreach (string i in _itemset)
            {
                s = s + i + "|";
            }//foreach

            s = s.Substring(0, s.Length - 1) + "} Support Count = " + _setCount;

            return s;
        }//toString

        //Gets/Sets *******************************************************
        public List<string> Itemset
        {
            get { return _itemset; }
            set { _itemset = value; }
        }

        public int SetCount
        {
            get { return _setCount; }
            set { _setCount = value; }
        }
    }//class
}//namespace

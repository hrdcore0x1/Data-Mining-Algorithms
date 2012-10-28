using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    public class ItemSetDto:IEquatable<ItemSetDto>,IComparable<ItemSetDto>
    {
        //Properties/Variables ********************************************
        private string _itemset;
        private int _setCount;

        //Constructors ****************************************************
        public ItemSetDto() { }

        public ItemSetDto(string ItemSet)
        {
            _itemset = ItemSet;
            _setCount = 0;
        }//initial Itemset constructor

        public ItemSetDto(ItemSetDto Old)
        {
            this._itemset = Old._itemset;
            this._setCount = Old._setCount;
        }//copy constructor

        public ItemSetDto(string ItemSet, int SetCount)
        {
            this._itemset = ItemSet;
            this._setCount = SetCount;
        }//full constructor

        //Methods *********************************************************
        public bool Equals(ItemSetDto other)
        {
            if (this._itemset.Equals(other._itemset))
            {
                return true;
            }//if
            else
            {
                return false;
            }//else
        }//Equals

        public int CompareTo(ItemSetDto other)
        {
            return this._setCount - other._setCount;
        }//CompareTo

        public void Increment()
        {
            _setCount++;
        }//Increment

        //Gets/Sets *******************************************************
        public string Itemset
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

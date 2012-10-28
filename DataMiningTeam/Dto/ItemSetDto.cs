using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    public class ItemSetDto
    {
        //Properties/Variables ********************************************
        private string _itemset;
        private int _setCount;

        //Constructors ****************************************************
        public ItemSetDto() { }

        public ItemSetDto(string ItemSet)
        {
            _itemset = ItemSet;
            _setCount = 1;
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

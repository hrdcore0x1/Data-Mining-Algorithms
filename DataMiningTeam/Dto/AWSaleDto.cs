using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam.Dto
{
    public class AWSaleDto
    {
        //Properties/Variables ***************************************
        private string _orderNumber;
        private List<string> _productList;

        //Constructors ***********************************************
        public AWSaleDto() { }

        public AWSaleDto(string OrderNumber, List<string> Products)
        {
            this._orderNumber = OrderNumber;
            this._productList = Products;
        }//AWSaleDto

        //Gets/Sets **************************************************
        public string OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        public List<string> ProductList
        {
            get { return _productList; }
            set { _productList = value; }
        }
    }//class
}//namespace

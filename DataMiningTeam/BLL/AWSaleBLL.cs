using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Data;
using DataMiningTeam.Dto;

namespace DataMiningTeam.BLL
{
    public class AWSaleBLL
    {
        public AWSaleDto Process(SalesItemsetsEntity Sale)
        {
            AWSaleDto ret = null;

            ret.OrderNumber = Sale.salesorder;

            string[] productArray = Sale.Products.Split('|');

            ret.ProductList = productArray.ToList();

            return ret;
        }//Process
    }//class
}//namespace

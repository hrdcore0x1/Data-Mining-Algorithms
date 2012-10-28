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
        public TransactionDto Process(SalesItemsetsEntity Sale)
        {
            TransactionDto ret = null;

            ret.OrderNumber = Sale.salesorder;

            string[] productArray = Sale.Products.Split('|');

            ret.ProductList = productArray.ToList();

            return ret;
        }//Process
    }//class
}//namespace

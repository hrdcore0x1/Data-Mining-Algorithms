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

            ret.tid = Sale.salesorder;

            string[] productArray = Sale.Products.Split('|');

            ret.items = productArray.ToList();

            return ret;
        }//Process
    }//class
}//namespace

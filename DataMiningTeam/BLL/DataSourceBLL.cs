using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using DataMiningTeam.Data;

namespace DataMiningTeam.BLL
{
    public class DataSourceBLL
    {
        //Properties/Variables *********************************************************
        //Constructors *****************************************************************
        //Methods **********************************************************************
        public List<TransactionDto> process(string DataSource)
        {
            List<TransactionDto> ret = null;

            if (DataSource.Equals("AdventureWorks"))
            {
                AWSaleBLL awsBLL = new AWSaleBLL();
                ret = new List<TransactionDto>();

                using (AdventureWorksEntities awe = new AdventureWorksEntities())
                {
                    IEnumerable<SalesItemsetsEntity> transactions = awe.SalesItemsetsEntities;

                    foreach (SalesItemsetsEntity si in transactions)
                    {
                        ret.Add(awsBLL.Process(si));
                    }//foreach
                }//using
            }//if AdventureWorks
            else if (DataSource.Equals("Book Example"))
            {
                ret = BookExBLL.getAWSaleDto();
            }
            return ret;
        }//process
    }//class
}//namespace

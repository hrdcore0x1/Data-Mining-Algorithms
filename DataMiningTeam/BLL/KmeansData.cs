using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;

namespace DataMiningTeam.BLL
{
    class KmeansData
    {

        private List<VerticalItemSetDto> GenerateInitialItemSets(List<TransactionDto> Transactions)
        {
            List<VerticalItemSetDto> ret = new List<VerticalItemSetDto>();

            foreach (TransactionDto t in Transactions)
            {
                foreach (string s in t.items)
                {
                    List<string> ts = new List<string>();
                    ts.Add(s);
                    VerticalItemSetDto temp = new VerticalItemSetDto(ts);
                    temp.Increment(t.tid);

                    if (ret.Contains(temp))
                    {
                        ret[ret.IndexOf(temp)].Increment(t.tid);
                    }//if
                    else
                    {
                        ret.Add(temp);
                    }//else
                }//foreach in productList
            }//foreach in Transactions

            return ret;
        }//GenerateInitialItemSets
    }
}

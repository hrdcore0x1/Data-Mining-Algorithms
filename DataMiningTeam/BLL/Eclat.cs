using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;

namespace DataMiningTeam.BLL
{
    public class Eclat
    {
        //Properties/Variables *********************************************************
        //Constructors *****************************************************************
        //Methods **********************************************************************
        public List<VerticalItemSetDto> Process(List<TransactionDto> Transactions, List<VerticalItemSetDto> ItemSets, double MinimumSupport)
        {
            int minSupport = (int)Math.Round((MinimumSupport / 100 * Transactions.Count), MidpointRounding.AwayFromZero);

            List<VerticalItemSetDto> ret = GetFrequentItemSets(ItemSets, Transactions, minSupport);

            return ret;
        }//Process

        private List<VerticalItemSetDto> GetFrequentItemSets(List<VerticalItemSetDto> TargetItemSets, List<TransactionDto> Transactions, int MinimumSupport)
        {
            List<VerticalItemSetDto> ret = new List<VerticalItemSetDto>();

            //generate candidates if null
            if (TargetItemSets == null)
            {
                TargetItemSets = GenerateInitialItemSets(Transactions);
            }//if
            else if (TargetItemSets.Count == 0)
            {
                return ret;
            }//else if


            //compare candidate counts to min support and eliminate failures
            TargetItemSets.Sort();

            List<VerticalItemSetDto> survivors = new List<VerticalItemSetDto>();

            for (int i = 0; i < TargetItemSets.Count; i++)
            {
                if (TargetItemSets[i].SetCount >= MinimumSupport)
                {
                    survivors.Add(TargetItemSets[i]);
                }//if
                else
                {
                    break;
                }//else
            }//for

            //add surviving candidates to ret
            ret.AddRange(survivors);

            //join the candidates to produce a new candidate set
            List<VerticalItemSetDto> joinedCandidates = join(survivors);

            //prune the new candidate set
            List<VerticalItemSetDto> newCandidates = prune(joinedCandidates, ret);

            //add the counts to the newCandidates
            foreach (VerticalItemSetDto c in newCandidates)
            {
                c.GetCount(Transactions);
            }//foreach

            //call GetFrequentItemSets with the new candidate set and add all it returns to ret
            ret.AddRange(GetFrequentItemSets(newCandidates, Transactions, MinimumSupport));

            return ret;
        }//GetFrequentItemSets

        private List<VerticalItemSetDto> prune(List<VerticalItemSetDto> joinedCandidates, List<VerticalItemSetDto> oldItems)
        {
            List<VerticalItemSetDto> ret = new List<VerticalItemSetDto>();
            bool good;

            foreach (VerticalItemSetDto isd in joinedCandidates)
            {
                good = true;
                List<string> combinations = new List<string>();

                //suggestion for generating combinations from
                // http://stackoverflow.com/questions/7802822/all-possible-combinations-of-a-list-of-values-in-c-sharp
                double count = Math.Pow(2, isd.Itemset.Count);
                for (int i = 1; i <= count - 1; i++)
                {
                    string str = Convert.ToString(i, 2).PadLeft(isd.Itemset.Count, '0');
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j] == '1')
                        {
                            combinations.Add(isd.Itemset[j]);
                        }//if
                    }//for j
                    if (combinations.Count == isd.Itemset.Count - 1)
                    {
                        VerticalItemSetDto c = new VerticalItemSetDto(combinations);
                        if (!oldItems.Contains(c))
                        {
                            good = false;
                            break;
                        }
                    }//if
                }//for i

                if (good)
                {
                    ret.Add(isd);
                }//if
            }//foreach

            return ret;
        }//prune

        private List<VerticalItemSetDto> join(List<VerticalItemSetDto> survivors)
        {
            List<VerticalItemSetDto> ret = new List<VerticalItemSetDto>();

            for (int i = 0; i < survivors.Count; i++)
            {
                for (int j = i + 1; j < survivors.Count; j++)
                {
                    for (int x = 0; x < survivors[j].Itemset.Count; x++)
                    {
                        List<string> newItems = new List<string>();
                        newItems.AddRange(survivors[i].Itemset);

                        if (!survivors[i].Itemset.Contains(survivors[j].Itemset[x]))
                        {
                            newItems.Add(survivors[j].Itemset[x]);
                        }//if

                        VerticalItemSetDto newItemset = new VerticalItemSetDto(newItems);

                        if (!ret.Contains(newItemset))
                        {
                            ret.Add(newItemset);
                        }//if
                    }//for x
                }//for j
            }//for i

            return ret;
        }//join

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
    }//class
}//namespace

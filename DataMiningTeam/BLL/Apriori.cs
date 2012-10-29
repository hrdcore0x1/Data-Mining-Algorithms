using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;

namespace DataMiningTeam.BLL
{
    public class Apriori
    {
        //Properties/Variables *********************************************************
        //Constructors *****************************************************************
        //Methods **********************************************************************
        public List<ItemSetDto> Process(List<TransactionDto> Transactions, List<ItemSetDto> ItemSets, double MinimumSupport, double MinimumConfidence)
        {
            int minSupport = (int)Math.Round((MinimumSupport / 100 * Transactions.Count),MidpointRounding.AwayFromZero);
            int minConfidence = (int)Math.Round((MinimumConfidence / 100 * Transactions.Count),MidpointRounding.AwayFromZero);

            List<ItemSetDto> ret = GetFrequentItemSets(ItemSets, Transactions, minSupport, minConfidence);

            return ret;
        }//Process

        private List<ItemSetDto> GetFrequentItemSets(List<ItemSetDto> TargetItemSets, List<TransactionDto> Transactions, int MinimumSupport, int MinimumConfidence)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();

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

            List<ItemSetDto> survivors = new List<ItemSetDto>();

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
            List<ItemSetDto> joinedCandidates = join(survivors);

            //prune the new candidate set
            List<ItemSetDto> newCandidates = prune(joinedCandidates, ret);

            //add the counts to the newCandidates
            foreach (ItemSetDto c in newCandidates)
            {
                c.SetCount = getCount(c, Transactions);
            }//foreach

            //call GetFrequentItemSets with the new candidate set and add all it returns to ret
            ret.AddRange(GetFrequentItemSets(newCandidates,Transactions,MinimumSupport,MinimumConfidence));

            return ret;
        }//GetFrequentItemSets

        private int getCount(ItemSetDto c, List<TransactionDto> Transactions)
        {
            int count = 0;
            bool inThere = true;

            //look at each transaction and see if c = the itemset
            foreach (TransactionDto t in Transactions)
            {
                foreach (string item in c.Itemset)
                {
                    if (!t.items.Contains(item))
                    {
                        inThere = false;
                        break;
                    }//if
                }//foreach

                if (inThere)
                {
                    count++;
                }//if

                inThere = true;
            }//foreach

            return count;
        }//getCount

        private List<ItemSetDto> prune(List<ItemSetDto> joinedCandidates, List<ItemSetDto> oldItems)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();
            bool good;

            foreach (ItemSetDto isd in joinedCandidates)
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
                        ItemSetDto c = new ItemSetDto(combinations);
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

        private List<ItemSetDto> join(List<ItemSetDto> survivors)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();

            for (int i = 0; i < survivors.Count; i++)
            {
                for (int j = i + 1; j < survivors.Count; j++)
                {
                    for (int x = 0; x < survivors[j].Itemset.Count; x++)
                    {
                        List<string> newItems = new List<string>();
                        newItems.AddRange(survivors[i].Itemset);
                        newItems.Add(survivors[j].Itemset[x]);

                        ItemSetDto newItemset = new ItemSetDto(newItems);

                        if (!ret.Contains(newItemset))
                        {
                            ret.Add(newItemset);
                        }//if
                    }//for x
                }//for j
            }//for i

            return ret;
        }//join

        private List<ItemSetDto> GenerateInitialItemSets(List<TransactionDto> Transactions)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();

            foreach (TransactionDto t in Transactions)
            {
                foreach (string s in t.items)
                {
                    List<string> ts = new List<string>();
                    ts.Add(s);
                    ItemSetDto temp = new ItemSetDto(ts);
                    temp.Increment();

                    if (ret.Contains(temp))
                    {
                        ret[ret.IndexOf(temp)].Increment();
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

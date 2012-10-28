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
            int minSupport = (int)(MinimumSupport / 100 * Transactions.Count);
            int minConfidence = (int)(MinimumConfidence / 100 * Transactions.Count);

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

            //call GetFrequentItemSets with the new candidate set and add all it returns to ret
            ret.AddRange(GetFrequentItemSets(newCandidates,Transactions,MinimumSupport,MinimumConfidence));

            return ret;
        }//GetFrequentItemSets

        private List<ItemSetDto> prune(List<ItemSetDto> joinedCandidates, List<ItemSetDto> oldItems)
        {
            throw new NotImplementedException();
            //split each candidate into a stringArray

        }//prune

        private List<ItemSetDto> join(List<ItemSetDto> survivors)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();
            List<string[]> itemList = new List<string[]>();

            foreach (ItemSetDto isd in survivors)
            {
                itemList.Add(isd.Itemset.Split('|'));
            }//foreach

            return ret;
        }//join

        private List<ItemSetDto> GenerateInitialItemSets(List<TransactionDto> Transactions)
        {
            List<ItemSetDto> ret = new List<ItemSetDto>();

            foreach (TransactionDto t in Transactions)
            {
                foreach (string s in t.ProductList)
                {
                    ItemSetDto temp = new ItemSetDto(s);
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

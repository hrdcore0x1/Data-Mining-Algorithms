using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
namespace DataMiningTeam.BLL
{
    class DecisionTree
    {
        DecisionTreeNode _root;
        
        public DecisionTree(List<TransactionDto> transactions, int class_index)
        {
            _root = new DecisionTreeNode(-1, String.Empty);
            train(ref _root, transactions, class_index);
        }

        public string decide(TransactionDto transaction)
        {
            return decide(transaction, _root);
        }

        public string decide(TransactionDto transaction, DecisionTreeNode node)
        {
            if (node.split_index == -1) return node.decision;

            foreach (DecisionTreeNode child in node.children)
            {
                if (transaction.items[node.split_index] == child.split_value) return decide(transaction, child);
            }

            return "NOT_FOUND";  //this case *SHOULD* never be reached
        }

        private void train(ref DecisionTreeNode root, List<TransactionDto> transactions, int class_index)
        {
            //clear children
            root.children.Clear();

            List<DecisionTreeClass> classes = new List<DecisionTreeClass>();

            foreach (TransactionDto t in transactions)
            {
                string value = t.items[class_index];
                Boolean found = false;
                foreach (DecisionTreeClass c in classes)
                {
                    if (c.value == value)
                    {
                        found = true;
                        c.increment();
                    }
                }

                if (!found)
                {
                    classes.Add(new DecisionTreeClass(value));
                }
            }
            int m = classes.Count;
            if (m == 1)
            {
                root.decision = classes[0].value;
                return;
            }
            //compute infoGain for classifier
            int totalTuples = transactions.Count();
            double infoGain = 0.0;
            for (int i = 0; i < m; i++)
            {
                infoGain -= ((double)classes[i].count / totalTuples) * Math.Log(((double)classes[i].count / totalTuples), 2);
            }


            List<double> attributeGain = new List<double>();
            //compute infoGain for attributes

            for (int i = 0; i < transactions[0].items.Count; i++)
            {
                if (i == class_index) continue;  //skip classifier

                var distinctValues = from v in transactions
                                     select v.items[i];
                distinctValues = distinctValues.Distinct().ToList();

                double ttlAttrInfoGain = 0.0;
                foreach (string d in distinctValues)
                {
                    List<int> counts = new List<int>();
                    foreach (DecisionTreeClass c in classes)
                    {
                        var distinctClassValues = from e in transactions
                                    where e.items[class_index] == c.value && e.items[i] == d
                                    select e;
                        counts.Add(distinctClassValues.Count());
                    }

                    double attrInfoGain = 0.0;
                    int sum = counts.Sum();
                    foreach (int c in counts)
                    {
                        if (c == 0) continue;
                        attrInfoGain -= ((double)c / sum) * Math.Log(((double)c / sum), 2); 
                    }
                    attrInfoGain *= (double)sum / totalTuples;
                    ttlAttrInfoGain += attrInfoGain;
                }
                attributeGain.Add(ttlAttrInfoGain);
            }  //attribute loop
            var aGain = from a in attributeGain
                        select infoGain - a;
            attributeGain = aGain.ToList();
            //pg 339, need to partition based on max(attrGain)
            
            
            int splitIndex = attributeGain.IndexOf(attributeGain.Max());
            var splitValues = from v in transactions
                              select v.items[splitIndex];
            splitValues = splitValues.Distinct().ToList();
            foreach (string split in splitValues)
            {
                var nTrans = from t in transactions
                             where t.items[splitIndex] == split
                             select t;
                nTrans = nTrans.ToList();
                root.split_index = splitIndex;
                DecisionTreeNode node = new DecisionTreeNode(-1, split);
                root.children.Add(node);
                train(ref node, (List<TransactionDto>)nTrans, class_index);
            }
        }//train
    }//class
}//namespace

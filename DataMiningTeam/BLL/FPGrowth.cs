using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;

/*
 * Author: Cory Nance
 * Date: 26 October 2012 
 * Based off of FP-Growth algorithm presented in Data Mining Concepts and Techniques 3rd edition by Jiawei Han | Micheline Kamber | Jian Pei
 * pg. 257-259
 */

namespace DataMiningTeam.BLL
{
    class FPGrowth
    {
        private List<AWSaleDto> dtos;
        private int min_sup;

        public FPGrowth(List<AWSaleDto> dtos, int min_sup)
        {
            this.dtos = dtos;
            this.min_sup = min_sup;
        }

        public List<FrequentPattern> mine()
        {
            var item = new List<string>();
            var support = new List<int>();
            int index;


            /* get support count of each item */
            foreach (AWSaleDto dto in dtos)
            {
                foreach (string p in dto.items)
                {
                    index = item.IndexOf(p);
                    if (index == -1) //not found
                    {
                        item.Add(p);
                        support.Add(1);
                    }
                    else
                    {
                        support[index] = support[index] + 1;
                    }
                }//foreach
            }//foreach




            /* sort in desc order based on support...thank you .NET 4.0!! */
            var orderedSupport = support.Zip(item, (x, y) => new { x, y }).OrderBy(pair => pair.x).ToList(); 
            support = orderedSupport.Select(pair => pair.x).ToList();
            item = orderedSupport.Select(pair => pair.y).ToList();
            support.Reverse();
            item.Reverse();
            /* we are now sorted */

            /*
            Console.WriteLine("Frequent 1-itemsets");
            for (int i = 0; i < support.Count; i++)
            {
                Console.WriteLine(item[i] + " --> " + support[i]);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */

            /* create item header table */
            List<ItemHeaderElement> itemHeaderTable = new List<ItemHeaderElement>();
            for (int i = 0; i < support.Count; i++)
            {
                if (support[i] >= min_sup)
                {
                    itemHeaderTable.Add(new ItemHeaderElement(item[i], support[i]));
                }

            }

            /*
            Console.WriteLine("Create ItemHeaderTable");
            Console.WriteLine("ItemID | Support Count | Node-Link");
            for (int i = 0; i < item.Count; i++) Console.WriteLine(item[i] + " | " + support[i] + " | NULL");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */
            

            List<string> items;
            List<int> lsupport;
            /* sort all transaction items in L order */
            foreach (AWSaleDto dto in dtos)
            {
                items = dto.items;
                lsupport = new List<int>();

                foreach (string i in items)
                {
                    lsupport.Add(support[item.IndexOf(i)]);
                }


                orderedSupport = lsupport.Zip(items, (x, y) => new { x, y }).OrderBy(pair => pair.y).Reverse().OrderBy(pair => pair.x).ToList();
                items = orderedSupport.Select(pair => pair.y).ToList();
                items.Reverse();
                dto.items = items;
            }

            /*
            Console.WriteLine("Tranactions sorted in L order");
            for (int i = 0; i < dtos.Count; i++)
            {
                string strItem = string.Empty;
                foreach (string s in dtos[i].items)
                {
                    strItem += s + ", ";
                }
                strItem = strItem.Substring(0, strItem.Length - 2);
                Console.WriteLine(dtos[i].tid + "| " + strItem);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */


            /* start building FP-Tree */
            FPNode root = new FPNode(null, 0);
            foreach (AWSaleDto dto in dtos)
            {
                addToTree(ref root, dto, ref itemHeaderTable);
            }

            /*
            Console.WriteLine("ItemHeaderTable w/node-links");
            
            foreach (ItemHeaderElement e in itemHeaderTable)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Console.WriteLine("Root branches");
            
            foreach (FPNode aNode in root.children)
            {
                Console.WriteLine("----");
                FPNode myNode = aNode;
                while (myNode != null)
                {
                    Console.WriteLine(myNode.item + " --> ");
                    if (myNode.children.Count == 0) myNode = null;
                    else myNode = myNode.children[0];
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */
            /* Finally, let's mine the tree! */

            /* start at the bottom because we want to work our way up the tree from the leafs */
            //for simplicity I will reverse the list
            itemHeaderTable.Reverse();

            /*
            Console.WriteLine("Reversed ItemHeaderTable w/node-links");
            foreach (ItemHeaderElement e in itemHeaderTable)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */


            //List to hold frequent patterns
            List<FrequentPattern> frequentPatterns = new List<FrequentPattern>();

            mineTheTree(ref itemHeaderTable, ref frequentPatterns);

            return frequentPatterns;
         
        }//mine()



        private void mineTheTree(ref List<ItemHeaderElement> itemHeaderTable, ref List<FrequentPattern> frequentPatterns)
        {
            if (itemHeaderTable.Count == 0) return;
            FPNode parent;
            FPNode suffix;
            List<FPPrefixPath> prefixPaths = new List<FPPrefixPath>();
            string suffixItem = string.Empty;
            

            foreach (FPNode node in itemHeaderTable[0].nodeLinks)
            {
                List<FPNode> prefixes = new List<FPNode>();
                suffix = node;
                suffixItem = suffix.item;
                parent = node.parent;

                if (parent.item == null) continue;   /* skip those that are the children of the root node.  i.e. I2:7 */
                
                while (parent.item != null)
                {
                    prefixes.Add(parent);
                    parent = parent.parent;
                }
                
                
                prefixPaths.Add(new FPPrefixPath(prefixes, suffix));
            }

            /*
            Console.WriteLine("PrefixPaths");
            foreach (FPPrefixPath p in prefixPaths)
            {
                Console.WriteLine(p);

            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
             */

            List<ItemHeaderElement> conditionalItemHeader = new List<ItemHeaderElement>();
            createConditionalFPTree(prefixPaths, ref conditionalItemHeader);


            mineConditionalFPTree(ref conditionalItemHeader, ref frequentPatterns, suffixItem);


            itemHeaderTable.RemoveAt(0);
            mineTheTree(ref itemHeaderTable, ref frequentPatterns);
        }


        private void mineConditionalFPTree(ref List<ItemHeaderElement> conditionalItemHeader, ref List<FrequentPattern> frequentPatterns, string suffix)
        {

            foreach (ItemHeaderElement ihe in conditionalItemHeader)
            {
                List<string> items = new List<string>();
                items.Add(suffix);
                items.Add(ihe.itemID);
                items.Reverse();
                FrequentPattern fp = new FrequentPattern(items, ihe.support);
                frequentPatterns.Add(fp);
            }


            foreach (ItemHeaderElement ihe in conditionalItemHeader)
            { 
                foreach (FPNode fpn in ihe.nodeLinks)
                {
                    List<string> items = new List<string>();
                    FPNode aFpn = fpn;
                    int support = aFpn.support;
                    while (aFpn.item != null)
                    {
                        support = min(support, aFpn.support);
                        items.Add(aFpn.item);
                        aFpn = aFpn.parent;
                    }
                    if (items.Count > 1)
                    {
                        items.Reverse();
                        items.Add(suffix);
                        FrequentPattern fp = new FrequentPattern(items, support);
                        frequentPatterns.Add(fp);
                    }
                }
            }
        }

        private int min(int a, int b)
        {
            if (a <= b) return a;
            return b;
        }

        private void createConditionalFPTree(List<FPPrefixPath> prefixPaths, ref List<ItemHeaderElement> itemHeaders)
        {
            
            foreach (FPPrefixPath p in prefixPaths)
            {   
                for (int i = 0; i < p.support; i++)
                {
                    for (int j = 0; j < p.prefixpath.Count; j++)
                    {
                        Boolean found = false;
                        for (int k=0; k<itemHeaders.Count; k++){
                            if (itemHeaders[k].itemID == p.prefixpath[j].item) {
                                found = true;
                                itemHeaders[k].support = itemHeaders[k].support + 1;
                            }

                        }
                        if (!found) itemHeaders.Add(new ItemHeaderElement(p.prefixpath[j].item, 1));

                    }
                }
            }


            /* remove the itemHeaders that don't meet min_sup */
            for (int n = 0; n < itemHeaders.Count; n++)
            {
                if (itemHeaders[n].support < min_sup) itemHeaders.RemoveAt(n);
            }


            /*
            Console.WriteLine("ConditionalHeaderTable wo/node-links");
            foreach (ItemHeaderElement e in itemHeaders)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */


            /* remove prefixpath nodes that don't meet min_sup */
            foreach (FPPrefixPath p in prefixPaths)
            {
                List<int> removeList = new List<int>();
                p.prefixpath.Reverse();
                for (int i = 0; i < p.prefixpath.Count; i++)
                {
                    bool found = false;
                    for (int j = 0; j < itemHeaders.Count; j++)
                    {
                        if (itemHeaders[j].itemID == p.prefixpath[i].item)
                        {
                            found = true;
                        }
                    }
                    if (!found) removeList.Add(i);
                }
                for (int i = 0; i < removeList.Count; i++)
                {
                    p.prefixpath.RemoveAt(removeList[i]);
                }
            }




            FPNode root = new FPNode(null, 0);
            foreach (FPPrefixPath p in prefixPaths)
            {
                AWSaleDto dto = new AWSaleDto();
                foreach (FPNode fpn in p.prefixpath)
                {
                    dto.items.Add(fpn.item);
                }
                /*
                Console.WriteLine("Adding to the tree " + p.support + " times: ");
                Console.WriteLine(dto);
                Console.ReadKey();
                */
                for (int i = 0; i < p.support; i++)
                {
                    AWSaleDto tmpDto = new AWSaleDto();
                    foreach (string item in dto.items)
                    {
                        tmpDto.items.Add(item);
                    }
                    addToTree(ref root, tmpDto, ref itemHeaders);
                }
            }




            /*
            Console.WriteLine("ConditionalHeaderTable w/node-links");
            foreach (ItemHeaderElement e in itemHeaders)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Console.WriteLine("Conditional FP-Tree children");
            foreach (FPNode aNode in root.children)
            {
                Console.WriteLine("----");
                FPNode myNode = aNode;
                while (myNode != null)
                {
                    Console.WriteLine(myNode.item + ":" + myNode.support + " --> ");
                    if (myNode.children.Count == 0) myNode = null;
                    else myNode = myNode.children[0];
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            */
        }

        /* add the items in dto to tree recursively && add node links to itemHeaderTable */
        private void addToTree(ref FPNode root, AWSaleDto dto, ref List<ItemHeaderElement> itemHeaderTable)
        {
            if (dto.items.Count == 0) return;

            FPNode parent = root;
            foreach (FPNode node in root.children)
            {
                if (node.item == dto.items[0])
                {
                    parent = node;
                }
            }

            if (parent == root)
            {
                FPNode fpn = new FPNode(dto.items[0], 1, parent);
                parent.children.Add(fpn);
                parent = fpn;
                for (int i = 0; i < itemHeaderTable.Count; i++)
                {
                    if (itemHeaderTable[i].itemID == dto.items[0]) itemHeaderTable[i].addNodeLink(fpn);
                }
            }
            else
            {
                parent.support = parent.support + 1;
            }

            dto.items.RemoveAt(0);
            addToTree(ref parent, dto, ref itemHeaderTable);
        }
    }//class
}//namespace

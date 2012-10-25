using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;


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
                    if (index == -1)
                    {
                        //not found
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
            var orderedSupport = support.Zip(item, (x, y) => new { x, y }).OrderBy(pair => pair.x).ToList();  //to remove min_sup --> .Where(pair => pair.x >= min_sup)
            support = orderedSupport.Select(pair => pair.x).ToList();
            item = orderedSupport.Select(pair => pair.y).ToList();
            support.Reverse();
            item.Reverse();
            /* we are now sorted */

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
            for (int i = 0; i < item.Count; i++) Console.WriteLine(item[i] + " | " + support[i]);
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
                orderedSupport = lsupport.Zip(items, (x, y) => new { x, y }).OrderBy(pair => pair.x).ToList();
                items = orderedSupport.Select(pair => pair.y).ToList();
                items.Reverse();
                dto.items = items;
            }

            /* start building FP-Tree */
            FPNode root = new FPNode(null, 0);

            foreach (AWSaleDto dto in dtos)
            {
                addToTree(ref root, dto, ref itemHeaderTable);
            }

            
            foreach (ItemHeaderElement e in itemHeaderTable)
            {
                Console.WriteLine(e.ToString());
            }
            

            /* Finally, let's mine the tree! */

            /* start at the bottom because we want to work our way up the tree from the leafs */
            //for simplicity I will reverse the list
            itemHeaderTable.Reverse();

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

            

            foreach (FPNode node in itemHeaderTable[0].nodeLinks)
            {
                List<FPNode> prefixes = new List<FPNode>();
                suffix = node;
                parent = node.parent;
                
                while (parent != null)
                {
                    prefixes.Add(parent);
                    parent = node.parent;
                }
                prefixPaths.Add(new FPPrefixPath(prefixes, suffix));
                List<ItemHeaderElement> conditionalItemHeader = new List<ItemHeaderElement>();
                FPNode conditionalRoot = createConditionalFPTree(prefixPaths, ref conditionalItemHeader);


            }



            itemHeaderTable.RemoveAt(0);
            mineTheTree(ref itemHeaderTable, ref frequentPatterns);
        }


        private FPNode createConditionalFPTree(List<FPPrefixPath> prefixPaths, ref List<ItemHeaderElement> itemHeaders)
        {
            FPNode root = new FPNode(null, 0);
                        foreach (FPPrefixPath p in prefixPaths)
            {   
                for (int i = 0; i < p.support; i++)
                {
                    AWSaleDto dto = new AWSaleDto();
                    dto.tid = String.Empty;
                    for (int j = 0; j < p.prefixpath.Count; j++)
                    {
                        dto.items.Add(p.prefixpath[j].item);
                    }
                    addToTree(ref root, dto, ref itemHeaders);
                }
            }
            return root;
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

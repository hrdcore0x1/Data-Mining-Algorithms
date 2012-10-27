﻿//in entities folder
namespace AprioriAlgorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Output
    {
        public IList<Rule> StrongRules { get; set; }

        public IList<string> MaximalItemSets { get; set; }

        public Dictionary<string, Dictionary<string, double>> ClosedItemSets { get; set; }

        public IndexedDictionary FrequentItems { get; set; }
    }
}
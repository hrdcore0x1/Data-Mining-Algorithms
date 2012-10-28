using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMiningTeam
{
    class Item<T>
    {
        private T item;
        private double support;
        private double confidence;

        
        public Item(T item, double support, double confidence)
        {
            this.item = item;
            this.support = support;
            this.confidence = confidence;
        }

        public T getItem()
        {
            return item;
        }

        public double getSupport()
        {
            return support;
        }

        public double getConfidence()
        {
            return confidence;
        }

        public void setItem(T item)
        {
            this.item = item;
        }

        public void setSupport(double support)
        {
            this.support = support;
        }

        public void setConfidence(double confidence)
        {
            this.confidence = confidence;
        }

        public string toString()
        {
            return item + ": " + support + " - " + confidence;
        }
    }
}

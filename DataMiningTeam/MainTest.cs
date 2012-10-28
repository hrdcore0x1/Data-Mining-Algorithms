using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.BLL;
using DataMiningTeam.Dto;
namespace DataMiningTeam
{
    class MainTest
    {
        public static void Main()
        {
            List<TransactionDto> dtos = AWSaleBLL2.getAWSaleDto();
            FPGrowth fpg = new FPGrowth(dtos, 2);
            List<FrequentPattern> fp = fpg.mine();
            Console.WriteLine("Done, found " + fp.Count + " frequent patterns!");
            foreach (FrequentPattern f in fp)
            {
                Console.WriteLine(f.ToString());
            }
            //Console.ReadKey();
            Console.Read();
        }
    }
}

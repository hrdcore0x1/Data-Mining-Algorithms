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
            List<AWSaleDto> dtos = AWSaleBLL2.getAWSaleDto();
            FPGrowth fpg = new FPGrowth(dtos, 47);
            fpg.mine();
            Console.ReadKey();
        }
    }
}

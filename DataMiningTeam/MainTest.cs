using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.BLL;
using DataMiningTeam.Dto;
using System.Windows.Forms;
using DataMiningTeam.WindowsForms;
namespace DataMiningTeam
{
    class MainTest
    {
        public static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DMForm());
            // Application.Run(new frmMain());


            List<AWSaleDto> dtos = AWSaleBLL2.getAWSaleDto();
            FPGrowth fpg = new FPGrowth(dtos, 2);
            List<FrequentPattern> fp = fpg.mine();
            Console.WriteLine("Done, found " + fp.Count + " frequent patterns!");
            foreach (FrequentPattern f in fp)
            {
                Console.WriteLine(f.ToString());
            }
            Console.ReadKey();
        }
    }
}

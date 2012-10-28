using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataMiningTeam.BLL;
using DataMiningTeam.Dto;
using DataMiningTeam.WindowsForms;
using AprioriAlgorithm;
//using Client;



namespace DataMiningTeam.WindowsForms
{
    public partial class DMForm : Form
    {
        #region Global Variables
        Dictionary<int, string> transactions = new Dictionary<int, string>();
        int m_nLastTransId = 1;
        #endregion

        public DMForm()
        {
            InitializeComponent();
        }


        //--------------------data for removal------------------------------------------------------->


        private void btn_AddItem_Click(object sender, EventArgs e)
        {

        }


        private bool ValidateInput(TextBox txtBox, bool bIsNumber)
        {
            if (txtBox.Text.Length == 0)
            {
               // errorProvider1.SetError(txtBox, "please enter value");
                return false;
            }
            else
            {
                if (bIsNumber && int.Parse(txtBox.Text) > 100)
                {
                   // errorProvider1.SetError(txtBox, "please enter value between 0 and 100");
                    return false;
                }
                else
                {
                   // errorProvider1.SetError(txtBox, "");
                    return true;
                }
            }
        }


        //------------------data for removal-------------------------------------------------------->

        private void AddTransactionButton_Click(object sender, EventArgs e)
        {
            //need to add transactions from the database and have them appear in the textbox
            //ResultsTextBox.Text = 

        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            #region validation
            if (!ValidateInput(txtSupport, true) || !ValidateInput(txtConfidence, true))
            {
                return;
            }
          
            #endregion

            IApriori Apriori = new Apriori();
            double minSupport = double.Parse(txtSupport.Text) / 100;
            double minConfidence = double.Parse(txtConfidence.Text) / 100;
            //IEnumerable<string> Items = GetItems();
            var ourput = Apriori.Solve(minSupport, minConfidence, null, transactions);


           // frmOutput1 frm1 = new frmOutput1();
            frmOutput1 objfrmOutput = new frmOutput1(ourput);
           //frmOutput1.ShowDialog();
           objfrmOutput.ShowDialog();


           /*fpgrowth
            * List<AWSaleDto> dtos = AWSaleBLL2.getAWSaleDto();
            FPGrowth fpg = new FPGrowth(dtos, 2);
            List<FrequentPattern> fp = fpg.mine();

            //write results to results text box
            ResultsTextBox.Text = "Done, found " + fp.Count + " frequent patterns!";
            foreach (FrequentPattern f in fp)
            {
                ResultsTextBox.Text = f.ToString();
            }
            //Console.ReadKey();

            Console.WriteLine("Done, found " + fp.Count + " frequent patterns!");
            foreach (FrequentPattern f in fp)
            {
                Console.WriteLine(f.ToString());
            }
            Console.ReadKey();
        
            */
           }



        private void AprioriButton_Click(object sender, EventArgs e)
        {

          
            string strTransactiondic = "abcabcabcabcabcabcabcabc";
            string strTransactionLV ="abcabcabcabcabcabcabcabc";
            ListViewItem lvi = new ListViewItem(m_nLastTransId.ToString());
            lvi.Tag = m_nLastTransId;
            lvi.SubItems.Add(strTransactionLV);
            TransactionsListView.Items.Add(lvi);
            transactions.Add(m_nLastTransId, strTransactiondic);
            m_nLastTransId++;
        }

    

      
    }
}

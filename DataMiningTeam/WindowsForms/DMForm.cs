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

namespace DataMiningTeam.WindowsForms
{
    public partial class DMForm : Form
    {
        //Properties/Variables ************************************************
        

        //Constructors ********************************************************
        public DMForm()
        {
            InitializeComponent();
            cmbSource.Items.Add("AdventureWorks");
            cmbSource.Items.Add("Book Example");
            cmbAlgorithm.Items.Add("Apriori");
            cmbAlgorithm.Items.Add("FPGrowth");
            cmbAlgorithm.Items.Add("Eclat");
        }//DMForm

        //Methods *************************************************************
        //Events **************************************************************
        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("Please select an algorithm.");
                return;
            }//if no algorithm

            string algorithm = cmbAlgorithm.SelectedItem.ToString();

            if (algorithm.Equals("Apriori"))
            {
                rtbResults.Text = "";
                List<ItemSetDto> results = new List<ItemSetDto>();
                DateTime timer = DateTime.Now;
                if (cmbSource.SelectedItem == null)
                {
                    MessageBox.Show("Please select a data source");
                    return;
                }//if

                if (txtMinSupport.Text.Equals(""))
                {
                    MessageBox.Show("Please enter a Minimum Support");
                    return;
                }//if

                double minSupport = Convert.ToDouble(txtMinSupport.Text);
                string datasource = cmbSource.SelectedItem.ToString();

                DataSourceBLL dsBLL = new DataSourceBLL();
                Apriori ap = new Apriori();

                List<TransactionDto> transactions = dsBLL.process(datasource);

                results = ap.Process(transactions, null, minSupport);

                StringBuilder sb = new StringBuilder();
                foreach (ItemSetDto isd in results)
                {
                    sb.AppendLine(isd.toString());
                }

                rtbResults.AppendText(sb.ToString() + "\nTime to Complete: " + (DateTime.Now - timer).ToString());

            }//if "Apriori"
            else if (algorithm.Equals("FPGrowth"))
            {
                
                if (txtMinSupport.Text.Equals(""))
                {
                    MessageBox.Show("Please enter a Minimum Support");
                    return;
                }//if
                double minSupport = Convert.ToDouble(txtMinSupport.Text);
                string datasource = cmbSource.SelectedItem.ToString();
                DataSourceBLL dsBLL = new DataSourceBLL();
                List<TransactionDto> dtos = dsBLL.process(datasource);
                rtbResults.Text = "";
                DateTime timer = DateTime.Now;
                FPGrowth fpg = new FPGrowth(dtos, minSupport);
                List<FrequentPattern> fp = fpg.mine();
                StringBuilder sb = new StringBuilder();
                foreach (FrequentPattern f in fp)
                {
                    sb.AppendLine(f.ToString());
                }

                rtbResults.AppendText(sb.ToString() + "\nTime to Complete: " + (DateTime.Now - timer).ToString());
            }//if "FPGrowth"
            else if (algorithm.Equals("Eclat"))
            {
                throw new NotImplementedException();
            }//if "Eclat"
        }//btnExecute_Click

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbResults.Text = "";
            txtMinConfidence.Text = "";
            txtMinSupport.Text = "";
        }//button1_Click
    }//class
}//namespace

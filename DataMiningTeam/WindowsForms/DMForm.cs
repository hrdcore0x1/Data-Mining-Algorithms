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
        List<ItemSetDto> results = new List<ItemSetDto>();

        //Constructors ********************************************************
        public DMForm()
        {
            InitializeComponent();
            cmbSource.Items.Add("AdventureWorks");
            cmbSource.Items.Add("Book Example");
        }//DMForm

        //Methods *************************************************************
        //Events **************************************************************
        private void button1_Click(object sender, EventArgs e)
        {
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

            rtbResults.AppendText(sb.ToString());
        }//button1_Click
    }//class
}//namespace

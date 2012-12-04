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
            cmbSource.Items.Add("'|' Delimited File");
            cmbSource.Items.Add("Comma Delimited File");
            cmbSource.Items.Add("Tab Delimited File");
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
                    MessageBox.Show("Please select a data source.");
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

                if (transactions == null) return;

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

                if (dtos == null) return;

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
                rtbResults.Text = "";
                List<VerticalItemSetDto> results = new List<VerticalItemSetDto>();
                DateTime timer = DateTime.Now;
                if (cmbSource.SelectedItem == null)
                {
                    MessageBox.Show("Please select a data source.");
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
                Eclat ec = new Eclat();

                List<TransactionDto> transactions = dsBLL.process(datasource);

                if (transactions == null) return;

                results = ec.Process(transactions, null, minSupport);

                StringBuilder sb = new StringBuilder();
                foreach (VerticalItemSetDto isd in results)
                {
                    sb.AppendLine(isd.toString());
                }

                rtbResults.AppendText(sb.ToString() + "\nTime to Complete: " + (DateTime.Now - timer).ToString());
            }//if "Eclat"
        }//btnExecute_Click

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbResults.Text = "";
            txtMinConfidence.Text = "";
            txtMinSupport.Text = "";
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClassificationForm cfm = new ClassificationForm(this);
            cfm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kmeansForm form = new kmeansForm();
            form.Show();
            this.Hide();



        }
        private void DMForm_Unload(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void DMForm_Load(object sender, EventArgs e)
        {

        }

        private void myToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.youtube.com/watch?v=3t8MeE8Ik4Y");
        }//button1_Click
    }//class
}//namespace

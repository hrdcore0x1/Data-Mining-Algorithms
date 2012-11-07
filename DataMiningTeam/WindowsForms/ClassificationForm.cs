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
    public partial class ClassificationForm : Form
    {
        DMForm parent;
        DecisionTree dtree;
        List<TransactionDto> trainingDtos;
        public ClassificationForm(DMForm dmf)
        {
            InitializeComponent();
            parent = dmf;
            btnTrain.Enabled = false;
            btnClassify.Enabled = false;
        }




        private void btnTraining_Click_1(object sender, EventArgs e)
        {
            if (cmbSource.SelectedItem == null)
            {
                MessageBox.Show("Please select a data source.");
                return;
            }//if

            string datasource = cmbSource.SelectedItem.ToString();
            DataSourceBLL dsBLL = new DataSourceBLL();
            trainingDtos = dsBLL.process(datasource);
            TransactionDto t1 = trainingDtos[0];

            foreach (string i in t1.items)
            {
                cmbColumn.Items.Add(i);
            }
            cmbColumn.SelectedIndex = t1.items.Count - 1;
            btnTrain.Enabled = true;

            if (DialogResult.Yes == MessageBox.Show("Does your file have headers?", "File import", MessageBoxButtons.YesNo))
            {
                trainingDtos.RemoveAt(0);
            }
            

        }
        private void ClassificationForm_Close(object sender, EventArgs e)
        {
            parent.Show();

        }
        private void ClassificationForm_Load(object sender, EventArgs e)
        {
            cmbSource.Items.Add("'|' Delimited File");
            cmbSource.Items.Add("Comma Delimited File");
            cmbSource.Items.Add("Tab Delimited File");
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            dtree = new DecisionTree(trainingDtos, cmbColumn.SelectedIndex);
            btnClassify.Enabled = true;
            cmbClassification.Items.Add("'|' Delimited File");
            cmbClassification.Items.Add("Comma Delimited File");
            cmbClassification.Items.Add("Tab Delimited File");
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            if (cmbSource.SelectedItem == null)
            {
                MessageBox.Show("Please select a data source.");
                return;
            }//if

            string datasource = cmbSource.SelectedItem.ToString();
            DataSourceBLL dsBLL = new DataSourceBLL();
            List<TransactionDto> dtos = dsBLL.process(datasource);

            if (DialogResult.Yes == MessageBox.Show("Does your file have headers?", "File import", MessageBoxButtons.YesNo))
            {
                dtos.RemoveAt(0);
            }

            foreach (TransactionDto dto in dtos)
            {
                string classification = dtree.decide(dto);
                txtResults.Text += dto.tid + ": " + classification + Environment.NewLine;
            }
        }
    }
}

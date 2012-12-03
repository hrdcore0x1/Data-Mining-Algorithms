using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataMiningTeam.BLL;
using DataMiningTeam.Dto;

//this form allows the user to enter kmeans specs
namespace DataMiningTeam.WindowsForms
{
    public partial class kmeansForm : Form
    {
        public List<TransactionDto> trainingDtos;
        public static int Kpick = 0;
        public static String Kval = String.Empty;
        public static String randomMark = String.Empty;
        private DMForm dfm;
        public kmeansForm(DMForm dfm)
        {
            this.dfm = dfm;
            InitializeComponent();
        }


        private void kmeansForm_Unload(object sender, EventArgs e)
        {
            dfm.Show();
        }

        private void kmeansForm_Load(object sender, EventArgs e)
        {
            Kpick = 3;
            Kval = "3";
            //adds new kind of file       
            this.comboBox1.Items.Add("| Delimited Points (x,y) File");
            Kpick = 3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {

            randomMark = "y";

            NewKmeans kmeans = new NewKmeans();
            StringBuilder sb =  NewKmeans.kmeanstoo();

            this.txtData.Text = sb.ToString();
            
            List<NewKmeans.XY> dsList = new List<NewKmeans.XY>();

            dsList = NewKmeans._points;

            String listds = String.Empty;
            foreach(NewKmeans.XY XY in dsList){

                listds +=  "(" + XY.XToString + "  " + XY.YToString + ") , ";

            }//end foreach

            this.dataGridView1.DataSource = dsList;
           
        }

        private void GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String intstr = String.Empty;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //kmeansForm.randomMark = "n";
            if (Kval == String.Empty)
            {
                MessageBox.Show("Please specify a K value.");
                return;
            }//end if 

            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a data source.");
                return;
            }//if

            string datasource = this.comboBox1.SelectedItem.ToString();
            DataSourceBLL dsBLL = new DataSourceBLL();
            trainingDtos = dsBLL.process(datasource);

            if (trainingDtos == null) return; 
            if (trainingDtos.Count == 0) return;
     
         
            

            if (DialogResult.Yes == MessageBox.Show("Does your file have headers?", "File import", MessageBoxButtons.YesNo))
            {
                trainingDtos.RemoveAt(0);
            }
           


            StringBuilder sb; 
              sb = NewKmeans.kmeanst(trainingDtos);
              this.dataGridView1.DataSource = NewKmeans._points;
              this.txtData.Text = sb.ToString();
          
        
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        public void setk(){
            Kpick = Int32.Parse(this.txtKset.Text);

        }
        public static int getk()
        {
            
                return Kpick;
          
        }

        private void KButton_Click(object sender, EventArgs e)
        {
            try
            {
                Kval = this.txtKset.Text;
                int kvalint = Int16.Parse(Kval);
                if (kvalint > 9)
                {
                    MessageBox.Show("Please enter a value between 1 and 9.", "Consistency");
                    return;

                }
                Kpick = Int32.Parse(Kval);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter a value for k.","K Means",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}

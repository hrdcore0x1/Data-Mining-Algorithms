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
        public static String Kval = "3";
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
            //Kpick = 3;
            //Kval = "3";
            //adds new kind of file       
            this.comboBox1.Items.Add("| Delimited Points (x,y) File");
            //Kpick = 3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            //kmeansForm.randomMark = "n";
            if (Kval == null)
            {
                MessageBox.Show("Please specify a K value.");
                return;
            }//end if 
            
            List<NewKmeans.XY> _pointsb = new List<NewKmeans.XY>();
             Random _rand = new Random();


            // Create random region of clusters
            for (int i = 0; i < 50; i++)
            {
                var x = _rand.Next(0, 100);
                var y = _rand.NextDouble() * 100;
                _pointsb.Add(new NewKmeans.XY(x, y));
            }
            for (int i = 0; i < 50; i++)
            {
                var x = _rand.Next(0, 100);
                var y = _rand.Next(0, 100);
                _pointsb.Add(new NewKmeans.XY(x, y));
            }
            for (int i = 0; i < 50; i++)
            {
                var x = _rand.Next(0, 100);
                var y = _rand.Next(0, 100);
                _pointsb.Add(new NewKmeans.XY(x, y));
            }





            Kval = this.txtKset.Text;
            int kvalint = Int16.Parse(Kval);
            if (kvalint > 9)
            {
                MessageBox.Show("Please enter a value between 1 and 9.", "Consistency");
                return;

            }
            Kpick = Int32.Parse(Kval);


            randomMark = "y";

            //NewKmeans kmeans = new NewKmeans();
            StringBuilder sb = NewKmeans.kmeanstoo(_pointsb);

            this.txtData.Text = sb.ToString();

            List<NewKmeans.XY> dsList = new List<NewKmeans.XY>();

            dsList = NewKmeans._points;

            String listds = String.Empty;
            foreach (NewKmeans.XY XYS in dsList)
            {

                listds += "(" + XYS.XToString + "  " + XYS.YToString + ") , ";

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
            if (Kval == null)
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

            foreach (TransactionDto tdto in trainingDtos)
            {
                tdto.items.Insert(0, tdto.tid);
            }
            

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
            //kmeansForm.randomMark = "n";
            if (Kval == null)
            {
                MessageBox.Show("Please specify a K value.");
                return;
            }//end if 
            
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NewKmeans.BaseClusternewKmeans.BaseBucketsLookup.Clear();
                NewKmeans.BaseClusternewKmeans.BaseDataset.Clear();
                NewKmeans.GridBaseCluster.BaseBucketsLookup.Clear();
                NewKmeans.points.Clear();

                NewKmeans.KMeans.clusterPPP.Clear();
                NewKmeans.KMeans.cluster.Clear();
                NewKmeans.KMeans.clpoints.Clear();


                NewKmeans.KMeans.clpoints.Clear();
                NewKmeans.KMeans.BaseBucketsLookup.Clear();
                NewKmeans.KMeans.BaseDataset.Clear();
                Kpick = 0;



                this.txtData.Text = "";
                this.txtKset.Text = "";
                this.dataGridView1.DataSource = String.Empty;
            }//end try
            catch
            {
                MessageBox.Show("You must run the program to reset it. Please close the application and begin again.");
                return;
            }//end catch

        }
    }
}

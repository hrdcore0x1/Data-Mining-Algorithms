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

        public kmeansForm()
        {
            InitializeComponent();
        }


        private void kmeansForm_Load(object sender, EventArgs e)
        {  
            
            //adds new kind of file       
            this.comboBox1.Items.Add("| Delimited Points (x,y) File");
            this.txtKset.Text = "3";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public static readonly Random _rand = new Random();
        public const double MinX = 10;
        public const double MinY = 10;
        public const double MaxX = 400;
        public const double MaxY = 300;
        public NewKmeans.XY XY; 
        private void btnStart_Click(object sender, EventArgs e)
        {
             List<NewKmeans.XY> _pointsb = new List<NewKmeans.XY>();
           

            // Create random region of clusters
            for (int i = 0; i < 50; i++)
            {
                var x = _rand.Next(0, 100);
                var y =  _rand.NextDouble() * 100;
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
                _pointsb.Add(new  NewKmeans.XY(x, y));
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
            foreach(NewKmeans.XY XYS in dsList){

                listds +=  "(" + XYS.XToString + "  " + XYS.YToString + ") , ";

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

            foreach (TransactionDto tdto in trainingDtos)
            {
                tdto.items.Insert(0, tdto.tid);
            }


            if (DialogResult.Yes == MessageBox.Show("Does your file have headers?", "File import", MessageBoxButtons.YesNo))
            {
                trainingDtos.RemoveAt(0);
            }



            StringBuilder sb;
            //NewKmeans newk = new NewKmeans();
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
            
            Kval = this.txtKset.Text;
            int kvalint = Int16.Parse(Kval);
            if (kvalint > 9)
            {
                MessageBox.Show("Please enter a value between 1 and 9.", "Consistency");
                return;

            }
            Kpick = Int32.Parse(Kval);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();

            DMForm dmform = new DMForm();
            dmform.ShowDialog();

            this.Close();
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            this.txtKset.Text = "3";
            this.dataGridView1.DataSource = String.Empty;
            //NewKmeans clearnk = new NewKmeans();
            this.txtData.Text = "";

            //NewKmeans.datainput2 = null;
            //NewKmeans.datainput = null;
            //NewKmeans._points.Clear();

            
            return;     
           
            


        }
    }
   
}

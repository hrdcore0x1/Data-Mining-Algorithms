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


/*amanda changing for push*/

namespace DataMiningTeam.WindowsForms
{
    public partial class kmeansForm : Form
    {
        public List<TransactionDto> trainingDtos;

        public kmeansForm()
        {
            InitializeComponent();
        }


        private void kmeansForm_Load(object sender, EventArgs e)
        {  
            
            //adds new kind of file       
            this.comboBox1.Items.Add("| Delimited Points (x,y) File");
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            NewKmeans kmeans = new NewKmeans();
            StringBuilder sb =  NewKmeans.kmeanstoo();

            this.txtData.Text = sb.ToString();
            
            List<NewKmeans.XY> dsList = new List<NewKmeans.XY>();

            dsList = NewKmeans._points;

            String listds = String.Empty;
            foreach(NewKmeans.XY XY in dsList){

                listds +=  "(" + XY.XToString + "  " + XY.YToString + ") , ";

            }//end foreach

            //this.txtData.Text = listds;

            
            this.dataGridView1.DataSource = dsList;
            this.textBox1.Text = GlobalClass.program;

        }

        private void GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String intstr = String.Empty;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            
            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a data source.");
                return;
            }//if

            string datasource = this.comboBox1.SelectedItem.ToString();
            DataSourceBLL dsBLL = new DataSourceBLL();
            trainingDtos = dsBLL.process(datasource);

         
            

            if (DialogResult.Yes == MessageBox.Show("Does your file have headers?", "File import", MessageBoxButtons.YesNo))
            {
                trainingDtos.RemoveAt(0);
            }
           



            /*NewKmeans kmeans2 = new NewKmeans();
            StringBuilder sb2 = kmeans2.kmeanst(trainingDtos);

            this.txtData.Text = sb2.ToString();
            
            List<NewKmeans.XY> dsList = new List<NewKmeans.XY>();
            
            dsList = kmeans2.Data(trainingDtos);

            String listds = String.Empty;
            foreach (NewKmeans.XY XY in dsList)
            {

                listds += "(" + XY.XToString + "  " + XY.YToString + ") , ";

            }//end foreach
            */
            

           

            //NewKmeans k = new NewKmeans();

            StringBuilder sb; 
              sb = NewKmeans.kmeanst(trainingDtos);



            /*List<NewKmeans.XY> points;
            points = NewKmeans.Data(trainingDtos);
            points = NewKmeans._points;
            String listp = String.Empty;

            foreach(NewKmeans.XY p in points){
            
                listp += "(" + p.XToString + ", " + p.YToString + ")";

                this.textBox1.Text = listp;

            }//end foreach
            */

              this.dataGridView1.DataSource = NewKmeans._points;

            //StringBuilder sb2 = NewKmeans.kmeanst(trainingDtos);

            this.txtData.Text = sb.ToString();
            this.textBox1.Text = GlobalClass.program;
        
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}

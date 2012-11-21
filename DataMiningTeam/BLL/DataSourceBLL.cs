using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using DataMiningTeam.Data;
using DataMiningTeam.WindowsForms;
using System.Windows.Forms;

namespace DataMiningTeam.BLL
{
    public class DataSourceBLL
    {   
        //Properties/Variables *********************************************************
        private string _fileName;

        //Constructors *****************************************************************
        //Methods **********************************************************************
        public List<TransactionDto> process(string DataSource)
        {
            List<TransactionDto> ret = null;

            if (DataSource.Equals("AdventureWorks"))
            {
                AWSaleBLL awsBLL = new AWSaleBLL();
                ret = new List<TransactionDto>();

                using (AdventureWorksEntities awe = new AdventureWorksEntities())
                {
                    IEnumerable<SalesItemsetsEntity> transactions = awe.SalesItemsetsEntities;

                    foreach (SalesItemsetsEntity si in transactions)
                    {
                        ret.Add(awsBLL.Process(si));
                    }//foreach
                }//using
            }//if AdventureWorks
            else if (DataSource.Equals("Book Example"))
            {
                ret = BookExBLL.getAWSaleDto();
            }// if Book Example
            else if (DataSource.Equals("'|' Delimited File") || DataSource.Equals("Comma Delimited File") || DataSource.Equals("Tab Delimited File"))
            {
                _fileName = "";
                frmFilePicker filePicker = new frmFilePicker(this);
                DialogResult flatFile = filePicker.ShowDialog();
                if (flatFile == DialogResult.OK)
                {
                    ret = FlatFileBLL.Process(DataSource, _fileName);
                }//if

            }//If flat file

                //modified to accept new files      
            else if (DataSource.Equals("| Delimited Points (x,y) File"))
            { 
                _fileName = "";
                frmFilePicker filePicker = new frmFilePicker(this);
                DialogResult flatFile = filePicker.ShowDialog();
                if (flatFile == DialogResult.OK)
                {
                    ret = FlatFileBLL.Process(DataSource, _fileName);
                }//if

            }//If flat file

            return ret;
        }//process

        //Gets/Sets ********************************************************************
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
    }//class
}//namespace

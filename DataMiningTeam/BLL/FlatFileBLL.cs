using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using System.Windows.Forms;
using System.IO;

namespace DataMiningTeam.BLL
{
    public class FlatFileBLL
    {
        //Properties/Variables ***********************************************************
        //Constructors *******************************************************************
        //Methods ************************************************************************
        public static List<TransactionDto> Process(string FileType, string FileName)
        {
            List<TransactionDto> ret = new List<TransactionDto>();

            try
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    string delimiter = "";
                    if (FileType.Equals("'|' Delimited File"))
                    {
                        delimiter = "|";
                    }
                    else if (FileType.Equals("Comma Delimited File"))
                    {
                        delimiter = ",";
                    }
                    else if (FileType.Equals("Tab Delimited File"))
                    {
                        delimiter = "\t";
                    }
                    else if (FileType.Equals("| Delimited Points (x,y) File"))
                    {
                        delimiter = "|";
                    }
                    else return null;

                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] items = line.Split(delimiter.ToCharArray());

                        if (items.Length > 1)
                        {
                            TransactionDto temp = new TransactionDto();
                            temp.tid = items[0];

                            for (int i = 1; i < items.Length; i++)
                            {
                                temp.items.Add(items[i]);
                            }//for

                            ret.Add(temp);
                        }//if at least one product
                    }//while
                }//using
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an issue with the file: " + FileName);
                return null;
            }//catch

            return ret;
        }//Process
    }//class
}//namespace

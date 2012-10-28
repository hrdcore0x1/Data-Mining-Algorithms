using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using DataMiningTeam.Data;
using System.Data;
using System.Data.SqlClient;

namespace DataMiningTeam.BLL
{
    class BookExBLL
    {
        public static List<TransactionDto> getAWSaleDto(){
            //string dbConnStr = "Data Source=hrdcore_vb;Initial Catalog=AdventureWorksDW2012;Integrated Security=True";  //CWCNANCE-DLT
            //string query = "SELECT TID as salesorder, items as Products FROM BookExample;";
            ////string query = "SELECT salesorder, Products FROM SalesDataConsolidated;";
            //string salesorder;
            //string products;
            //AWSaleDto dto;
            //SqlConnection dbConn = new SqlConnection(dbConnStr);
            //List<AWSaleDto> dtos = new List<AWSaleDto>();

            

            //dbConn.Open();           
            //SqlCommand queryCmd = new SqlCommand(query, dbConn);
            //SqlDataReader dr = queryCmd.ExecuteReader();
            //DataTable dt = new DataTable();
            //dt.Load(dr);

            

            //foreach (DataRow row in dt.Rows)
            //{
            //    salesorder = row["salesorder"].ToString();
            //    products = row["Products"].ToString();

            //    dto = new AWSaleDto();
            //    dto.tid = salesorder;
            //    dto.items = products.Split('|').ToList();
            //    dtos.Add(dto);
            //}



            //dbConn.Close();


            TransactionDto dto;
            List<TransactionDto> dtos = new List<TransactionDto>();

            using (AdventureWorksEntities awe = new AdventureWorksEntities())
            {
                IQueryable<BookExampleEntity> bee = awe.BookExampleEntities;

                foreach(BookExampleEntity be in bee)
                {
                    dto = new TransactionDto();
                    dto.tid = be.TID;
                    dto.items = be.items.Split('|').ToList();
                    dtos.Add(dto);
                }//foreach

            }//using

            return dtos;
        }

    }
}

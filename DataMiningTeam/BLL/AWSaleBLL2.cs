using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using System.Data;
using System.Data.SqlClient;

namespace DataMiningTeam.BLL
{
    class AWSaleBLL2
    {
        public static List<AWSaleDto> getAWSaleDto(){
            string dbConnStr = "Data Source=hrdcore_vb;Initial Catalog=AdventureWorksDW2012;Integrated Security=True";  //CWCNANCE-DLT
            string query = "SELECT TID as salesorder, items as Products FROM BookExample;";
            //string query = "SELECT salesorder, Products FROM SalesDataConsolidated;";
            string salesorder;
            string products;
            AWSaleDto dto;
            SqlConnection dbConn = new SqlConnection(dbConnStr);
            List<AWSaleDto> dtos = new List<AWSaleDto>();



            dbConn.Open();           
            SqlCommand queryCmd = new SqlCommand(query, dbConn);
            SqlDataReader dr = queryCmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);



            foreach (DataRow row in dt.Rows)
            {
                salesorder = row["salesorder"].ToString();
                products = row["Products"].ToString();

                dto = new AWSaleDto();
                dto.tid = salesorder;
                dto.items = products.Split('|').ToList();
                dtos.Add(dto);
            }



            dbConn.Close();
            return dtos;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DataMiningTeam
{
    class AdventureWorksSQL
    {
        //String dbConnStr = "Data Source=CWCNANCE-DLT;Initial Catalog=AdventureWorks2012;Integrated Security=True"; /* work */
        String dbConnStr = "Data Source=hrdcore_vb;Initial Catalog=AdventureWorksDW2012;Integrated Security=True"; /* home */
        SqlConnection dbLink;


        /* create dbLink and open the connection */
        public AdventureWorksSQL()
        {
            dbLink = new SqlConnection(dbConnStr);

        }

        public SqlDataReader query(string sql)
        {
            dbLink.Close();
            try
            {
                dbLink.Open();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AdventureWorksSQL class error: " + e.ToString());
            }
            SqlCommand cmd = new SqlCommand(sql, dbLink);
            try
            {

                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public void insert(string sql)
        {
            dbLink.Close();
            try
            {
                dbLink.Open();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AdventureWorksSQL class error: " + e.ToString());
            }
            SqlCommand cmd = new SqlCommand(sql, dbLink);
            try
            {

                cmd.ExecuteNonQuery();
                return;
            }
            catch (Exception e)
            {
                return;
            }

        }
        /* close connection */
        public void close()
        {
            dbLink.Close();
        }
    }

}


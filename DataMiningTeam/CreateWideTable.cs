using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DataMiningTeam
{
    class CreateWideTable
    {


//        static void Main()
//        {
//            AdventureWorksSQL aws = new AdventureWorksSQL();
//            string sql = @"drop table [AdventureWorksDW2012].[dbo].Apriori_Data;";
//            aws.query(sql);
            
//            sql = @"CREATE TABLE Apriori_Data(
//                            trans_num int,
//                            item1 int,
//                            item2 int,
//                            item3 int,
//                            item4 int,
//                            item5 int,
//                            item6 int,
//                            item7 int,                            
//                            item8 int
//                           )";



//            aws.query(sql);


//            sql = "drop function dbo.hasItem;";
//            aws.query(sql);

//            sql = @"create function dbo.hasItem(@trans_num as int, @item as int) returns bit
//                    begin
//                    declare @count int;
//                    SELECT @count = count(*) FROM AdventureWorksDW2012.dbo.Apriori_Data WHERE trans_num = @trans_num AND
//                    (@item = item1 OR @item = item2 OR @item=item3 OR @item=item4 OR @item=item5 OR @item=item6 OR @item=item7 OR @item=item8);
//                    if @count > 0 return 1;
//                    return 0;
//                    end;";
//            aws.query(sql);
            

//            int trans_id = 0;
//            sql = @"SELECT distinct CustomerKey FROM FactInternetSales;";
//            SqlDataReader dr = aws.query(sql);
//            List<int> customers = new List<int>();
//            while (dr.Read())
//            {
//                customers.Add((int)dr["CustomerKey"]);
//            }

//            foreach (int c in customers)
//            {
//                sql = @"SELECT distinct OrderDateKey FROM FactInternetSales WHERE CustomerKey = " + c + ";";
//                dr = aws.query(sql);
//                List<int> dates = new List<int>();
//                while (dr.Read())
//                {
//                    dates.Add((int)dr["OrderDateKey"]);
//                }
//                foreach (int d in dates)
//                {
//                    sql = @"SELECT ProductKey FROM FactInternetSales
//                                WHERE CustomerKey = " + c + " AND OrderDateKey = " + d + ";";

//                    dr = aws.query(sql);
//                    List<int> prodKeys = new List<int>();

//                    while (dr.Read())
//                    {
//                        prodKeys.Add((int)dr["ProductKey"]);
//                    }

//                    for (int i=prodKeys.Count; i<8; i++)
//                    {
//                        prodKeys.Add(-1);
//                    }
//                    sql = "INSERT INTO [AdventureWorksDW2012].[dbo].Apriori_Data VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});";
//                    sql = sql.Replace("{0}", string.Format("{0}", trans_id));
//                    sql = sql.Replace("{1}", string.Format("{0}", prodKeys[0]));
//                    sql = sql.Replace("{2}", string.Format("{0}", prodKeys[1]));
//                    sql = sql.Replace("{3}", string.Format("{0}", prodKeys[2]));
//                    sql = sql.Replace("{4}", string.Format("{0}", prodKeys[3]));
//                    sql = sql.Replace("{5}", string.Format("{0}", prodKeys[4]));
//                    sql = sql.Replace("{6}", string.Format("{0}", prodKeys[5]));
//                    sql = sql.Replace("{7}", string.Format("{0}", prodKeys[6]));
//                    sql = sql.Replace("{8}", string.Format("{0}", prodKeys[7]));
//                    sql = sql.Replace("-1", "null");
//                    Console.WriteLine(sql);
//                    aws.insert(sql);
//                    trans_id++;
//                }
//            }
//      }


    }
}

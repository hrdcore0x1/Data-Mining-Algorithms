using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using ats_KMean;
using ats.KMeans;


public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs z)
    {
        //double[] Student = new double[] { Compiles, Errors, Time(Minutes) };

        double[] StudentA = new double[] { 15, 32, 35.6 };
        double[] StudentB = new double[] { 19, 54, 65.1 };
        double[] StudentC = new double[] { 22, 95, 45.6 };


        ClusterCollection clusters;

        double[,] data = { { 15, 32, 35.6 }, { 19, 54, 65.1 }, {22,95,45.6 } };

        clusters = KMeans.ClusterDataSet(2, data);
        GridView1.DataSource = clusters;
        GridView1.DataBind();

        Response.Write(clusters);


        //Test the Euclidean Distance calculation between two data points
        double distance = KMeans.EuclideanDistance(StudentA, StudentB);
        Response.Write("<br/>Euclidean Distance A and B:" + distance);

        //Test the Manhattan Distance calculation between two data points
        double distances = KMeans.ManhattanDistance(StudentA, StudentB);
        Response.Write("<br/>Manhattan Distance A and B:" + distances);

        //Test the Cluster Mean calculation between two data points
        double[,] cluster = { { 15, 32, 35.6 }, { 19, 54, 65.1 } };
        double[] centroid = KMeans.ClusterMean(cluster);
        Response.Write("<br/>Cluster mean Calc: "+ centroid.ToString());
    }
}

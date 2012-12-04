using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DataMiningTeam.BLL;
using DataMiningTeam.Dto;
using DataMiningTeam.WindowsForms;
using System.Data;
using System.Windows.Forms;

/*code adapted for project from several code snippets found @ kunuk.wordpress.com
 * Amanda Fouts
 * This code accepts files in the form of cartesian points or randomly generates 
 * cartesian points for use in a kmeans algorithm that outputs a list of points
 * according to cluster
 */

namespace DataMiningTeam.BLL
{

    public class NewKmeans
    {

        //public static String program = String.Empty;
        public static List<TransactionDto> datainput;
        public static DateTime Starttime;
        public static int num = 0;

        List<XY> plist2 = new List<XY>();

        //method to generate a string of data for the form
        //this method used for file uploads
        public static StringBuilder kmeanst(List<TransactionDto> trainingDtos)
        {

            if (trainingDtos == null)
                System.Windows.Forms.MessageBox.Show("no data");
            GlobalClass.program += "1. kmeanst .. ";

            datainput = trainingDtos;
            num = 100;

            Starttime = DateTime.Now;
            var algo = new NewKmeans();

            // run kmeans or grid
            algo.Run(ClusterType.KMeans);
            //algo.Run(ClusterType.Grid);

            StringBuilder sb =
                algo.GenerateClusterGrid();

            return sb;


            //var timespend = DateTime.Now.Subtract(Starttime).TotalSeconds;

        }

        //method to generate a string of data for the form
        //this method used to generate random data
        public static StringBuilder kmeanstoo()
        {
            GlobalClass.program += "2. kmeanstoo .. ";
            datainput = null;
            Starttime = DateTime.Now;
            var algo = new NewKmeans();

            // run kmeans or grid
            algo.Run(ClusterType.KMeans);
            //algo.Run(ClusterType.Grid);

            StringBuilder sb =
                algo.GenerateClusterGrid();

            return sb;


        }


        public const bool DoUpdateAllCentroidsToNearestContainingPoint = false;
        public const bool UseProfiling = false;

        public enum ClusterType { Unknown = -1, Grid, KMeans } ;
        static readonly CultureInfo Culture_enUS = new CultureInfo("en-US");
        const string S = "G";

        //lists to contain points and clusters
        public static readonly List<XY> _points = new List<XY>();
        static List<XY> _clusters = new List<XY>();
        static List<XY> clusterPoints = new List<XY>();

        public static readonly Random _rand = new Random();
        public const double MinX = 10;
        public const double MinY = 10;
        public const double MaxX = 400;
        public const double MaxY = 300;

        // used with Grid cluster algo
        public const int Grxid = 8;
        public const int Gryid = 7;

        const double AbsSizeX = MaxX - MinX;
        const double AbsSizeY = MaxY - MinY;

        // for bucket placement calc, grid cluster algo
        const double DeltaX = AbsSizeX / Grxid;
        const double DeltaY = AbsSizeY / Gryid;

        static private readonly string NL = Environment.NewLine;
        private const string ctx = ""; //variable used to call the date time object
        public bool DisplayGridInCanvas; //autoset by used cluster type

        public NewKmeans()
        {
            GlobalClass.program += "3. NewKmeans .. ";
            //if there is no data input, generate random data
            if (NewKmeans.datainput == null)
            {

                CreateDataSet();
            }
            else
            {
                Data(datainput);
            }//end else
        }
        public static List<XY> Data(List<TransactionDto> trainingDtos)
        {
            //sets transaction data to XY list type after parsing
            GlobalClass.program += "4. Data .. ";
            datainput = trainingDtos;
            Array arr = datainput.ToArray();
            int counterz = arr.Length;

           
            foreach (TransactionDto TXY in datainput)
            {
                GlobalClass.program += "5. For each transaction in data .. ";
                Array arrz = TXY.items.ToArray();
                counterz = arrz.Length;

                for (int r = 0; r < counterz; r++)
                {
                    //parse string to set x and y variables
                    var x = TXY.items[r].ToString();
                    if (x == String.Empty) continue;
                    x = x.Substring(x.IndexOf("(") + 1, x.IndexOf(",") - 1);
                    double xdouble = Double.Parse(x);
                    var y = TXY.items[r].ToString();
                    int ednin = y.IndexOf(",");
                    y = y.Substring(ednin + 1, (y.IndexOf(")") - 1 - ednin));
                  
                    double ydouble = Double.Parse(y);
                    _points.Add(new XY(xdouble, ydouble));
                }//end for
            }//end foreach


            return _points;


        }//end data
        //used to get neighbors for the kmeans algorithm
        public static string GetId(int xid, int yid)
        {
            GlobalClass.program += "6. GetId .. ";
            return xid + ";" + yid;
        }

        public static List<XY> CreateDataSet()
        {

            GlobalClass.program += "7. CreateDataSet .. ";

            // Create random region of clusters
            for (int i = 0; i < 50; i++)
            {
                var x = MinX + _rand.NextDouble() * 100;
                var y = MinY + _rand.NextDouble() * 100;
                _points.Add(new XY(x, y));
            }
            for (int i = 0; i < 50; i++)
            {
                var x = MinX + 200 + _rand.NextDouble() * 100;
                var y = MinY + 10 + _rand.NextDouble() * 100;
                _points.Add(new XY(x, y));
            }
            for (int i = 0; i < 50; i++)
            {
                var x = MinX + 50 + _rand.NextDouble() * 100;
                var y = MinY + 150 + _rand.NextDouble() * 100;
                _points.Add(new XY(x, y));
            }
            return _points;

        }

        public void Run(ClusterType clustertype)
        {
            //set a clusters constant
            GlobalClass.program += "9. Run .. ";
            _clusters = new KMeans(_points).GetCluster();

        }

        int counterx = 0;

        //returns a string of the grid of clusters
        public StringBuilder GenerateClusterGrid()
        {
            GlobalClass.program += "10. GenerateClusterGrid .. ";
            counterx = counterx + 1;
            var sb = new StringBuilder();

            sb.Append("Total number of points   " +
                _points.Count.ToString() + NL + NL
                );

            // markers
            if (_points != null)
                foreach (var p in _points)
                {
                    sb.Append(string.Format("Point({0}, {1})",
                        ToStringEN(p.X), ToStringEN(p.Y), counterx, NL, NL));
                    sb.Append(NL);
                }

            Dictionary<string, Bucket> points = new Dictionary<string, Bucket>();

            string clusterInfo = "0";
            int cluscounter = 0;
            if (_clusters != null)
            {

                int[] clu33 = new int[100];
                int count = 0;
                //assigns clusters to an array
                XY[] arrc = _clusters.ToArray();
                List<XY> plist = new List<XY>();
                XY mmn;
                foreach (XY mm in KMeans.cluster)
                {

                    sb.Append(string.Format("Cluster - cluster point({0}, {1}, {2});{3}",
                        ToStringEN(mm.X), ToStringEN(mm.Y),
                                            mm.Size, NL));
                   
                    clu33[count] = mm.Size;
                    count++;
                    plist.Add(mm);
                    mmn = mm;

                }


                //the program draws the clusters with a centroid of radius 12
                //all points must fit in this radius to be part of the cluster
                // foreach (XY c in _points)

                int pc = _points.Count;
                List<XY> cpoints = _points;
                sb.Append(NL);

                //distance between that point and the cluster centroid 
                //must be less than 12 -- call the distance formula
                foreach (XY c in _clusters)
                {
                    sb.Append(NL + (string.Format(NL + "Cluster - cluster point({0}, {1}, {2});{3}",
                    ToStringEN(c.X), ToStringEN(c.Y),
                                        c.Size, NL)));
                   
                 
                    foreach (XY p in _points)
                    {
                        if (p.CGroup == c.CGroup)
                        {
                            sb.Append(NL + (string.Format("------- cluster point({0}, {1}, {2});",
                        ToStringEN(p.X), ToStringEN(p.Y),
                                            p.Size)));
                        }
                    }


                }
                sb.Append(NL);


                List<XY> clusterlist = KMeans.clpoints;
                List<XY> clusterPartList = null;
                int nnp = 0;
                int intx = 0;
                int inty = 0;
                int clcount = 0;

                for (int nn = 0; nn < clcount; nn++)
                {
                    nnp = nn + 1;
                    sb.Append("Beginning of cluster" + nnp.ToString() + "----------------------------------------------------------" + NL);
                    sb.Append("Cluster Points in clusters: " + NL);

                    //this line needs to read clusterlist.GetRange(0..x)then (x...y) however many clusters
                    clusterPartList = clusterlist.GetRange(intx, inty);
                    foreach (var e in clusterPartList)
                    {
                        sb.Append("(" + e.XToString + ", " + e.YToString + "), ");

                    }//end for each

                    sb.Append("End of cluster ----------------------------------------------------------" + NL);

                }//end for

                cluscounter += cluscounter;
                clusterInfo = _clusters.Count + string.Empty;

            }


            sb.Append(NL);

            sb.Append(string.Format("Clusters = ' + {0}, {1}, {2});{3}",
                clusterInfo, ToStringEN(MinX + 10), ToStringEN(MaxY + 20), NL));

            return sb;
        }
        public static string ToStringEN(double d)
        {
            GlobalClass.program += "11. ToStringEN .. ";
            double rounded = Math.Round(d, 3);
            return rounded.ToString(S, Culture_enUS);
        }
        public static void Profile(string s)
        {
            GlobalClass.program += "12. Profile .. ";
            if (!UseProfiling)
                return;

        }

        public class Segment
        {
            public Segment(NewKmeans.XY p1, NewKmeans.XY p2)
            {
                P1 = p1;
                P2 = p2;
            }

            public readonly XY P1;
            public readonly XY P2;

            public float Length()
            {
                return (float)Math.Sqrt(LengthSquared());
            }

            public double LengthSquared()
            {
                return (P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y);
            }


            /*code for brute-force taken from Rosetta code
             */
            public static List<XY> resulta = new List<XY>();
            public static Segment Closest_BruteForce(List<XY> points)
            {
                int n = points.Count;
                Segment result = Enumerable.Range(0, n - 1)
                    .SelectMany(i => Enumerable.Range(i + 1, n - (i + 1))
                        .Select(j => new Segment(points[i], points[j])))
                        .OrderBy(seg => seg.LengthSquared())
                        .First();


                resulta.Add(result.P1);
                resulta.Add(result.P2);

                return result;
            }

            public Segment MyClosestDivide(List<XY> points)
            {
                return MyClosestRec(points.OrderBy(p => p.X).ToList());
            }

            private static Segment MyClosestRec(List<XY> pointsByX)
            {
                int count = pointsByX.Count;
                if (count <= 4)
                {


                    return Segment.Closest_BruteForce(pointsByX);
                }
                // left and right lists sorted by X, as order retained from full list
                var leftByX = pointsByX.Take(count / 2).ToList();
                var leftResult = MyClosestRec(leftByX);

                var rightByX = pointsByX.Skip(count / 2).ToList();
                var rightResult = MyClosestRec(rightByX);

                var result = rightResult.Length() < leftResult.Length() ? rightResult : leftResult;

                // There may be a shorter distance that crosses the divider
                // Thus, extract all the points within result.Length either side
                var mxid = leftByX.Last().X;
                var bandWidth = result.Length();
                var inBandByX = pointsByX.Where(p => Math.Abs(mxid - p.X) <= bandWidth);

                // Sort by Y, so we can efficiently check for closer pairs
                var inBandByY = inBandByX.OrderBy(p => p.Y).ToArray();

                int iLast = inBandByY.Length - 1;
                for (int i = 0; i < iLast; i++)
                {
                    var pLower = inBandByY[i];

                    for (int j = i + 1; j <= iLast; j++)
                    {
                        var pUpper = inBandByY[j];

                        // Comparing each point to successivly increasing Y values
                        // Thus, can terminate as soon as deltaY is greater than best result
                        if ((pUpper.Y - pLower.Y) >= result.Length())
                            break;

                        if (MathTool.Distance(pLower, pUpper) < result.Length())
                            result = new Segment(pLower, pUpper);
                    }
                }

                return result;
            }

        }



        public class Cluster
        {
            public static double clusterx, clustery;
            public Cluster(double X1, double Y1)
            {
                clusterx = X1;
                clustery = Y1;

            }//end method

        }//end Cluster class


        public class XY : IComparable
        {


            //newka.program = program +  "13. XY IComparable .. ";
            public double X { get; set; }
            public double Y { get; set; }
            public string CGroup { get; set; }

            public int Size { get; set; }
            public string XToString { get { return X.ToString(S, Culture_enUS); } }
            public string YToString { get { return Y.ToString(S, Culture_enUS); } }
            public XY(double x, double y)
            {
           
                X = x;
                Y = y;
                Size = 1;
            }

            
            public static XY plist;
            public XY(XY p) //clone
            {
                plist = p;
                GlobalClass.program += "14. IComp. XY clone.. ";
                this.X = p.X;
                this.Y = p.Y;
                this.CGroup = p.CGroup;
                this.Size = p.Size;
            }

            public int CompareTo(object o) // if used in sorted list
            {
                GlobalClass.program += "15. IComp. CompareTo .. ";
                if (this.Equals(o))
                    return 0;

                var other = (XY)o;
                if (this.X > other.X)
                    return -1;
                if (this.X < other.X)
                    return 1;

                return 0;
            }
            public override int GetHashCode()
            {
                GlobalClass.program += "16. GetHashCode() .. ";
                var x = X * 10000;
                var y = Y * 10000;
                var r = x * 17 + y * 37;
                return (int)r;
            }
            private const int ROUND = 6;
            public override bool Equals(Object o)
            {
                GlobalClass.program += "17. IComp. Equals .. ";
                if (o == null)
                    return false;
                var other = o as XY;
                if (other == null)
                    return false;

                var x = Math.Round(this.X, ROUND) == Math.Round(other.X, ROUND);
                var y = Math.Round(this.Y, ROUND) == Math.Round(other.Y, ROUND);
                return x && y;
            }
        }
        public class Bucket
        {

            public string Id { get; private set; }
            public List<XY> Points { get; private set; }
            public XY Centroid { get; set; }
            public int Xid { get; private set; }
            public int Yid { get; private set; }
            public double ErrorLevel { get; set; } // clusterpoint and points avg dist
            private bool _IsUsed;
            public bool IsUsed
            {

                get { return _IsUsed && Centroid != null; }
                set { _IsUsed = value; }
            }
            public Bucket(string id)
            {
                GlobalClass.program += "19. Bucket.bucket .. ";
                IsUsed = true;
                Centroid = null;
                Points = new List<XY>();
                Id = id;
            }
            public Bucket(int xid, int yid)
            {
                GlobalClass.program += "20. Bucket.Bucket .. ";
                IsUsed = true;
                Centroid = null;
                Points = new List<XY>();
                Xid = xid;
                Yid = yid;
                Id = GetId(xid, yid);
            }
        }

        public abstract class BaseClusternewKmeans
        {
            public static List<XY> BaseDataset; // all points
            //id, bucket
            public static readonly Dictionary<string, Bucket> BaseBucketsLookup =
                new Dictionary<string, Bucket>();
            public abstract List<XY> GetCluster();
            public static int kone;
            public static XY[] kcentroids = new XY[kone];
            public static XY[] kcentroids2 = new XY[kone];

            public static XY[] BaseGetRandomCentroids(List<XY> list, int k12)
            {
                GlobalClass.program += "21. BaseClusternewKmeans.Basegetrandomcentroids .. ";
                kone = k12;
                kcentroids = new XY[kone];
                var set = new HashSet<XY>();
                int i = 0;
                

                int MAX = list.Count;
                while (MAX >= k12)
                {
                    int index = _rand.Next(0, MAX - 1);
                    var xy = list[index];
                    if (set.Contains(xy))
                        continue;

                    set.Add(xy);
                    kcentroids[i++] = new XY(xy.X, xy.Y) { };

                    if (i >= k12)
                        break;
                }

                Profile("BaseGetRandomCentroids");
                kcentroids2 = kcentroids;
                return kcentroids;
            }

            public static List<XY> BaseGetClusterResult()//O(k*n)
            {
                GlobalClass.program += "22. BaseClusternewKmeans.BAseGetClusterResult .. ";
                //var clusterPoints = new List<XY>();

             
                foreach (var item in BaseBucketsLookup)
                {
                    var bucket = item.Value;
                    if (bucket.IsUsed)
                        clusterPoints.Add(bucket.Centroid);
                }

                Profile("BaseGetClusterResult");
                return clusterPoints;
            }
            public static List<XY> clist;

            public static XY BaseGetCentroidFromCluster(List<XY> list) //O(n)
            {

                GlobalClass.program += "23. BaseClusternewKmeans.Basegetcentroidfromcluster .. ";
                int count = list.Count;
                if (list == null || count == 0)
                    return null;

                // CGroup is set for the points and the cluster point here
                var centroidgroup = GetRandomGroup(); //O(1)
                XY centroid = new XY(0, 0) { Size = list.Count };//O(1)
                foreach (XY p in list)
                {
                    p.CGroup = centroidgroup;
                    centroid.X += p.X;
                    centroid.Y += p.Y;
                }
                centroid.X /= count;
                centroid.Y /= count;
                var cp = new XY(centroid.X, centroid.Y) { Size = count, CGroup = centroidgroup };

                Profile("BaseGetCentroidFromCluster");
                return cp;
            }

            public static readonly Random Rand = new Random();
            public static int counterk = 0;
            public static string GetRandomGroup()
            {
                counterk++; 
                int r = Rand.Next(0, 1000);
               
                return string.Format("centroid group {0}{1}", r, counterk);

            }





            public static void BaseSetCentroidForAllBuckets(IEnumerable<Bucket> buckets)
            {
                GlobalClass.program += "24. BaseClusternewKmeans.BaseSetCentroidForAllBuckets .. ";
                foreach (var item in buckets)
                {
                    var bucketPoints = item.Points;
                    var cp = BaseGetCentroidFromCluster(bucketPoints);
                    item.Centroid = cp;
                }
                Profile("BaseSetCentroidForAllBuckets");
            }
            public static double BaseGetTotalError()
            {
                GlobalClass.program += "25. BaseClusternewKmeans.Basegetrandomcentroids .. ";
                int centroidsUsed = BaseBucketsLookup.Values.Count(b => b.IsUsed);
                double sum = BaseBucketsLookup.Values.Where(b => b.IsUsed).Sum(b => b.ErrorLevel);
                return sum / centroidsUsed;
            }
            public static string BaseGetMaxError()
            {
                double maxError = -double.MaxValue;
                string id = string.Empty;
                foreach (var b in BaseBucketsLookup.Values)
                {
                    if (!b.IsUsed || b.ErrorLevel <= maxError)
                        continue;

                    maxError = b.ErrorLevel;
                    id = b.Id;
                }
                return id;
            }
            public static XY BaseGetClosestPoint(XY from, List<XY> list) //O(n)
            {
                GlobalClass.program += "26. BaseClusternewKmeans.BaseGetClosestPoint .. ";
                double min = double.MaxValue;
                XY closests = null;
                foreach (var p in list)
                {
                    var d = MathTool.Distance(from, p);
                    if (d >= min)
                        continue;

                    // update
                    min = d;
                    closests = p;
                }
                return closests;
            }
            public static XY BaseGetLongestPoint(XY from, List<XY> list) //O(n)
            {
                GlobalClass.program += "27. BaseClusternewKmeans.BaeGetLongestPoint .. ";
                double max = -double.MaxValue;
                XY longest = null;
                foreach (var p in list)
                {
                    var d = MathTool.Distance(from, p);
                    if (d <= max)
                        continue;

                    // update
                    max = d;
                    longest = p;
                }
                return longest;
            }

            // update centroid location to nearest point, 
            // e.g. if you want to show cluster point on a real existing point area
            //O(n)
            public static void BaseUpdateCentroidToNearestContainingPoint(Bucket bucket)
            {
                GlobalClass.program += "28. BaseClusternewKmeans.BaseUpdateCentroidtoNearestContainingPoint .. ";
                if (bucket == null || bucket.Centroid == null ||
                    bucket.Points == null || bucket.Points.Count == 0)
                    return;

                var closest = BaseGetClosestPoint(bucket.Centroid, bucket.Points);
                bucket.Centroid.X = closest.X;
                bucket.Centroid.Y = closest.Y;
                Profile("BaseUpdateCentroidToNearestContainingPoint");
            }
            //O(k*n)
            public static void BaseUpdateAllCentroidsToNearestContainingPoint()
            {
                GlobalClass.program += "29. BaseClusternewKmeans.BaseUpdateAllCentroidstoNearestContainingPoint .. ";

                foreach (var bucket in BaseBucketsLookup.Values)
                    BaseUpdateCentroidToNearestContainingPoint(bucket);
                Profile("BaseUpdateAllCentroidsToNearestContainingPoint");
            }
        }

        // O(exponential) ~ can be slow when n or k is big
        public class KMeans : BaseClusternewKmeans
        {

            public static int InitClusterSize = kmeansForm.getk();
                
            // start from this cluster points

            // heuristic, set tolerance for cluster density, has effect on running time.
            // Set high for many points in dataset, can be lower for fewer points
            private const double MAX_ERROR = 30;

            private const int _MaxIterations = 100; // cluster point optimization iterations
           
            public static List<XY> clpoints;

            public KMeans(List<XY> dataset)
            {
                GlobalClass.program += "30. KMeans .. ";

                clpoints = dataset;
                if (dataset == null || dataset.Count == 0)
                    throw new ApplicationException(
                        string.Format("KMeans dataset is null or empty"));

                BaseDataset = dataset;
            }
            public static List<XY> cluster;
            public override List<XY> GetCluster()
            {
                GlobalClass.program += "31. getcluster .. ";

                cluster = RunClusterAlgo();
                return cluster;
            }

            public static List<XY> clusterPPP = new List<XY>();
            public static List<XY> RunClusterAlgo()
            {

                GlobalClass.program += "32. runclusteralg .. ";
                //var clusterPoints = new List<XY>();
                // params invalid
                if (BaseDataset == null || BaseDataset.Count == 0)
                {
                    clusterPPP = clusterPoints;
                    return clusterPoints;
                }//end if 

                RunAlgo();
                return BaseGetClusterResult();
            }

            public static void RunAlgo()
            {
                GlobalClass.program += "32. runalgo .. ";
                // Init clusters
                var centroids = BaseGetRandomCentroids(BaseDataset, InitClusterSize);
                for (int i = 0; i < centroids.Length; i++)
                {
                    var newbucket = new Bucket(i.ToString()) { Centroid = centroids[i] };
                    BaseBucketsLookup.Add(i.ToString(), newbucket);
                }

             
                
            }

            public static void RunIterationsUntilKClusterPlacementAreDone()
            {
                GlobalClass.program += "34. runiterationsuntilkclusterplacement are done .. ";
                double prevError = Double.MaxValue;
                double currError = Double.MaxValue;

                for (int i = 0; i < _MaxIterations; i++)
                {
                    prevError = currError;
                    currError = RunOneIteration();
                    if (currError >= prevError) // no improvement then stop
                        break;
                }
                Profile("RunIterationsUntilKClusterPlacementAreDone");
            }

            static double RunOneIteration() //O(k*n)
            {
                GlobalClass.program += "35. Runoneiteration .. ";
                // update points, assign points to cluster
                UpdatePointsByCentroid();
                // update centroid pos by its points
                BaseSetCentroidForAllBuckets(BaseBucketsLookup.Values);//O(k*n)
                var clustersCount = BaseBucketsLookup.Count;
                for (int i = 0; i < clustersCount; i++)
                {
                    var currentBucket = BaseBucketsLookup[i.ToString()];
                    if (currentBucket.IsUsed == false)
                        continue;

                    //update centroid                
                    var newcontroid = BaseGetCentroidFromCluster(currentBucket.Points);
                    //no need to update CGroup, autoset
                    currentBucket.Centroid = newcontroid;
                    currentBucket.ErrorLevel = 0;
                    //update error                
                    foreach (var p in currentBucket.Points)
                    {
                        var dist = MathTool.Distance(newcontroid, p);
                        currentBucket.ErrorLevel += dist;
                    }
                    var val = currentBucket.ErrorLevel / currentBucket.Points.Count;
                    currentBucket.ErrorLevel = val; //Math.Sqrt(val);
                }

                Profile("RunOneIteration");
                return BaseGetTotalError();
            }
            static void UpdatePointsByCentroid()//O(n*k)
            {
                GlobalClass.program += "36. Updatepointsbycentroid.. ";
                int count = BaseBucketsLookup.Count();

                // clear points in the buckets, they will be re-inserted
                foreach (var bucket in BaseBucketsLookup.Values)
                    bucket.Points.Clear();

                foreach (XY p in BaseDataset)
                {
                    double minDist = Double.MaxValue;
                    int index = -1;
                    for (int i = 0; i < count; i++)
                    {
                        var bucket = BaseBucketsLookup[i.ToString()];
                        if (bucket.IsUsed == false)
                            continue;

                        var centroid = bucket.Centroid;
                        var dist = MathTool.Distance(p, centroid);
                        if (dist < minDist)
                        {
                            // update
                            minDist = dist;
                            index = i;
                        }
                    }
                    //update CGroup for point to match centroid and re-insert
                    var closestBucket = BaseBucketsLookup[index.ToString()];
                    p.CGroup = closestBucket.Centroid.CGroup;
                    closestBucket.Points.Add(p);
                }

                Profile("UpdatePointsByCentroid");
            }
        }





        public class GridBaseCluster : BaseClusternewKmeans
        {
            public static Array neighP;
            public GridBaseCluster(List<XY> dataset)
            {
                GlobalClass.program += "38. GridBaseCluster.Gridbasecluster .. ";
                BaseDataset = dataset;
            }

            public override List<XY> GetCluster()
            {
                GlobalClass.program += "39. GridBaseCluster.GetCluster .. ";
                var cluster = RunClusterAlgo(Grxid, Gryid);
                return cluster;

            }



            public static string[] nei = new string[10000];
            void MergeClustersGrid()
            {
                GlobalClass.program += "40. GridBaseCluster.MergeClustersGrid .. ";
                foreach (var key in BaseBucketsLookup.Keys)
                {
                    var bucket = BaseBucketsLookup[key];
                    if (bucket.IsUsed == false)
                        continue;

                    var x = bucket.Xid;
                    var y = bucket.Yid;

                    // get keys for neighbors
                    var N = GetId(x, y + 1);
                    var NE = GetId(x + 1, y + 1);
                    var E = GetId(x + 1, y);
                    var SE = GetId(x + 1, y - 1);
                    var S = GetId(x, y - 1);
                    var SW = GetId(x - 1, y - 1);
                    var W = GetId(x - 1, y);
                    var NW = GetId(x - 1, y - 1);
                    var neighbors = new[] { N, NE, E, SE, S, SW, W, NW };
                    nei = neighbors;
                    MergeClustersGridHelper(key, neighbors);
                }


            }
            void MergeClustersGridHelper(string currentKey, string[] neighborKeys)
            {
                GlobalClass.program += "41. GridBaseCluster.MergeClustersGridHelper .. ";
                double minDistX = DeltaX / 2.0;//heuristic, higher value gives less merging
                double minDistY = DeltaY / 2.0;
                //if clusters in grid are too close to each other, merge them
                double minDist = MathTool.Max(minDistX, minDistY);

                foreach (var neighborKey in neighborKeys)
                {
                    if (!BaseBucketsLookup.ContainsKey(neighborKey))
                        continue;

                    var neighbor = BaseBucketsLookup[neighborKey];
                    if (neighbor.IsUsed == false)
                        continue;

                    var current = BaseBucketsLookup[currentKey];
                    var dist = MathTool.Distance(current.Centroid, neighbor.Centroid);
                    if (dist > minDist)
                        continue;

                    var centroidgroup = current.Centroid.CGroup;
                    foreach (var p in neighbor.Points)
                    {
                        //update CGroup
                        p.CGroup = centroidgroup;

                    }//end

                    neighP = neighbor.Points.ToArray();


                    current.Points.AddRange(neighbor.Points);//O(n)

                    // recalc centroid
                    var cp = BaseGetCentroidFromCluster(current.Points);
                    current.Centroid = cp;
                    neighbor.IsUsed = false; //merged, then not used anymore
                }
            }

            public List<XY> RunClusterAlgo(int grxid, int gryid)
            {
                GlobalClass.program += "43. GridBaseCluster.RunClusterAlgo .. ";
                // var clusterPoints = new List<XY>();
                // params invalid
                if (BaseDataset == null || BaseDataset.Count == 0 ||
                    grxid <= 0 || gryid <= 0)
                    return clusterPoints;

                // put points in buckets
                foreach (var p in BaseDataset)
                {
                    // find relative val
                    var relativeX = p.X - MinX;
                    var relativeY = p.Y - MinY;

                    int xid = (int)(relativeX / DeltaX);
                    int yid = (int)(relativeY / DeltaY);

                    // bucket id
                    string id = GetId(xid, yid);

                    // bucket exists, add point
                    if (BaseBucketsLookup.ContainsKey(id))
                        BaseBucketsLookup[id].Points.Add(p);

                    // new bucket, create and add point
                    else
                    {
                        Bucket bucket = new Bucket(xid, yid);
                        bucket.Points.Add(p);
                        BaseBucketsLookup.Add(id, bucket);
                    }
                }

                // calc centrod for all buckets
                BaseSetCentroidForAllBuckets(BaseBucketsLookup.Values);

                // merge if gridpoint is to close
                MergeClustersGrid();

                return BaseGetClusterResult();
            }
        }

        public class MathTool
        {

            private const double Exp = 2; // 2=euclid, 1=manhatten
            //Minkowski dist
            public static double Distance(NewKmeans.XY a, NewKmeans.XY b)
            {
                return Math.Pow(Math.Pow(Math.Abs(a.X - b.X), Exp) +
                    Math.Pow(Math.Abs(a.Y - b.Y), Exp), 1.0 / Exp);
            }

            public static double Min(double a, double b)
            {
                return a <= b ? a : b;
            }
            public static double Max(double a, double b)
            {
                return a >= b ? a : b;
            }
        }



    }

}


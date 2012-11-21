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
/*code adapted for project from several code snippets found @ kunuk.wordpress.com
 * Amanda Fouts
 * changing file for push
 * 
 * 
 */

namespace DataMiningTeam.BLL
{
    public class NewKmeans
    {

        public static List<TransactionDto> datainput;
        public static DateTime Starttime;

        public static int num = 0;

        public static StringBuilder kmeanst(List<TransactionDto> trainingDtos)
        {
            if(trainingDtos == null)
            System.Windows.Forms.MessageBox.Show("no data");
        

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


            var timespend = DateTime.Now.Subtract(Starttime).TotalSeconds;
        
        }
        public static StringBuilder kmeanstoo()
        {
            datainput = null;
            Starttime = DateTime.Now;
            var algo = new NewKmeans();

            // run kmeans or grid
            algo.Run(ClusterType.KMeans);
            //algo.Run(ClusterType.Grid);

            StringBuilder sb =
                algo.GenerateClusterGrid();

            return sb;


            var timespend = DateTime.Now.Subtract(Starttime).TotalSeconds;
            
        }

        
        public const bool DoUpdateAllCentroidsToNearestContainingPoint = false;
        public const bool UseProfiling = false; //debug, output time spend

        public enum ClusterType { Unknown = -1, Grid, KMeans } ;
        static readonly CultureInfo Culture_enUS = new CultureInfo("en-US");
        const string S = "G";




        static readonly List<XY> _points = new List<XY>();
        List<XY> _clusters = new List<XY>();

        public static readonly Random _rand = new Random();
        public const double MinX = 10;
        public const double MinY = 10;
        public const double MaxX = 400;
        public const double MaxY = 300;

        // used with Grid cluster algo
        public const int GridX = 8;
        public const int GridY = 7;

        const double AbsSizeX = MaxX - MinX;
        const double AbsSizeY = MaxY - MinY;

        // for bucket placement calc, grid cluster algo
        const double DeltaX = AbsSizeX / GridX;
        const double DeltaY = AbsSizeY / GridY;

        static private readonly string NL = Environment.NewLine;
        private const string ctx = ""; //variable used to call the date time object
        public bool DisplayGridInCanvas; //autoset by used cluster type

        public NewKmeans()
        {
            CreateDataSet();
        }
        public static List<XY> Data(List<TransactionDto> trainingDtos)
        {

            datainput = trainingDtos;

            int counterx = 0;
            int countery = 0;

            Array arr = datainput.ToArray();
            int counterz = arr.Length;


         
                   
                foreach(TransactionDto TXY in datainput){
                 
                    Array arrz = TXY.items.ToArray();
                    counterz = arrz.Length;

                    for (int r = 0; r < counterz/2; r++)
                    {


                        //should be parsing (x,y)

                        //var x = XY.ToString().Substring(1,1);
                        var x = TXY.items[r].ToString();


                        //System.Windows.Forms.MessageBox.Show(counterz.ToString());


                        x = x.Substring(1, 1);
                        double xdouble = Double.Parse(x);
                        //var y = TXY.ToString().Substring(3,1);
                        var y = TXY.items[r].ToString();
                        y = y.Substring(3, 1);
                        double ydouble = Double.Parse(y);
                        _points.Add(new XY(xdouble, ydouble));
                    }//end for
                }//end foreach
     

            return _points;


        }//end data
        public static string GetId(int idx, int idy) //O(1)
        {
            return idx + ";" + idy;
        }

        public static List<XY> CreateDataSet()
        {

            
              
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
                //this pulls null
              
               
            
         
          

                return _points;
            
        }

        public void Run(ClusterType clustertype)
        {
            
            
                    _clusters = new KMeans(_points).GetCluster();
                   
            
        }

      
        public StringBuilder GenerateClusterGrid()
        {
            var sb = new StringBuilder();

      
            // markers
            if (_points != null)
                foreach (var p in _points)
                {
                    sb.Append(string.Format("Point({0}, {1})",
                        ToStringEN(p.X), ToStringEN(p.Y), NL, NL));
                    sb.Append(NL);
                }

            string clusterInfo = "0";
            if (_clusters != null)
            {
                foreach (var c in _clusters)
                    sb.Append(string.Format("Cluster({0}, {1}, {2});{3}",
                        ToStringEN(c.X), ToStringEN(c.Y), 
                                            c.Size, NL));

                clusterInfo = _clusters.Count + string.Empty;
            }

           
            sb.Append(string.Format("Clusters = ' + {0}, {1}, {2});{3}",
                clusterInfo, ToStringEN(MinX + 10), ToStringEN(MaxY + 20), NL));

            
           
            return sb;
        }
        public static string ToStringEN(double d)
        {
            double rounded = Math.Round(d, 3);
            return rounded.ToString(S, Culture_enUS);
        }
        public static void Profile(string s)
        {
            if (!UseProfiling)
                return;
            var timespend = DateTime.Now.Subtract(Starttime).TotalSeconds;
            
        }

        public class XY : IComparable
        {
            public double X { get; set; }
            public double Y { get; set; }
            
            public int Size { get; set; }
            public string XToString { get { return X.ToString(S, Culture_enUS); } }
            public string YToString { get { return Y.ToString(S, Culture_enUS); } }
            public XY(double x, double y)
            {
                X = x;
                Y = y;
                //Color = "'rgb(0,0,0)'";//default
                Size = 1;
            }
            public XY(XY p) //clone
            {
                this.X = p.X;
                this.Y = p.Y;
                //this.Color = p.Color;
                this.Size = p.Size;
            }

            public int CompareTo(object o) // if used in sorted list
            {
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
                var x = X * 10000;
                var y = Y * 10000;
                var r = x * 17 + y * 37;
                return (int)r;
            }
            private const int ROUND = 6;
            public override bool Equals(Object o)
            {
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
            public int Idx { get; private set; }
            public int Idy { get; private set; }
            public double ErrorLevel { get; set; } // clusterpoint and points avg dist
            private bool _IsUsed;
            public bool IsUsed
            {
                get { return _IsUsed && Centroid != null; }
                set { _IsUsed = value; }
            }
            public Bucket(string id)
            {
                IsUsed = true;
                Centroid = null;
                Points = new List<XY>();
                Id = id;
            }
            public Bucket(int idx, int idy)
            {
                IsUsed = true;
                Centroid = null;
                Points = new List<XY>();
                Idx = idx;
                Idy = idy;
                Id = GetId(idx, idy);
            }
        }

        public abstract class BaseClusternewKmeans
        {
            public List<XY> BaseDataset; // all points
            //id, bucket
            public readonly Dictionary<string, Bucket> BaseBucketsLookup =
                new Dictionary<string, Bucket>();
            public abstract List<XY> GetCluster();
            //O(k?? random fn can be slow)
            public static XY[] BaseGetRandomCentroids(List<XY> list, int k)
            {
                var set = new HashSet<XY>();
                int i = 0;
                var kcentroids = new XY[k];

                int MAX = list.Count;
                while (MAX >= k)
                {
                    int index = _rand.Next(0, MAX - 1);
                    var xy = list[index];
                    if (set.Contains(xy))
                        continue;

                    set.Add(xy);
                    kcentroids[i++] = new XY(xy.X, xy.Y) { };

                    if (i >= k)
                        break;
                }

                Profile("BaseGetRandomCentroids");
                return kcentroids;
            }

            public List<XY> BaseGetClusterResult()//O(k*n)
            {
                var clusterPoints = new List<XY>();

                if (DoUpdateAllCentroidsToNearestContainingPoint)
                    BaseUpdateAllCentroidsToNearestContainingPoint();

                foreach (var item in BaseBucketsLookup)
                {
                    var bucket = item.Value;
                    if (bucket.IsUsed)
                        clusterPoints.Add(bucket.Centroid);
                }

                Profile("BaseGetClusterResult");
                return clusterPoints;
            }
            public static XY BaseGetCentroidFromCluster(List<XY> list) //O(n)
            {
                int count = list.Count;
                if (list == null || count == 0)
                    return null;

                // color is set for the points and the cluster point here
                //var color = GetRandomColor(); //O(1)
                XY centroid = new XY(0, 0) { Size = list.Count };//O(1)
                foreach (XY p in list)
                {
                    //p.Color = color;
                    centroid.X += p.X;
                    centroid.Y += p.Y;
                }
                centroid.X /= count;
                centroid.Y /= count;
                var cp = new XY(centroid.X, centroid.Y) { Size = count };

                Profile("BaseGetCentroidFromCluster");
                return cp;
            }
            //O(k*n)
            public static void BaseSetCentroidForAllBuckets(IEnumerable<Bucket> buckets)
            {
                foreach (var item in buckets)
                {
                    var bucketPoints = item.Points;
                    var cp = BaseGetCentroidFromCluster(bucketPoints);
                    item.Centroid = cp;
                }
                Profile("BaseSetCentroidForAllBuckets");
            }
            public double BaseGetTotalError()
            {
                int centroidsUsed = BaseBucketsLookup.Values.Count(b => b.IsUsed);
                double sum = BaseBucketsLookup.Values.Where(b => b.IsUsed).Sum(b => b.ErrorLevel);
                return sum / centroidsUsed;
            }
            public string BaseGetMaxError() 
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
            public XY BaseGetClosestPoint(XY from, List<XY> list) //O(n)
            {
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
            public XY BaseGetLongestPoint(XY from, List<XY> list) //O(n)
            {
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
            public void BaseUpdateCentroidToNearestContainingPoint(Bucket bucket)
            {
                if (bucket == null || bucket.Centroid == null ||
                    bucket.Points == null || bucket.Points.Count == 0)
                    return;

                var closest = BaseGetClosestPoint(bucket.Centroid, bucket.Points);
                bucket.Centroid.X = closest.X;
                bucket.Centroid.Y = closest.Y;
                Profile("BaseUpdateCentroidToNearestContainingPoint");
            }
            //O(k*n)
            public void BaseUpdateAllCentroidsToNearestContainingPoint()
            {
                foreach (var bucket in BaseBucketsLookup.Values)
                    BaseUpdateCentroidToNearestContainingPoint(bucket);
                Profile("BaseUpdateAllCentroidsToNearestContainingPoint");
            }
        }

        // O(exponential) ~ can be slow when n or k is big
        public class KMeans : BaseClusternewKmeans
        {
            private const int _InitClusterSize = 1; // start from this cluster points

            // heuristic, set tolerance for cluster density, has effect on running time.
            // Set high for many points in dataset, can be lower for fewer points
            private const double MAX_ERROR = 30;

            private const int _MaxIterations = 100; // cluster point optimization iterations
            private const int _MaxClusters = 100;

            public KMeans(List<XY> dataset)
            {
                if (dataset == null || dataset.Count == 0)
                    throw new ApplicationException(
                        string.Format("KMeans dataset is null or empty"));

                this.BaseDataset = dataset;
            }

            public override List<XY> GetCluster()
            {
                var cluster = RunClusterAlgo();
                return cluster;
            }

            List<XY> RunClusterAlgo()
            {
              

                var clusterPoints = new List<XY>();
                // params invalid
                if (BaseDataset == null || BaseDataset.Count == 0)
                    return clusterPoints;

                RunAlgo();
                return BaseGetClusterResult();
            }

            void RunAlgo()
            {
                // Init clusters
                var centroids = BaseGetRandomCentroids(BaseDataset, _InitClusterSize);
                for (int i = 0; i < centroids.Length; i++)
                {
                    var newbucket = new Bucket(i.ToString()) { Centroid = centroids[i] };
                    BaseBucketsLookup.Add(i.ToString(), newbucket);
                }

                //
                double currentMaxError = double.MaxValue;
                while (currentMaxError > MAX_ERROR && BaseBucketsLookup.Count < _MaxClusters)
                {
                    RunIterationsUntilKClusterPlacementAreDone();

                    var id = BaseGetMaxError();
                    var bucket = BaseBucketsLookup[id];
                    currentMaxError = bucket.ErrorLevel; //update
                    if (currentMaxError > MAX_ERROR)
                    {
                        var longest = BaseGetLongestPoint(bucket.Centroid, bucket.Points);
                        var newcentroid = new XY(longest);
                        var newid = BaseBucketsLookup.Count.ToString();
                        var newbucket = new Bucket(newid) { Centroid = newcentroid };
                        BaseBucketsLookup.Add(newid, newbucket);
                    }
                }
            }

            void RunIterationsUntilKClusterPlacementAreDone()
            {
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

            double RunOneIteration() //O(k*n)
            {
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
                    //no need to update color, autoset
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
            void UpdatePointsByCentroid()//O(n*k)
            {
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
                    //update color for point to match centroid and re-insert
                    var closestBucket = BaseBucketsLookup[index.ToString()];
                    //p.Color = closestBucket.Centroid.Color;
                    closestBucket.Points.Add(p);
                }

                Profile("UpdatePointsByCentroid");
            }
        }

        public class GridBaseCluster : BaseClusternewKmeans
        {
            public GridBaseCluster(List<XY> dataset)
            {
                BaseDataset = dataset;
            }

            public override List<XY> GetCluster()
            {
                var cluster = RunClusterAlgo(GridX, GridY);
                return cluster;
            }

            // O(k*n)
            void MergeClustersGrid()
            {
                foreach (var key in BaseBucketsLookup.Keys)
                {
                    var bucket = BaseBucketsLookup[key];
                    if (bucket.IsUsed == false)
                        continue;

                    var x = bucket.Idx;
                    var y = bucket.Idy;

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

                    MergeClustersGridHelper(key, neighbors);
                }
            }
            void MergeClustersGridHelper(string currentKey, string[] neighborKeys)
            {
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

                   

                    current.Points.AddRange(neighbor.Points);//O(n)

                    // recalc centroid
                    var cp = BaseGetCentroidFromCluster(current.Points);
                    current.Centroid = cp;
                    neighbor.IsUsed = false; //merged, then not used anymore
                }
            }

            public List<XY> RunClusterAlgo(int gridx, int gridy)
            {
                var clusterPoints = new List<XY>();
                // params invalid
                if (BaseDataset == null || BaseDataset.Count == 0 ||
                    gridx <= 0 || gridy <= 0)
                    return clusterPoints;

                // put points in buckets
                foreach (var p in BaseDataset)
                {
                    // find relative val
                    var relativeX = p.X - MinX;
                    var relativeY = p.Y - MinY;

                    int idx = (int)(relativeX / DeltaX);
                    int idy = (int)(relativeY / DeltaY);

                    // bucket id
                    string id = GetId(idx, idy);

                    // bucket exists, add point
                    if (BaseBucketsLookup.ContainsKey(id))
                        BaseBucketsLookup[id].Points.Add(p);

                    // new bucket, create and add point
                    else
                    {
                        Bucket bucket = new Bucket(idx, idy);
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

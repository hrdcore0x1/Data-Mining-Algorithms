using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DataMiningTeam.BLL
{
 public class KmeansCluster
        {
            public static DateTime Starttime;
       

            //------------------------------
            // USER CONFIG, CUSTOMIZE BELOW
            public const ClusterType clusterType = ClusterType.MarkerClusterer;
            public static readonly int N = 100; //number of points

            // use centroid or semi-centroid cluster point placement visualization?
            public const bool DoUpdateAllCentroidsToNearestContainingPoint = false;
            public const bool UseProfiling = false; //debug, output time spend

            // window frame boundary, aka viewport
            public const double MinX = 10;
            public const double MinY = 10;
            public const double MaxX = 400;
            public const double MaxY = 300;
            // where the files are saved or loaded
            public const string FolderPath = @"c:\temp\";

            // K-MEANS config
            // heuristic, set tolerance for cluster density, has effect on running time.
            // Set high for many points in dataset, can be lower for fewer points
            public const double MAX_ERROR = 50;

            // MARKERCLUSTERER config
            // box area i.e. cluster size
            public const int MARKERCLUSTERER_SIZE = 100;

            // DISTANCECLUSTERER config
            // radius size i.e. cluster size
            public const int DISTANCECLUSTERER_SIZE = 62;

            // GRIDCLUSTER config
            // grid size
            public const int GridX = 7;
            public const int GridY = 6;
            public const bool DoMergeGridIfCentroidsAreCloseToEachOther = true;
            // USER CONFIG, CUSTOMIZE ABOVE
            //------------------------------



            public enum ClusterType { Unknown = -1, Grid, KMeans, MarkerClusterer, DistanceClusterer } ;
            static readonly CultureInfo Culture_enUS = new CultureInfo("en-US");
            const string S = "G";

            readonly List<XY> _points = new List<XY>();
            List<XY> _clusters = new List<XY>();

            public static readonly Random Rand = new Random();

            const double AbsSizeX = MaxX - MinX;
            const double AbsSizeY = MaxY - MinY;

            // for bucket placement calc, grid cluster algo
            const double DeltaX = AbsSizeX / GridX;
            const double DeltaY = AbsSizeY / GridY;

            static private readonly string NL = Environment.NewLine;
            private const string ctx = "ctx"; // javascript canvas
            public bool DisplayGridInCanvas = false; //autoset by used cluster type

            public KmeansCluster()
            {
                CreateTestDataSet(N); // create points in memory
                //SaveDataSetToFile(_points); // save test points to file
                //_points = LoadDataSetFromFile(); // load test points from file            
            }

            // dictionary lookup key used by grid cluster algo
            public static string GetId(int idx, int idy) //O(1)
            {
                return idx + ";" + idy;
            }

            // save points from mem to file
            private const string DatasetSerializeName = "dataset.ser";
            static void SaveDataSetToFile(List<XY> dataset)
            {
                var objectToSerialize = new DatasetToSerialize { Dataset = dataset };
                new Serializer().SerializeObject(FolderPath + DatasetSerializeName, objectToSerialize);
            }
            // load points from file to mem
            static List<XY> LoadDataSetFromFile()
            {
                var objectToSerialize = (DatasetToSerialize)
                    (new Serializer().DeSerializeObject(FolderPath + DatasetSerializeName));
                return objectToSerialize.Dataset;
            }

            void CreateTestDataSet(int n) //O(n)
            {
                // CREATE DATA SET

                // Create random scattered points
                //for (int i = 0; i < n; i++)
                //{
                //    var x = MinX + Rand.NextDouble() * (AbsSizeX);
                //    var y = MinY + Rand.NextDouble() * (AbsSizeY);
                //    _points.Add(new XY(x, y));
                //}


                // Create random region of clusters
                for (int i = 0; i < n / 3; i++)
                {
                    var x = MinX + 50 + Rand.NextDouble() * 100;
                    var y = MinY + 50 + Rand.NextDouble() * 100;
                    _points.Add(new XY(x, y));
                }

                for (int i = 0; i < n / 3; i++)
                {
                    var x = MinX + 260 + Rand.NextDouble() * 100;
                    var y = MinY + 110 + Rand.NextDouble() * 100;
                    _points.Add(new XY(x, y));
                }

                for (int i = 0; i < n / 3; i++)
                {
                    var x = MinX + 100 + Rand.NextDouble() * 100;
                    var y = MinY + 200 + Rand.NextDouble() * 100;
                    _points.Add(new XY(x, y));
                }
            }

            public void Run(ClusterType clustertype)
            {
                switch (clustertype)
                {
                    case ClusterType.Grid:
                        _clusters = new GridCluster(_points).GetCluster();
                        DisplayGridInCanvas = true;
                        break;
                    case ClusterType.KMeans:
                        _clusters = new KMeans(_points).GetCluster();
                        break;
                    case ClusterType.MarkerClusterer:
                        _clusters = new MarkerClusterer(_points).GetCluster();
                        break;
                    case ClusterType.DistanceClusterer:
                        _clusters = new DistanceClusterer(_points).GetCluster();
                        break;
                    case ClusterType.Unknown:
                        break;
                }
            }

            public static string GetRandomColor()
            {
                int r = Rand.Next(10, 250);
                int g = Rand.Next(10, 250);
                int b = Rand.Next(10, 250);
                return string.Format("'rgb({0},{1},{2})'", r, g, b);
            }

            static void CreateFile(string s)
            {
                var path = new FileInfo(FolderPath + "draw.js");
                var isCreated = FileUtil.WriteFile(s, path);
                Console.WriteLine(isCreated + " = File is Created");
            }
            public void GenerateJavascriptDrawFile(ClusterType clusterType)
            {
                var sb = new StringBuilder();

                // markers
                var head = "function drawMarkers(ctx) {" + NL;
                var tail = NL + "}" + NL;

                sb.Append(head);

                // grid
                if (DisplayGridInCanvas)
                {
                    sb.Append("ctx.beginPath();" + NL);
                    for (int i = 0; i < GridX + 1; i++)
                    {
                        sb.Append(String.Format("ctx.moveTo({0}, {1});{2}",
                                                ToStringEN(MinX + i * DeltaX),
                                                ToStringEN(MinY), NL));
                        sb.Append(String.Format("ctx.lineTo({0}, {1});{2}",
                                                ToStringEN(MinX + i * DeltaX),
                                                ToStringEN(MaxY), NL));
                    }
                    for (int j = 0; j < GridY + 1; j++)
                    {
                        sb.Append(String.Format("ctx.moveTo({0}, {1});{2}",
                                                ToStringEN(MinX),
                                                ToStringEN(MinY + j * DeltaY), NL));
                        sb.Append(String.Format("ctx.lineTo({0}, {1});{2}",
                                                ToStringEN(MaxX),
                                                ToStringEN(MinY + j * DeltaY), NL));
                    }
                    sb.Append("ctx.stroke(); " + NL);
                }

                // if to many points, the canvas can not be drawn or is slow, 
                // use max points and clusters for drawing
                const int max = 10000;

                // markers
                if (_points != null)
                {
                    sb.Append(NL);
                    for (int i = 0; i < _points.Count; i++)
                    {
                        var p = _points[i];
                        sb.Append(string.Format("drawMark({0}, {1}, {2}, {3});{4}",
                                  ToStringEN(p.X), ToStringEN(p.Y), p.Color, ctx, NL));
                        if (i > max)
                            break;
                    }
                }
                string clusterInfo = "0";
                if (_clusters != null)
                {
                    sb.Append(NL);

                    if (clusterType == ClusterType.Grid || clusterType == ClusterType.KMeans)
                    {
                        for (int i = 0; i < _clusters.Count; i++)
                        {
                            var c = _clusters[i];
                            sb.Append(string.Format("drawCluster({0}, {1}, {2}, {3}, {4});{5}",
                                                    ToStringEN(c.X), ToStringEN(c.Y), c.Color,
                                                    c.Size, ctx, NL));

                            if (i > max)
                                break;
                        }
                    }

                    else if (clusterType == ClusterType.MarkerClusterer)
                    {
                        for (int i = 0; i < _clusters.Count; i++)
                        {
                            var c = _clusters[i];
                            sb.Append(string.Format(
                                "drawMarkerCluster({0}, {1}, {2}, {3}, {4}, {5});{6}",
                                                    ToStringEN(c.X), ToStringEN(c.Y), c.Color,
                                                    c.Size, MARKERCLUSTERER_SIZE, ctx, NL));

                            if (i > max)
                                break;
                        }
                    }
                    else if (clusterType == ClusterType.DistanceClusterer)
                    {
                        for (int i = 0; i < _clusters.Count; i++)
                        {
                            var c = _clusters[i];
                            sb.Append(string.Format(
                                "drawDistanceCluster({0}, {1}, {2}, {3}, {4}, {5});{6}",
                                                    ToStringEN(c.X), ToStringEN(c.Y), c.Color,
                                                    c.Size, DISTANCECLUSTERER_SIZE, ctx, NL));

                            if (i > max)
                                break;
                        }
                    }

                    clusterInfo = _clusters.Count + string.Empty;
                }

                // bottom text                
                sb.Append("ctx.fillStyle = 'rgb(0,0,0)';" + NL);
                sb.Append(string.Format("ctx.fillText('Clusters = ' + {0}, {1}, {2});{3}",
                    clusterInfo, ToStringEN(MinX + 10), ToStringEN(MaxY + 20), NL));

                sb.Append(tail);
                CreateFile(sb.ToString());
                //Console.WriteLine(sb.ToString());
            }
            public static string ToStringEN(double d)
            {
                double rounded = Math.Round(d, 3);
                return rounded.ToString(S, Culture_enUS);
            }
            public static void Profile(string s)//O(1)
            {
                if (!UseProfiling)
                    return;
           
            }


            [Serializable()]
            public class XY : IComparable, ISerializable
            {
                public double X { get; set; }
                public double Y { get; set; }
                public string Color { get; set; }
                public int Size { get; set; }

                //public string XToString(){return X.ToString(S, Culture_enUS);}
                //public string YToString(){return Y.ToString(S, Culture_enUS);}

                public XY() { }
                public XY(double x, double y)
                {
                    X = x;
                    Y = y;
                    Color = "'rgb(0,0,0)'";//default            
                }
                public XY(XY p) //clone
                {
                    this.X = p.X;
                    this.Y = p.Y;
                    this.Color = p.Color;
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

                // used by k-means random distinct selection of cluster point
                public override int GetHashCode()
                {
                    var x = X * 10000; //make the decimals be important
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

                    // rounding could be skipped
                    // depends on granularity of wanted decimal precision
                    // note, 2 points with same x,y is regarded as being equal
                    var x = Math.Round(this.X, ROUND) == Math.Round(other.X, ROUND);
                    var y = Math.Round(this.Y, ROUND) == Math.Round(other.Y, ROUND);
                    return x && y;
                }


                public XY(SerializationInfo info, StreamingContext ctxt)
                {
                    this.X = (double)info.GetValue("X", typeof(double));
                    this.Y = (double)info.GetValue("Y", typeof(double));
                    this.Color = (string)info.GetValue("Color", typeof(string));
                    this.Size = (int)info.GetValue("Size", typeof(int));
                }

                public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
                {
                    info.AddValue("X", this.X);
                    info.AddValue("Y", this.Y);
                    info.AddValue("Color", this.Color);
                    info.AddValue("Size", this.Size);
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


            public abstract class BaseClusterAlgorithm
            {
                public List<XY> BaseDataset; // all points
                //id, bucket
                public static readonly Dictionary<string, Bucket> BaseBucketsLookup =
                    new Dictionary<string, Bucket>();

                public BaseClusterAlgorithm() { }
                public BaseClusterAlgorithm(List<XY> dataset)
                {
                    if (dataset == null || dataset.Count == 0)
                        throw new ApplicationException(
                            string.Format("dataset is null or empty"));

                    BaseDataset = dataset;
                }

                public abstract List<XY> GetCluster();
                //O(k?? random fn can be slow, but is not slow because atm the k is always 1)
                public static XY[] BaseGetRandomCentroids(List<XY> list, int k)
                {
                    Profile("BaseGetRandomCentroids beg");
                    var set = new HashSet<XY>();
                    int i = 0;
                    var kcentroids = new XY[k];

                    int MAX = list.Count;
                    while (MAX >= k)
                    {
                        int index = Rand.Next(0, MAX - 1);
                        var xy = list[index];
                        if (set.Contains(xy))
                            continue;

                        set.Add(xy);
                        kcentroids[i++] = new XY(xy.X, xy.Y) { Color = GetRandomColor() };

                        if (i >= k)
                            break;
                    }
                    return kcentroids;
                }

                public List<XY> BaseGetClusterResult()
                {
                    Profile("BaseGetClusterResult beg");

                    // collect used buckets and return the result
                    var clusterPoints = new List<XY>();
                    foreach (var item in BaseBucketsLookup)
                    {
                        var bucket = item.Value;
                        if (bucket.IsUsed)
                        {
                            bucket.Centroid.Size = bucket.Points.Count;
                            clusterPoints.Add(bucket.Centroid);
                        }
                    }

                    return clusterPoints;
                }
                public static XY BaseGetCentroidFromCluster(List<XY> list) //O(n)
                {
                    Profile("BaseGetCentroidFromCluster beg");
                    int count = list.Count;
                    if (list == null || count == 0)
                        return null;

                    // color is set for the points and the cluster point here
                    var color = GetRandomColor(); //O(1)
                    XY centroid = new XY(0, 0) { Size = list.Count };//O(1)
                    foreach (XY p in list)
                    {
                        p.Color = color;
                        centroid.X += p.X;
                        centroid.Y += p.Y;
                    }
                    centroid.X /= count;
                    centroid.Y /= count;
                    var cp = new XY(centroid.X, centroid.Y) { Size = count, Color = color };

                    return cp;
                }
                //O(k*n)
                public static void BaseSetCentroidForAllBuckets(IEnumerable<Bucket> buckets)
                {
                    Profile("BaseSetCentroidForAllBuckets beg");
                    foreach (var item in buckets)
                    {
                        var bucketPoints = item.Points;
                        var cp = BaseGetCentroidFromCluster(bucketPoints);
                        item.Centroid = cp;
                    }
                }
                public double BaseGetTotalError()//O(k)
                {
                    int centroidsUsed = BaseBucketsLookup.Values.Count(b => b.IsUsed);
                    double sum = BaseBucketsLookup.Values.
                        Where(b => b.IsUsed).Sum(b => b.ErrorLevel);
                    return sum / centroidsUsed;
                }
                public string BaseGetMaxError() //O(k)
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
                // assign all points to nearest cluster
                public void BaseUpdatePointsByCentroid()//O(n*k)
                {
                    Profile("UpdatePointsByCentroid beg");
                    int count = BaseBucketsLookup.Count();

                    // clear points in the buckets, they will be re-inserted
                    foreach (var bucket in BaseBucketsLookup.Values)
                        bucket.Points.Clear();

                    foreach (XY p in BaseDataset)
                    {
                        double minDist = Double.MaxValue;
                        string index = string.Empty;
                        //for (int i = 0; i < count; i++)
                        foreach (var i in BaseBucketsLookup.Keys)
                        {
                            var bucket = BaseBucketsLookup[i];
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
                        var closestBucket = BaseBucketsLookup[index];
                        p.Color = closestBucket.Centroid.Color;
                        closestBucket.Points.Add(p);
                    }
                }

                // update centroid location to nearest point, 
                // e.g. if you want to show cluster point on a real existing point area
                //O(n)
                public static void BaseUpdateCentroidToNearestContainingPoint(Bucket bucket)
                {
                    Profile("BaseUpdateCentroidToNearestContainingPoint beg");
                    if (bucket == null || bucket.Centroid == null ||
                        bucket.Points == null || bucket.Points.Count == 0)
                        return;

                    var closest = BaseGetClosestPoint(bucket.Centroid, bucket.Points);
                    bucket.Centroid.X = closest.X;
                    bucket.Centroid.Y = closest.Y;
                }
                //O(k*n)
                public static void BaseUpdateAllCentroidsToNearestContainingPoint()
                {
                    Profile("BaseUpdateAllCentroidsToNearestContainingPoint beg");
                    foreach (var bucket in BaseBucketsLookup.Values)
                        BaseUpdateCentroidToNearestContainingPoint(bucket);
                }
            }

            public class DistanceClusterer : BaseClusterAlgorithm
            {
                public DistanceClusterer(List<XY> dataset)
                    : base(dataset)
                {
                }

                public override List<XY> GetCluster()
                {
                    var cluster = RunClusterAlgo();
                    return cluster;
                }

                // O(k*n)
                List<XY> RunClusterAlgo()
                {
                    // put points in buckets     
                    int allPointsCount = BaseDataset.Count;
                    var firstPoint = BaseDataset[0];
                    firstPoint.Color = GetRandomColor();
                    var firstId = 0.ToString();
                    var firstBucket = new Bucket(firstId) { Centroid = firstPoint };
                    BaseBucketsLookup.Add(firstId, firstBucket);

                    for (int i = 1; i < allPointsCount; i++)
                    {
                        var set = new HashSet<string>(); //cluster candidate list
                        var p = BaseDataset[i];
                        // iterate clusters and collect candidates
                        foreach (var bucket in BaseBucketsLookup.Values)
                        {
                            var isInCluster = MathTool.DistWithin(p, bucket.Centroid, DISTANCECLUSTERER_SIZE);
                            if (!isInCluster)
                                continue;

                            set.Add(bucket.Id);
                            //use first, short dist will be calc at last step before returning data
                            break;
                        }

                        // if not within box area, then make new cluster   
                        if (set.Count == 0)
                        {
                            var pid = i.ToString();
                            p.Color = GetRandomColor();
                            var newbucket = new Bucket(pid) { Centroid = p };
                            BaseBucketsLookup.Add(pid, newbucket);
                        }
                    }

                    //important, align all points to closest cluster point
                    BaseUpdatePointsByCentroid();

                    return BaseGetClusterResult();
                }

            }

            public class MarkerClusterer : BaseClusterAlgorithm
            {
                public MarkerClusterer(List<XY> dataset)
                    : base(dataset)
                {
                }

                public override List<XY> GetCluster()
                {
                    var cluster = RunClusterAlgo();
                    return cluster;
                }

                // O(k*n)
                List<XY> RunClusterAlgo()
                {
                    // put points in buckets     
                    int allPointsCount = BaseDataset.Count;
                    var firstPoint = BaseDataset[0];
                    firstPoint.Color = GetRandomColor();
                    var firstId = 0.ToString();
                    var firstBucket = new Bucket(firstId) { Centroid = firstPoint };
                    BaseBucketsLookup.Add(firstId, firstBucket);

                    for (int i = 1; i < allPointsCount; i++)
                    {
                        var set = new HashSet<string>(); //cluster candidate list
                        var p = BaseDataset[i];
                        // iterate clusters and collect candidates
                        foreach (var bucket in BaseBucketsLookup.Values)
                        {
                            var isInCluster = MathTool.BoxWithin(p, bucket.Centroid, MARKERCLUSTERER_SIZE);
                            if (!isInCluster)
                                continue;

                            set.Add(bucket.Id);
                            //use first, short dist will be calc at last step before returning data
                            break;
                        }

                        // if not within box area, then make new cluster   
                        if (set.Count == 0)
                        {
                            var pid = i.ToString();
                            p.Color = GetRandomColor();
                            var newbucket = new Bucket(pid) { Centroid = p };
                            BaseBucketsLookup.Add(pid, newbucket);
                        }
                    }

                    //important, align all points to closest cluster point
                    BaseUpdatePointsByCentroid();

                    return BaseGetClusterResult();
                }
            }

            // O(exponential) ~ can be slow when n or k is big
            public class KMeans : BaseClusterAlgorithm
            {
                private readonly int _InitClusterSize; // start from this cluster points
                // Rule of thumb k = sqrt(n/2)        

                // cluster point optimization iterations
                private const int _MaxIterations = 100;
                private const int _MaxClusters = 100;

                public KMeans(List<XY> dataset)
                    : base(dataset)
                {
                    _InitClusterSize = 1;
                }

                public override List<XY> GetCluster()
                {
                    var cluster = RunClusterAlgo();
                    return cluster;
                }

                List<XY> RunClusterAlgo()
                {
                    /*
                    ITERATIVE LINEAR ADDING CLUSTER UNTIL 
                    REPEATE iteration of clusters convergence 
                       until the max error is small enough
                    if not, insert a new cluster at worst place 
                    (farthest in region of worst cluster area) and run again 
                    keeping current cluster points              
             
                     // one iteration of clusters convergence is defined as ..
                  1) Random k centroids
                  2) Cluster data by euclidean distance to centroids
                  3) Update centroids by clustered data,     
                  4) Update cluster
                  5) Continue last two steps until error is small, error is sum of diff
                      between current and updated centroid                          
                   */

                    RunAlgo();

                    if (DoUpdateAllCentroidsToNearestContainingPoint)
                        BaseUpdateAllCentroidsToNearestContainingPoint();
                    return BaseGetClusterResult();
                }

                void RunAlgo()
                {
                    // Init clusters
                    var centroids = BaseGetRandomCentroids(BaseDataset, _InitClusterSize);
                    for (int i = 0; i < centroids.Length; i++)
                    {
                        var pid = i.ToString();
                        var newbucket = new Bucket(pid) { Centroid = centroids[i] };
                        BaseBucketsLookup.Add(pid, newbucket);
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
                            // Here it is linear speed when putting one new centroid at a time
                            // should be semi-fast because the new point is inserted at best area 
                            //from current centroids view.
                            // possible improvement exists by log2 search by inserting multiple centroids and 
                            // reducing centroid again if needed

                            // put new centroid in area where maxError but farthest away from current centroid in area
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
                    Profile("RunIterationsUntilKClusterPlacementAreDone beg");
                    double prevError = Double.MaxValue;
                    double currError = Double.MaxValue;

                    for (int i = 0; i < _MaxIterations; i++)
                    {
                        prevError = currError;
                        currError = RunOneIteration();
                        if (currError >= prevError) // no improvement
                            break;
                    }
                }

                double RunOneIteration() //O(k*n)
                {
                    Profile("RunOneIteration beg");

                    // update points, assign points to cluster
                    BaseUpdatePointsByCentroid();
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

                    return BaseGetTotalError();
                }

            }

            public class GridCluster : BaseClusterAlgorithm
            {
                public GridCluster(List<XY> dataset)
                    : base(dataset)
                {
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

                        // merge
                        var color = current.Centroid.Color;
                        foreach (var p in neighbor.Points)
                        {
                            //update color
                            p.Color = color;
                        }

                        current.Points.AddRange(neighbor.Points);//O(n)

                        // recalc centroid
                        var cp = BaseGetCentroidFromCluster(current.Points);
                        current.Centroid = cp;
                        neighbor.IsUsed = false; //merged, then not used anymore
                        neighbor.Points.Clear(); //clear mem
                    }
                }

                public List<XY> RunClusterAlgo(int gridx, int gridy)
                {
                    // params invalid
                    if (gridx <= 0 || gridy <= 0)
                        throw new ApplicationException("GridCluster.RunClusterAlgo gridx or gridy is <= 0");

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
                    if (DoMergeGridIfCentroidsAreCloseToEachOther)
                        MergeClustersGrid();

                    if (DoUpdateAllCentroidsToNearestContainingPoint)
                    {
                        BaseUpdateAllCentroidsToNearestContainingPoint();
                    }

                    // check again
                    // merge if gridpoint is to close
                    if (DoMergeGridIfCentroidsAreCloseToEachOther
                        && DoUpdateAllCentroidsToNearestContainingPoint)
                    {
                        MergeClustersGrid();
                        // and again set centroid to closest point in bucket 
                        BaseUpdateAllCentroidsToNearestContainingPoint();
                    }

                    return BaseGetClusterResult();
                }
            }

            public class MathTool
            {
                private const double Exp = 2; // 2=euclid, 1=manhatten
                //Minkowski dist        
                public static double Distance(XY a, XY b)
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

                public static bool DistWithin(XY a, XY b, double d)
                {
                    var dist = Distance(a, b);
                    return dist < d;
                }

                public static bool BoxWithin(XY a, XY b, double boxsize)
                {
                    var d = boxsize / 2;
                    var withinX = a.X - d <= b.X && a.X + d >= b.X;
                    var withinY = a.Y - d <= b.Y && a.Y + d >= b.Y;
                    return withinX && withinY;
                }
            }

            public static class FileUtil
            {
                public static bool WriteFile(string data, FileInfo fileInfo)
                {
                    bool isSuccess = false;
                    try
                    {
                        using (StreamWriter streamWriter =
                            File.CreateText(fileInfo.FullName))
                        {
                            streamWriter.Write(data);
                            isSuccess = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace + "\nPress a key ... ");
                        Console.ReadKey();
                    }
                    return isSuccess;
                }
            }

            [Serializable()]
            public class DatasetToSerialize : ISerializable
            {
                private const string Name = "Dataset";
                public List<XY> Dataset { get; set; }
                public DatasetToSerialize()
                {
                    Dataset = new List<XY>();
                }
                public DatasetToSerialize(SerializationInfo info, StreamingContext ctxt)
                {
                    this.Dataset = (List<XY>)info.GetValue(Name, typeof(List<XY>));
                }
                public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
                {
                    info.AddValue(Name, this.Dataset);
                }
            }

            public class Serializer
            {
                public void SerializeObject(string filename, object objectToSerialize)
                {
                    try
                    {
                        using (Stream stream = File.Open(filename, FileMode.Create))
                        {
                            BinaryFormatter bFormatter = new BinaryFormatter();
                            bFormatter.Serialize(stream, objectToSerialize);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace + "\nPress a key ... ");
                        Console.ReadKey();
                    }
                }
                public object DeSerializeObject(string filename)
                {
                    try
                    {
                        using (Stream stream = File.Open(filename, FileMode.Open))
                        {
                            BinaryFormatter bFormatter = new BinaryFormatter();
                            var objectToSerialize = bFormatter.Deserialize(stream);
                            return objectToSerialize;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace + "\nPress a key ... ");
                        Console.ReadKey();
                    }
                    return null;
                }
            }

            internal static void BaseUpdateAllCentroidsToNearestContainingPoint()
            {
                throw new NotImplementedException();
            }
        }
    

}

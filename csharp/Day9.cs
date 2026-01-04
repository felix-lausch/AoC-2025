class Day9 {
    public static void Solve() {
        using var sr = File.OpenText("inputs/day9input.txt");

        var points = new List<Point>();
        var edges = new List<Edge>();
        var maxArea = -1L;
        var absMinX = 100000d;
        var absMaxX = -1d;
        var areas = new List<Rectangle>();

        while (!sr.EndOfStream) {
            var line1 = sr.ReadLine()!;
            var splitLine1 = line1.Split(",");
            var newP1 = new Point(int.Parse(splitLine1[0]), int.Parse(splitLine1[1]));

            if (newP1.x < absMinX) absMinX = newP1.x;
            if (newP1.x > absMaxX) absMaxX = newP1.x;

            foreach (var existingP in points) {
                areas.Add(new Rectangle(newP1, existingP));
            }

            points.Add(newP1);
        }

        if (points.Count % 2 != 0) {
            throw new Exception("wrong assumption");
        }

        for (int i = 0; i < points.Count; i++) {
            var edge = new Edge(points[i], points[(i+1)%points.Count]);

            //only add vertical up and down edges
            if (edge.GetOrientation() != Orientation.Horizontal) {
                edges.Add(edge);
            }
        }

        areas = areas.OrderByDescending(x => x.Area).ToList();

        foreach (var rect in areas) {
            var isRectInside = true;

            foreach (var innerPoint in rect.GetInnerPoints()) {
                if (!IsPointInside(innerPoint)) {
                    isRectInside = false;
                    break;
                }
            }

            if (isRectInside) {
                Console.WriteLine($"The largest area is: {rect.Area}");
                // break;
            }
        }

        Console.WriteLine($"The final result is: {maxArea}");

        bool IsPointInside(Point point) {
            var relEdges = edges.Where(x => x.p1.x > point.x);
            var infEdge = new Edge(point, new(absMaxX+1, point.y));

            var num = 0;
            foreach (var relEdge in relEdges) {
                if (infEdge.Intersect(relEdge)) {
                    var o = relEdge.GetOrientation();
                    if (o == Orientation.Up) num++;
                    if (o == Orientation.Down) num--;
                }
            }

            return Math.Abs(num % 2) == 1;
        }

    }

    class Rectangle {

        public Rectangle(Point p1, Point p2)
        {
            Area = p1.Area(p2);

            var p3 = new Point(p1.x, p2.y);
            var p4 = new Point(p2.x, p1.y);

            var list = new List<Point>() { p1, p2, p3, p4 };

            var minX = 1000000d;
            var minY = 1000000d;
            var maxX = 0d;
            var maxY = 0d;

            foreach (var p in list) {
                if (p.x < minX) minX = p.x;
                if (p.y < minY) minY = p.y;

                if (p.x > maxX) maxX = p.x;
                if (p.y > maxY) maxY = p.y;
            }

            TopLeft = list.First(p => p.x == minX && p.y == minY);
            TopRight = list.Where(p => p.x == maxX && p.y == minY).First();
            BotLeft = list.Where(p => p.x == minX && p.y == maxY).First();
            BotRight = list.Where(p => p.x == maxX && p.y == maxY).First();
        }

        public Point TopLeft { get; }
        public Point TopRight { get; }
        public Point BotLeft { get; }
        public Point BotRight { get; }
        public double Area { get; }

        public List<Point> GetInnerPoints() {
            return new List<Point>() {
                new(TopLeft.x + 0.5, TopLeft.y + 0.5),
                new(TopRight.x - 0.5, TopRight.y + 0.5),
                new(BotLeft.x + 0.5, BotLeft.y - 0.5),
                new(BotRight.x - 0.5, BotRight.y - 0.5),
            };
        }
    }

    record Edge(Point p1, Point p2) {
        public Orientation GetOrientation() {
            if (p1.x == p2.x) {
                return p1.y < p2.y ? Orientation.Down : Orientation.Up;
            }

            return Orientation.Horizontal;
        }

        public bool Intersect(Edge other) {
            if (this.p1.y < Math.Min(other.p1.y, other.p2.y)) {
                return false;
            }

            if (this.p1.y > Math.Max(other.p1.y, other.p2.y)) {
                return false;
            }

            if (Math.Min(this.p1.x, this.p2.x) <= other.p1.x && Math.Max(this.p1.x, this.p2.x) >= other.p1.x) {
                return true;
            }

            return false;
        }
    }

    enum Orientation {
        Up,
        Down,
        Horizontal
    }

    record Point(double x, double y) {
        public double Area(Point p1) {
            var w = Math.Abs(this.x - p1.x) + 1;
            var h = Math.Abs(this.y - p1.y) + 1;

            return w * h;
        }
    }
}

//part1 solution
// using var sr = File.OpenText("day9input.txt");

// var points = new List<Point>();
// var maxArea = -1L;

// while (!sr.EndOfStream) {
//     var line = sr.ReadLine()!;
//     var splitLine = line.Split(",");
//     var newP = new Point(int.Parse(splitLine[0]), int.Parse(splitLine[1]));

//     foreach (var existingP in points) {
//         var area = newP.Area(existingP);
//         if (area > maxArea) {
//             maxArea = area;
//         }
//     }

//     points.Add(newP);
// }

// Console.WriteLine($"The final result is: {maxArea}");
// record Point(long x, long y) {
//     public long Area(Point p1) {
//         var w = Math.Abs(this.x - p1.x) + 1;
//         var h = Math.Abs(this.y - p1.y) + 1;

//         return w * h;
//     }
// }
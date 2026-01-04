class Day8 {
    public static void Solve() {

        using var sr = File.OpenText("inputs/day8input.txt");

        var coordinates = new List<Point>();
        var distances = new List<(double Dist, Point P1, Point P2)>();

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()!;
            var splitLine = line.Split(",");
            var point = new Point(int.Parse(splitLine[0]), int.Parse(splitLine[1]), int.Parse(splitLine[2]));

            foreach (var coord in coordinates) {
                var dist = coord.Distance(point);
                distances.Add((dist, coord, point));
            }

            coordinates.Add(point);
        }

        var sortedDistances = distances.OrderBy(x => x.Dist).ToList();
        var circuits = new List<Circuit>();

        foreach (var c in coordinates) {
            var circuit = new Circuit(c);

            c.Circuit = circuit;
            circuits.Add(circuit);
        }

        var result = 0L;
        var iterations = 0;

        // for (int i = 0; i < 1000; i++)
        foreach (var curr in sortedDistances)
        {
            iterations++;
            // var curr = sortedDistances[i];
            
            //possibile cases:

            //they already belong to the same circuit
            if (curr.P1.Circuit == curr.P2.Circuit) {
                continue;
            }

            //they belong to different circuits -> merge and update references
            var keep = curr.P1.Circuit!;
            var remove = curr.P2.Circuit!;

            keep.Members.UnionWith(remove.Members);

            foreach (var p in remove.Members)
                p.Circuit = keep;

            circuits.Remove(remove);

            if (circuits.Count == 1) {
                result = (long)curr.P1.x * curr.P2.x;
            }
        }

        // var orderedCounts = circuits
        //     .Select(x => x.Members.Count)
        //     .OrderByDescending(x => x)
        //     .ToList();

        // var result = orderedCounts
        //     .Take(3)
        //     .Aggregate((a, b) => a * b);

        Console.WriteLine($"The final result is: {result}. Calculated in {iterations} iterations.");

    }

    record Point(int x, int y, int z) {
            public Circuit? Circuit {get; set;}

            public double Distance(Point point) {
                var a = Math.Pow(point.x - this.x, 2);
                var b = Math.Pow(point.y - this.y, 2);
                var c = Math.Pow(point.z - this.z, 2);

                return Math.Sqrt(a+b+c);
            }
        }

    class Circuit {

        public Circuit(Point p)
        {
            Members.Add(p);
        }

        public HashSet<Point> Members { get; set; } = new();
    }
}
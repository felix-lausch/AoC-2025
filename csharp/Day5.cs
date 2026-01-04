class Day5 {
    public static void Solve() {
        using var sr = File.OpenText("inputs/day5input.txt");

        var freshIds = new List<LRange>();
        bool parsingFreshIds = true;

        while (!sr.EndOfStream) {
            var line = sr.ReadLine();
            
            if (string.IsNullOrEmpty(line)) {
                parsingFreshIds = false;
                break;
            }

            if (parsingFreshIds) {
                var splitLine = line.Split('-');

                var beginRange = long.Parse(splitLine[0]);
                var endRange = long.Parse(splitLine[1]);

                freshIds.Add(new LRange(beginRange, endRange));
            }
            // else {
            //     var id = long.Parse(line);
            //     foreach (var lrange in freshIds) {
            //         if (lrange.Contains(id)) {
            //             result++;
            //             break;
            //         }
            //     }
            // }
        }

        freshIds = freshIds.OrderBy(x => x.Start).ToList();

        for (int i = 0; i < freshIds.Count; i++) {
            var currentEnd = freshIds[i].End;
            var smaller = freshIds[(i+1)..].Where(x => x.Start <= currentEnd).ToArray();

            foreach (var x in smaller) {
                x.Start = currentEnd+1;
                if (x.Start >= x.End) x.Ignore = true;
            }
        }

        var result = freshIds
            .Where(x => !x.Ignore)
            .Sum(x => (x.End - x.Start) + 1);

        Console.WriteLine($"The final result is: {result}");
    }

    private class LRange {

        public LRange(long start, long end)
        {
            this.Start = start;
            this.End = end;
        }

        public long Start { get; set; }

        public long End { get; set; }

        public bool Ignore { get; set; }


        public bool Contains(long num) {
            return num >= Start && num <= End;
        }
    }
}


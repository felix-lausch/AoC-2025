namespace aoc;

class Day7 {
    static void Solve(string[] args) {
        using var sr = File.OpenText("day7input.txt");

        var lines = new List<string>();

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()!;
            lines.Add(line);
        }

        var result = 0L;

        var lastPositions = new Dictionary<int, long>();

        var sIndex = lines[0].IndexOf('S');
        lastPositions.Add(sIndex, 1);

        foreach (var line in lines[1..]) {
            var positions = new Dictionary<int, long>();

            foreach (var (key, val) in lastPositions) {
                var c = line[key];

                if (c == '.') {
                    InsertOrCountUp(positions, key, val);
                } else if (c == '^')
                {
                    InsertOrCountUp(positions, key-1, val);
                    InsertOrCountUp(positions, key+1, val);
                    result++;
                }

            }

            lastPositions = positions;
        }

        Console.WriteLine($"The final result is: {result}");
        Console.WriteLine($"There are {lastPositions.Values.Sum()} unique paths.");
    }

    private static void InsertOrCountUp(Dictionary<int, long> positions, int key, long val)
    {
        if (positions.ContainsKey(key)) {
            positions[key] += val;
            return;
        }

        positions.Add(key, val);
    }
}
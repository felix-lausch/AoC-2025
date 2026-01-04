class Day11 {
    Dictionary<(string, bool, bool), long> memo = new();

    public void Solve() {
        using var sr = File.OpenText("day11input.txt");

        var devices = new Dictionary<string, string[]>();

        while (!sr.EndOfStream) {
            var splitLine = sr.ReadLine()!.Split(":");
            var connections = splitLine[1][1..].Split(" ");

            devices.Add(splitLine[0], connections);
        }

        var result1 = SolvePart1("you");
        var result2 = SolvePart2("svr", false, false);

        Console.WriteLine($"The final result for part 1 is: {result1}");
        Console.WriteLine($"The final result for part 2 is: {result2}");

        long SolvePart1(string start) {
            var paths = new List<string[]>() { new string[] { start } };
            var result = 0L;

            //while paths is not empty
            while (paths.Count != 0) {
                var nextPaths = new List<string[]>();

                //foreach path
                foreach (var path in paths) {
                    //go split into each available connection
                    foreach (var output in devices[path.Last()]) {
                        //check if reached out -> count up result +1
                        if (output == "out") {
                            result++;
                            continue;
                        }

                        //else -> add those paths to nextPaths
                        var nextPath = new string[path.Length + 1];
                        Array.Copy(path, nextPath, path.Length);
                        nextPath[nextPath.Length - 1] = output;

                        nextPaths.Add(nextPath);
                    }
                }
                paths = nextPaths;
            }

            return result;
        }

        long SolvePart2(string current, bool visitedDac, bool visitedFft) {
            var key = (current, visitedDac, visitedFft);
            if (memo.TryGetValue(key, out var value)) {
                return value;
            }

            long result = 0;

            if (current == "out") {
                result = (visitedDac && visitedFft) ? 1 : 0;
            } else {
                if (current == "fft") visitedFft = true;
                else if (current == "dac") visitedDac = true;

                result = devices[current].Sum(x => SolvePart2(x, visitedDac, visitedFft));
            }
            
            memo[key] = result;
            return result;
        }
    }
}

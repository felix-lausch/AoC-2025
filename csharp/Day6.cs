namespace aoc;

class Day6 {
    static void Solve(string[] args) {
        using var sr = File.OpenText("inputs/day6input.txt");

        var lines = new List<string>();
        var cols = -1;

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()! + " ";
            lines.Add(line);

            if (cols == -1) {
                cols = line.Length;
            }
            else if (cols != line.Length) {
                throw new Exception("wrong assumption! Cols per line are not equal.");
            }
        }

        var operators = lines.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var operationCount = 0;
        var nums = new List<long>();
        var result = 0L;

        for (int i = 0; i < cols; i++) {
            var current = string.Empty;

            foreach (var line in lines[..^1]) {
                current += line[i];
            }

            if (!long.TryParse(current, out var num)) {
                var op = operators[operationCount];

                if (op == "+") {
                    result += nums.Sum();
                } else if (op == "*") {
                    result += nums.Aggregate((a,b) => a*b);
                }

                nums.Clear();
                operationCount++;
            } else {
                nums.Add(num);
            }
        }

        Console.WriteLine($"The final result is: {result}");
    }
}
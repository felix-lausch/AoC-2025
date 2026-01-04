class Day4 {
    public static void Solve() {
        using var sr = File.OpenText("input.txt");

        var grid = new List<char[]>();

        while (!sr.EndOfStream) {
            var line = sr.ReadLine();
            grid.Add(line!.ToCharArray());
        }

        var total = 0;
        var lastResult = 1;

        while (lastResult > 0) {
            lastResult = RemoveRolls(grid);
            total += lastResult;
        }

        Console.WriteLine($"The final result is: {total}");

        static int RemoveRolls(List<char[]> grid) {
            var result = 0;
            var takenRolls = new List<(int, int)>();

            for (int i = 0; i < grid.Count; i++) {
                for (int j = 0; j < grid[i].Length; j++) {
                    if (grid[i][j] != '@') {
                        continue;
                    }

                    var current = 0;

                    current += CheckRoll(grid, i-1, j-1);
                    current += CheckRoll(grid, i-1, j);
                    current += CheckRoll(grid, i-1, j+1);
                    current += CheckRoll(grid, i, j-1);
                    current += CheckRoll(grid, i, j+1);
                    current += CheckRoll(grid, i+1, j-1);
                    current += CheckRoll(grid, i+1, j);
                    current += CheckRoll(grid, i+1, j+1);

                    if (current < 4) {
                        takenRolls.Add((i,j));
                        result++;
                    }
                }
            }

            foreach (var (x, y) in takenRolls) {
                grid[x][y] = 'x';
            }

            return result;
        }

        static int CheckRoll(List<char[]> grid, int i, int j) {
            if (i < 0 || i >= grid.Count) {
                return 0;
            }

            if (j < 0 || j >= grid[i].Length) {
                return 0;
            }

            if (grid[i][j] != '@') {
                return 0;
            }

            return 1;
        }
    }
}
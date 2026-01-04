class Day12 {
    public static void Solve() {
        using var sr = File.OpenText("../inputs/day12input.txt");

        var text = sr.ReadToEnd()!;
        var splitText = text.Split("\r\n\r");

        var lines = splitText[6][1..].Split("\n");

        var result = 0;
        foreach (var l in lines) {
            var splitLine = l[..^2].Split(": ");
            var gridParts = splitLine[0].Split("x");

            var x = int.Parse(gridParts[0]);
            var y = int.Parse(gridParts[1]);
            var totalArea = x*y;

            var counts = splitLine[1].Trim().Split(" ").Select(x => int.Parse(x)).ToArray();

            var occupiedAreaEstimate = counts.Sum() * 3 * 3; //assume every shape covers 3x3 area
            if (occupiedAreaEstimate < totalArea) {
                result++;
            }
        }

        Console.WriteLine($"The final result is: {result}");
    }
}
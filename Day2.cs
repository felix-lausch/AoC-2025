class Day2 {
    public static void Solve() {

        using var sr = File.OpenText("day2input.txt");

        var line = sr.ReadLine()!;
        var splitLine = line.Split(',');

        var result = 0L;

        foreach (var range in splitLine) {
            var splitRange = range.Split('-');

            var s1 = splitRange[0];
            var s2 = splitRange[1];

            var num1 = long.Parse(s1);
            var num2 = long.Parse(s2);

            for (long i = num1; i <= num2; i++) {
                var iString = i.ToString();

                // SolvePart1(iString, i);
                SolvePart2(iString, i);
            }
        }

        Console.WriteLine($"The final result is: {result}");

        void SolvePart2(string iString, long num) {
            var partLength = iString.Length/2;

            while (partLength > 0) {
                if (iString.Length % partLength == 0) {
                    var numParts = iString.Length / partLength;
                    var parts = new string[numParts];

                    for (int i = 0; i < numParts; i++) {
                        parts[i] = iString.Substring(partLength*i, partLength);
                    }

                    var allEqual = parts.All(x => x == parts[0]);
                    if (allEqual) {
                        result += num;
                        break;
                    }
                }

                partLength--;
            }
        }

        void SolvePart1(string iString, long i) {
            if (iString.Length % 2 != 0) {
                return;
            }

            var firstHalf = iString.Substring(0, iString.Length/2);
            var secondHalf = iString.Substring(iString.Length/2);

            if (firstHalf == secondHalf) {
                result += i;
            }
        }
    }
}
class Day3 {
    public static void Solve() {

        using var sr = File.OpenText("inputs/day3input.txt");

        var result = 0L;

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()!;
            var nums = new int[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                nums[i] = c - '0';
            }

            var remainingDigits = 11;
            var startIdx = 0;
            var digits = new int[12];

            while (remainingDigits >= 0) {
                var (val, idx) = FindLargestValue(nums[startIdx..^remainingDigits]);

                digits[11-remainingDigits] = val;
                remainingDigits--;
                startIdx = idx + startIdx + 1;
            }

            var nextValue = long.Parse(string.Join("", digits));
            result += nextValue;
        }

        Console.WriteLine($"The final result is: {result}");

        (int, int) FindLargestValue(int[] nums) {
            var result = -1;
            var idx = -1;

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > result) {
                    result = nums[i];
                    idx = i;
                }
            }

            return (result, idx);
        }
    }
}
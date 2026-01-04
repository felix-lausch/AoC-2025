class Day1 {
    static void Solve() {
        using var sr = File.OpenText("inputs/day1input.txt");

        var curr = 50;
        var result = 0;

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()!;
            Console.WriteLine(line);

            var next = curr;
            var clicks = int.Parse(line[1..]);

            if (line[0] == 'L') {
                while (clicks > 0) {
                    next -= 1;
                    clicks -=1;

                    if (next < 0) {
                        next += 100;
                    }

                    if (next == 0) {
                        result++;
                    }
                }
            } else {
                while (clicks > 0) {
                    next += 1;
                    clicks -= 1;

                    if (next > 99) {
                        next -= 100;
                    }

                    if (next == 0) {
                        result++;
                    }
                }
            }
            
            Console.WriteLine($"Curr: {curr} Nxt: {next}");
            curr = next;
        }

        Console.WriteLine($"The final result is: {result}");
    }
}
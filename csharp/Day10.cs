using Microsoft.Z3;

class Day10 {
    public static void Solve() {
        using var sr = File.OpenText("inputs/day10input.txt");

        var result = 0;

        while (!sr.EndOfStream) {
            var line = sr.ReadLine()!;

            var braceIndex = line.IndexOf("(");
            var light = line.Substring(1, braceIndex-3);

            var curlyBraceIndex = line.IndexOf("{");
            var buttonsStr = line.Substring(braceIndex, curlyBraceIndex-braceIndex);
            var buttonsStrSplit = buttonsStr.Split(" ");

            var buttons = new List<int[]>();

            foreach (var x in buttonsStrSplit.Where(s => !string.IsNullOrEmpty(s))) {
                var nums = x.Substring(1, x.Length-2).Split(",");
                var numArray = nums.Select(num => int.Parse(num)).ToArray();
                buttons.Add(numArray);
            }

            var joltageStr = line.Substring(curlyBraceIndex+1, line.Length-curlyBraceIndex-2);
            var joltage = joltageStr
                .Split(",")
                .Select(x => int.Parse(x))
                .ToArray();

            // result += SolvePart1(light, buttons);
            result += SolvePart2Z3(joltage, buttons);
            Console.WriteLine("Processed line");
        }


        Console.WriteLine($"The final result is: {result}");

        //solution from reddit using Z3 theorem solver
        int SolvePart2Z3(int[] target, List<int[]> buttons) {

            using var ctx = new Context();
            using var opt = ctx.MkOptimize();
            
            //create int constants for each button
            var presses = Enumerable.Range(0, buttons.Count)
                .Select(i => ctx.MkIntConst($"p{i}"))
                .ToArray();

            //add expression for each button >= 0 => cant go backwards
            foreach (var press in presses) {
                var greaterExpression = ctx.MkGe(press, ctx.MkInt(0));
                opt.Add(greaterExpression);
            }

            //add all constraints for all button presses
            //sum of all affecting buttons must be equal to joltage target & that must be satisfied for all affecting buttons
            //at the same time because each button can affect multiple joltages
            for (int i = 0; i < target.Length; i++)
            {
                //find all buttons that affect joltage at pos: i
                var affecting = presses.Where((_, j) => buttons[j].Contains(i)).ToArray();
                if (affecting.Length > 0)
                {
                    //add expression that says that the sum of all button presses has to be equal to the value of that joltage
                    var sum = ctx.MkAdd(affecting);
                    opt.Add(ctx.MkEq(sum, ctx.MkInt(target[i])));
                }
            }

            //set minimization goal
            opt.MkMinimize(ctx.MkAdd(presses));
            //check model viability -> produce model
            var res = opt.Check();
            
            var model = opt.Model;
            return presses.Sum(p => ((IntNum)model.Evaluate(p, true)).Int);
        }

        //works for sample input but is too slow for real input
        int SolvePart2(int[] target, List<int[]> buttons) {
            var visited = new HashSet<string>();
            var highestVal = target.Max();

            var states = GetInitialStates(highestVal, target, buttons);
            var steps = highestVal-1;
            //off by one here i think
            while (true) {
                steps++;
                var nextStates = new List<int[]>();

                foreach (var state in states) {
                    foreach (var button in buttons) {
                        var nextState = PressJoltageButton(state, button);
                        if (nextState.SequenceEqual(target)) {
                            return steps;
                        }

                        var nextStateStr = string.Join("", nextState);
                        if (!visited.Contains(nextStateStr) && !ContainsTooHighJoltage(nextState, target)) {
                            nextStates.Add(nextState);
                        }
                            
                        visited.Add(nextStateStr);
                    }
                }

                states = nextStates;
            }
        }

        //find highest value in target
        //find all buttons that affect that value
        //press only all possible combinations of buttons targeting highest until it has reached its value
        //then continue normally
        List<int[]> GetInitialStates(int highestVal, int[] target, List<int[]> buttons) {

            var indices = target
                .Select((value, index) => new { value, index })
                .Where(x => x.value == highestVal)
                .Select(x => x.index)
                .ToArray();

            var relevantButtons = buttons
                .Where(button => indices.All(idx => button.Contains(idx)))
                .ToList();

            var visited = new HashSet<string>();
            var states = new List<int[]>() { Enumerable.Repeat(0, target.Length).ToArray() };
            var steps = 0;

            while (steps != highestVal-1) {
                steps++;
                var nextStates = new List<int[]>();

                foreach (var state in states) {
                    foreach (var button in relevantButtons) {
                        var nextState = PressJoltageButton(state, button);

                        var nextStateStr = string.Join("", nextState);
                        if (!visited.Contains(nextStateStr) && !ContainsTooHighJoltage(nextState, target)) {
                            nextStates.Add(nextState);
                        }
                            
                        visited.Add(nextStateStr);
                    }
                }

                states = nextStates;
            }

            return states;
        }

        bool ContainsTooHighJoltage(int[] nextState, int[] target) {
            for (int i = 0; i < target.Length; i++) {
                if (nextState[i] > target[i]) return true;
            }

            return false;
        }

        // start at all lights off: .....
        //count step 1
        // apply every button press from current
            //check if equal to target
                //if yes -> return steps taken
                //if not add to visited
        int SolvePart1(string target, List<int[]> buttons) {
            var visited = new HashSet<string> { target };
            var states = new List<string>() { new string('.', target.Length) };
            var steps = 0;

            while(true) {
                steps++;
                var nextStates = new List<string>();

                foreach (var state in states) {
                    foreach (var button in buttons) {
                        var nextState = PressButton(state, button);
                        if (nextState == target) {
                            return steps;
                        }
                        
                        if (!visited.Contains(nextState)) {
                            nextStates.Add(nextState);
                        }
                        
                        visited.Add(nextState);
                    }
                }

                states = nextStates;
            }
        }

        int[] PressJoltageButton(int[] currentState, int[] button) {
            var newState = (int[])currentState.Clone();

            foreach (var num in button)
            {
                newState[num]++;
            }

            return newState;
        }

        string PressButton(string currentState, int[] button) {
            var chars = currentState.ToCharArray();

            foreach (var num in button) {
                var oldChar = chars[num];

                if (oldChar == '.') {
                    chars[num] = '#';
                } else {
                    chars[num] = '.';
                }
            }

            return new string(chars);
        }
    }
}


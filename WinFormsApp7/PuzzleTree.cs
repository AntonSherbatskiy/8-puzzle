namespace WinFormsApp7
{
    internal class PuzzleTree
    {
        public PuzzleTree(PuzzleNode puzzleNode, long iterationCount, long stateCount, long blindCornersCount, int averageStateMemoryCount)
        {
            if (puzzleNode != null)
            {
                Path = puzzleNode.GetPath();

                InitialState = Path[0];
                GoalState = Path[^1];

                IterationCount = iterationCount;
                StateCount = stateCount;
                PathNodesCount = Path.Count;
                BlindCornersCount = blindCornersCount;
                StatesInMemoryCount = averageStateMemoryCount;
            }
        }

        public PuzzleNode InitialState { get; private set; }
        public PuzzleNode GoalState { get; private set; }
        public List<PuzzleNode> Path { get; private set; }

        public long IterationCount { get; private set; }
        public long StateCount { get; private set; }
        public int PathNodesCount { get; private set; }
        public long BlindCornersCount { get; private set; }
        public long StatesInMemoryCount { get; private set; }

        public override string ToString()
        {
            string line = string.Empty;

            for (int i = 0; i < Path.Count; i++)
            {
                line += $"Action-> {Path[i].Action}\n{Path[i]}";

                if (i < Path.Count - 1)
                {
                    line += "\n";
                }
            }

            line += $"\nIterations count-> {IterationCount}\n" +
                    $"Created state count-> {StateCount}\n" +
                    $"Average state count in RAM-> {StatesInMemoryCount}\n" +
                    $"Path nodes count-> {PathNodesCount}\n" +
                    $"Blind corners count-> {BlindCornersCount}\n" +
                    $"Elapsed seconds-> {PerformanceMeasurer.ElapsedSeconds}\n" +
                    $"Elapsed milliseconds-> {PerformanceMeasurer.ElapsedMilliSeconds}";

            return line;
        }
    }
}

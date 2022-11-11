namespace WinFormsApp7
{
    internal class PuzzleNode
    {
        #region Ctor
        public PuzzleNode(int[] puzzleArray, string action = null!, PuzzleNode parentNode = null!)
        {
            InstancesCount++;

            PuzzleArray = new int[9];

            for (int i = 0; i < PuzzleArray.Length; i++)
            {
                PuzzleArray[i] = puzzleArray[i];
            }

            if (parentNode == null)
            {
                ParentNode = null!;
                Action = "INITIAL_NODE";
                Depth = 0;
                InstancesCount = 1;
            }
            else
            {
                ParentNode = parentNode;
                Action = action;
                Depth = parentNode.Depth + 1;
            }

            Heuristic = GetHeuristic();
            EvaluationFunction = Heuristic + Depth;
        }

        #endregion

        private static readonly int[] GoalStateArray = new int[]
        {
            0, 1, 2,
            3, 4, 5,
            6, 7, 8
        };
        public static long InstancesCount { get; private set; } = 0;

        public int[] PuzzleArray { get; set; }

        public PuzzleNode ParentNode { get; private set; }
        private int TwoDimensionalSize { get; set; } = 3;
        public string Action { get; private set; }
        public int Depth { get; private set; }
        public int Heuristic { get; private set; }
        public int EvaluationFunction { get; set; }

        #region Methods
        public List<PuzzleNode> GetPath()
        {
            List<PuzzleNode> path = new();
            PuzzleNode current = this;

            path.Add(current);
            while (current.ParentNode != null)
            {
                current = current.ParentNode;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        public static int[] GeneratePuzzleRandomArray()
        {
            Random random = new Random();
            int[] arr = new int[9];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }

            do
            {
                random.Shuffle(arr);
            } while (!IsSolvable(arr));

            return arr;
        }

        private static bool IsSolvable(int[] arr)
        {
            int invCount = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (arr[i] > 0 && arr[j] > 0 && arr[i] > arr[j])
                    {
                        invCount++;
                    }
                }
            }

            return invCount % 2 == 0;
        }

        public int GetHeuristic()
        {
            int count = 0;

            for (int i = 0; i < PuzzleArray.Length; i++)
            {
                if (PuzzleArray[i] != GoalStateArray[i])
                {
                    count++;
                }
            }

            return count;
        }

        public void SetPuzzleNode(int[] puzzleArray)
        {
            Array.Copy(puzzleArray, this.PuzzleArray, this.PuzzleArray.Length);
        }

        public List<PuzzleNode> ExpandNode()
        {
            List<PuzzleNode> childrenNodes = new List<PuzzleNode>();
            int zeroCoord = GetZeroCoordinate();

            MoveRight(zeroCoord, childrenNodes);
            MoveLeft(zeroCoord, childrenNodes);
            MoveUp(zeroCoord, childrenNodes);
            MoveDown(zeroCoord, childrenNodes);

            return childrenNodes;
        }

        public int GetZeroCoordinate()
        {
            return Array.IndexOf(PuzzleArray, 0);
        }

        public int[] ToArray()
        {
            return PuzzleArray;
        }

        public void MoveRight(int zeroCoordinate, List<PuzzleNode> childrenNodes)
        {
            if (zeroCoordinate % TwoDimensionalSize < TwoDimensionalSize - 1)
            {
                int[] newArray = DuplicatePuzzle(PuzzleArray);

                (newArray[zeroCoordinate], newArray[zeroCoordinate + 1]) =
                    (newArray[zeroCoordinate + 1], newArray[zeroCoordinate]);

                PuzzleNode childNode = new PuzzleNode(newArray, "Right", this);
                childrenNodes.Add(childNode);
            }
        }

        public void MoveLeft(int zeroCoordinate, List<PuzzleNode> childrenNodes)
        {
            if (zeroCoordinate % TwoDimensionalSize > 0)
            {
                int[] newArray = DuplicatePuzzle(PuzzleArray);

                (newArray[zeroCoordinate], newArray[zeroCoordinate - 1]) =
                        (newArray[zeroCoordinate - 1], newArray[zeroCoordinate]);

                PuzzleNode childNode = new PuzzleNode(newArray, "Left", this);
                childrenNodes.Add(childNode);
            }
        }

        public void MoveUp(int zeroCoordinate, List<PuzzleNode> childrenNodes)
        {
            if (zeroCoordinate - TwoDimensionalSize >= 0)
            {
                int[] newArray = DuplicatePuzzle(PuzzleArray);

                (newArray[zeroCoordinate], newArray[zeroCoordinate - 3]) =
                    (newArray[zeroCoordinate - 3], newArray[zeroCoordinate]);

                PuzzleNode childNode = new PuzzleNode(newArray, "Up", this);
                childrenNodes.Add(childNode);
            }
        }

        public void MoveDown(int zeroCoordinate, List<PuzzleNode> childrenNodes)
        {
            if (zeroCoordinate + TwoDimensionalSize < PuzzleArray.Length)
            {
                int[] newArray = DuplicatePuzzle(PuzzleArray);

                (newArray[zeroCoordinate], newArray[zeroCoordinate + 3]) =
                    (newArray[zeroCoordinate + 3], newArray[zeroCoordinate]);

                PuzzleNode childNode = new PuzzleNode(newArray, "Down", this);
                childrenNodes.Add(childNode);
            }
        }

        public int[] DuplicatePuzzle(int[] puzzleArray)
        {
            return (int[])puzzleArray.Clone();
        }

        public bool IsSamePuzzle(int[] puzzleArray)
        {
            return Enumerable.SequenceEqual(PuzzleArray, puzzleArray);
        }

        public bool IsGoalState()
        {
            return Enumerable.SequenceEqual(PuzzleArray, GoalStateArray);
        }

        public override string ToString()
        {
            string line = string.Empty;
            int temp = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    line += PuzzleArray[temp] + "  ";
                    temp++;
                }

                line += "\n";
            }

            return line;
        }

        internal void ToButtons(Button[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Text = PuzzleArray[i].ToString();
            }
        }
        #endregion
    }
}
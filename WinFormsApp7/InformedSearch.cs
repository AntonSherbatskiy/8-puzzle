using System.ComponentModel;

namespace WinFormsApp7
{
    internal static class InformedSearch
    {
        public static long RBFSIterationsCount { get; private set; } = 0;
        private static BackgroundWorker? Worker { get; set; }

        public static PuzzleTree RecursiveBestFirstSearch(PuzzleNode initialState, BackgroundWorker worker, DoWorkEventArgs e)
        {
            Worker = worker;
            RBFSIterationsCount = 0;

            PerformanceMeasurer.StartMeasure();
            (PuzzleNode goalStateNode, int _) = RBFS(initialState, int.MaxValue, e);
            PerformanceMeasurer.StopMeasure();

            if (goalStateNode == null)
            {
                return null;
            }

            return new PuzzleTree(goalStateNode, RBFSIterationsCount, PuzzleNode.InstancesCount, 0, goalStateNode.Depth * 3);
        }

        private static (PuzzleNode, int) RBFS(PuzzleNode node, int fLimit, DoWorkEventArgs e)
        {
            RBFSIterationsCount++;

            if (node.IsGoalState())
            {
                return (node, 0);
            }

            List<PuzzleNode> successors = node.ExpandNode();

            while (true)
            {
                if (Worker!.CancellationPending)
                {
                    e.Cancel = true;
                    return (null!, 0);
                }

                successors = successors.OrderBy(p => p.EvaluationFunction).ToList();

                PuzzleNode best = successors[0];

                if (best.EvaluationFunction > fLimit)
                {
                    return (null!, best.EvaluationFunction);
                }

                int alternative = successors[1].EvaluationFunction;

                (PuzzleNode result, best.EvaluationFunction) = RBFS(best, Math.Min(fLimit, alternative), e);

                successors[0] = best;

                if (result != null)
                {
                    return (result, 0);
                }
            }
        }
    }
}

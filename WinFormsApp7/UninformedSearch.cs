using System.ComponentModel;

namespace WinFormsApp7
{
    internal static class UninformedSearch
    {
        private static long DLSIterations { get; set; } = 0;
        private static long BlindCornersCount { get; set; } = 0;
        private static BackgroundWorker? Worker;

        public static PuzzleTree DLS(PuzzleNode initialState, int limit, BackgroundWorker backgroundWorker, DoWorkEventArgs e)
        {
            Worker = backgroundWorker;
            DLSIterations = 0;
            BlindCornersCount = 0;

            PerformanceMeasurer.StartMeasure();
            (PuzzleNode? node, _, _) = RecursiveDLS(initialState, limit, e);
            PerformanceMeasurer.StopMeasure();

            return new PuzzleTree(node, DLSIterations, PuzzleNode.InstancesCount, BlindCornersCount, limit * 3);
        }

        private static (PuzzleNode?, bool, bool) RecursiveDLS(PuzzleNode puzzleNode, int limit, DoWorkEventArgs e)
        {
            if (Worker!.CancellationPending)
            {
                e.Cancel = true;
                return (null, false, false);
            }

            DLSIterations++;

            bool cutoff_occured = false;
            if (puzzleNode.IsGoalState())
            {
                return (puzzleNode, false, false);
            }
            else if (puzzleNode.Depth == limit)
            {
                return (null, false, true);
            }
            else
            {
                var successors = puzzleNode.ExpandNode();

                foreach (var item in successors)
                {
                    (PuzzleNode? result, bool failure, bool cutoff) = RecursiveDLS(item, limit, e);

                    if (result == null && failure == false && cutoff == true)
                    {
                        cutoff_occured = true;
                    }
                    else if (result != null)
                    {
                        return (result, false, false);
                    }
                }
            }
            if (cutoff_occured)
            {
                BlindCornersCount++;
                return (null, false, true);
            }
            else
            {
                return (null, true, false);
            }
        }
    }
}

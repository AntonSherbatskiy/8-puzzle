using System.Diagnostics;

namespace WinFormsApp7
{
    internal static class PerformanceMeasurer
    {
        public static double ElapsedSeconds { get; private set; } = 0;
        public static long ElapsedMilliSeconds { get; private set; } = 0;
        private static Stopwatch? Timer { get; set; }

        public static void StartMeasure()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }

        public static void StopMeasure()
        {
            Timer!.Stop();

            ElapsedMilliSeconds = Timer.ElapsedMilliseconds;
            ElapsedSeconds = Math.Round(Timer.ElapsedMilliseconds / 1000.0, 1);
        }
    }
}

namespace WinFormsApp7
{
    internal static class RandomExtensions
    {
        public static void Shuffle(this Random rng, int[] array)
        {
            int n = array.Length;

            while (n > 1)
            {
                int k = rng.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
    }
}
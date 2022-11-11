namespace WinFormsApp7
{
    internal static class ButtonExtension
    {
        public static void ClearButtons(this Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Text = string.Empty;
            }
        }

        public static void ChangeColor(this Button[] buttons, Color color)
        {
            foreach (var button in buttons)
            {
                button.BackColor = color;
            }
        }
    }


}

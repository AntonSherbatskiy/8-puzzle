using System.Timers;

namespace WinFormsApp7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private System.Timers.Timer SystemTimer;
        private int ElapsedSystemSeconds { get; set; }
        private int SecondsTimeLimit { get; set; } = 1800;

        private PuzzleNode? EightPuzzle { get; set; }
        private PuzzleTree PuzzleTree { get; set; }

        private Button[] InitialStateButtons { get; set; }
        private Button[] GoalStateButtons { get; set; }

        private readonly Color InitColor = Color.LightBlue;
        private readonly Color GoalColor = Color.LightGreen;
        private readonly Color EmptyTileColor = Color.Silver;

        private int LimititedDeepFirstDepth => (int)depthUpDown.Value;

        private void Form1_Load(object sender, EventArgs e)
        {
            SystemTimer = new System.Timers.Timer();
            SystemTimer.Interval = 1000;
            SystemTimer.Elapsed += OnTimeEvent;

            InitialStateButtons = new Button[]
            {
                button0, button1, button2,
                button3, button4, button5,
                button6, button7, button8
            };
            GoalStateButtons = new Button[]
            {
                button17, button16, button15,
                button14, button13, button12,
                button11, button10, button9
            };
        }

        private void OnTimeEvent(object? sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (++ElapsedSystemSeconds > SecondsTimeLimit)
                {
                    backgroundWorker1.CancelAsync();

                    MessageBox.Show($"The allocated time is up!", $"Time limit {SecondsTimeLimit / 60} minutes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }));
        }

        private void randomGenerateButton_Click(object sender, EventArgs e)
        {
            EightPuzzle = new PuzzleNode(PuzzleNode.GeneratePuzzleRandomArray());
            EightPuzzle.ToButtons(InitialStateButtons);

            if (button9.Text != string.Empty)
            {
                GoalStateButtons.ClearButtons();
            }

            richTextBox1.Text = string.Empty;
            ButtonControl(true, ldfsButton, rbfsButton);
            InitialStateButtons.ChangeColor(InitColor);
            GoalStateButtons.ChangeColor(EmptyTileColor);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                EightPuzzle = null;
                richTextBox1.Text = string.Empty;

                InitialStateButtons.ClearButtons();
                GoalStateButtons.ClearButtons();

                ButtonControl(false, ldfsButton, rbfsButton);

                InitialStateButtons.ChangeColor(Color.Silver);
                GoalStateButtons.ChangeColor(Color.Silver);
            }
        }

        private void ButtonControl(bool isEnabled, params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Enabled = isEnabled;
            }
        }

        private void ldfsButton_Click(object sender, EventArgs e)
        {
            SystemTimer.Start();

            richTextBox1.Text = "The LDFS algorithm is running now, please wait.";

            depthUpDown.Enabled = false;
            ButtonControl(false, ldfsButton, rbfsButton, randomGenerateButton);
            backgroundWorker1.RunWorkerAsync("ldfs");
        }

        private void rbfsButton_Click(object sender, EventArgs e)
        {
            SystemTimer.Start();

            richTextBox1.Text = "The RBFS algorithm is running now, please wait.";

            depthUpDown.Enabled = false;
            ButtonControl(false, ldfsButton, rbfsButton, randomGenerateButton);
            backgroundWorker1.RunWorkerAsync("rbfs");
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if ((string)e.Argument! == "ldfs")
            {
                PuzzleTree = UninformedSearch.DLS(EightPuzzle!, LimititedDeepFirstDepth, backgroundWorker1, e);
            }
            else
            {
                PuzzleTree = InformedSearch.RecursiveBestFirstSearch(EightPuzzle!, backgroundWorker1, e);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            SystemTimer.Stop();
            ElapsedSystemSeconds = 0;

            if (e.Cancelled)
            {
                EightPuzzle = null;
                richTextBox1.Text = string.Empty;

                InitialStateButtons.ClearButtons();
                GoalStateButtons.ClearButtons();

                ButtonControl(false, ldfsButton, rbfsButton);

                InitialStateButtons.ChangeColor(Color.Silver);
                GoalStateButtons.ChangeColor(Color.Silver);
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();

                if (PuzzleTree.PathNodesCount > 1)
                {
                    richTextBox1.Text = PuzzleTree.ToString();
                    PuzzleTree.GoalState.ToButtons(GoalStateButtons);

                    GoalStateButtons.ChangeColor(GoalColor);
                }
                else
                {
                    richTextBox1.Text = $"No solution find with depth: {LimititedDeepFirstDepth}";
                }
            }

            ButtonControl(true, randomGenerateButton);
            depthUpDown.Enabled = true;
        }

        private void depthUpDown_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
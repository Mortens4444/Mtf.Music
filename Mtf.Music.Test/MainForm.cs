using Mtf.Music.Melodies;

namespace Mtf.Music.Test
{
    public partial class MainForm : Form
    {
        private readonly Melody melody = new BociBoci();
        private readonly Player player = new();

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            player.PlayMusic(melody);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            player.Stop();
        }
    }
}

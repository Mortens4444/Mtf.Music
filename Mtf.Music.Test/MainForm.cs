using Mtf.Music.Melodies;

namespace Mtf.Music.Test
{
    public partial class MainForm : Form
    {
        private Melody melody = new BociBoci();
        private Player player = new();

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

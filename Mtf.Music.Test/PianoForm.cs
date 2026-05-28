using Mtf.Music.Melodies;
using Mtf.Music.Notes;

namespace Mtf.Music.Test;

public partial class PianoForm : Form
{
    private readonly Player player = new();

    public PianoForm()
    {
        InitializeComponent();
        player.CurrentlyPlayedMelody = new Empty(2, 4, 100);
        BuildKeyboard();
    }

    private void BuildKeyboard()
    {
        int startX = 40;
        int yWhite = 60;

        int whiteW = 100;
        int whiteH = 320;

        int blackW = 65;
        int blackH = 200;

        var notes = new (string name, Func<Note> ctor, bool isBlack)[]
        {
            ("C4", () => new C4(), false),
            ("C#4", () => new Cs4_Db4(), true),
            ("D4", () => new D4(), false),
            ("D#4", () => new Ds4_Eb4(), true),
            ("E4", () => new E4(), false),
            ("F4", () => new F4(), false),
            ("F#4", () => new Fs4_Gb4(), true),
            ("G4", () => new G4(), false),
            ("G#4", () => new Gs4_Ab4(), true),
            ("A4", () => new A4(), false),
            ("A#4", () => new As4_Bb4(), true),
            ("B4", () => new B4(), false),

            ("C5", () => new C5(), false),
            ("C#5", () => new Cs5_Db5(), true),
            ("D5", () => new D5(), false),
            ("D#5", () => new Ds5_Eb5(), true),
            ("E5", () => new E5(), false),
            ("F5", () => new F5(), false),
            ("F#5", () => new Fs5_Gb5(), true),
            ("G5", () => new G5(), false),
            ("G#5", () => new Gs5_Ab5(), true),
            ("A5", () => new A5(), false),
            ("A#5", () => new As5_Bb5(), true),
            ("B5", () => new B5(), false),
        };

        int whiteIndex = 0;
        var whitePositions = new Dictionary<string, int>();

        // First pass: white keys
        foreach (var n in notes.Where(n => !n.isBlack))
        {
            int x = startX + whiteIndex * whiteW;

            AddKey(n.name, n.ctor(), x, yWhite, whiteW, whiteH, Color.White, Color.Black);

            whitePositions[n.name] = x;
            whiteIndex++;
        }

        // Second pass: black keys (offset between whites)
        foreach (var n in notes.Where(n => n.isBlack))
        {
            var baseName = n.name.Replace("#", "").Replace("b", "");

            // crude positioning: map manually via white index pattern
            int x = GetBlackKeyX(n.name, startX, whiteW);

            AddKey(n.name, n.ctor(), x, yWhite, blackW, blackH, Color.Black, Color.White, true);
        }

        Width = startX + whiteIndex * whiteW + 100;
        Height = 520;
        Text = "Mtf.Music Piano (2 Octaves)";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }

    private static int GetBlackKeyX(string note, int startX, int whiteW)
    {
        return note switch
        {
            "C#4" => startX + (int)(0.70 * whiteW),
            "D#4" => startX + (int)(1.70 * whiteW),
            "F#4" => startX + (int)(3.70 * whiteW),
            "G#4" => startX + (int)(4.70 * whiteW),
            "A#4" => startX + (int)(5.70 * whiteW),

            "C#5" => startX + (int)(7.70 * whiteW),
            "D#5" => startX + (int)(8.70 * whiteW),
            "F#5" => startX + (int)(10.70 * whiteW),
            "G#5" => startX + (int)(11.70 * whiteW),
            "A#5" => startX + (int)(12.70 * whiteW),

            _ => startX
        };
    }

    private void AddKey(string text, Note note, int x, int y, int w, int h, Color back, Color fore, bool top = false)
    {
        var btn = new Button
        {
            Text = text,
            Width = w,
            Height = h,
            Left = x,
            Top = y,
            BackColor = back,
            ForeColor = fore,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        btn.Click += (_, _) => Play(note);

        Controls.Add(btn);

        if (top)
            btn.BringToFront();
    }

    private void Play(Note note)
    {
        Task.Run(() => player.PlayNote(note));
    }
}
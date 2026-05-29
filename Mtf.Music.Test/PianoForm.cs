using Mtf.Music.Melodies;
using System.Reflection;

namespace Mtf.Music.Test;

public partial class PianoForm : Form
{
    private readonly Player player = new();

    private const int NumberOfOctaves = 3;
    private const int StartOctave = 3;

    private const int WhiteKeyWidth = 100;
    private const int WhiteKeyHeight = 320;

    private const int BlackKeyWidth = 65;
    private const int BlackKeyHeight = 200;

    private static readonly string[] WhiteNotes = ["C", "D", "E", "F", "G", "A", "B"];
    private static readonly (string Name, double Offset)[] BlackNotes =
    [
        ("Cs", 0.7),
        ("Ds", 1.7),
        ("Fs", 3.7),
        ("Gs", 4.7),
        ("As", 5.7)
    ];

    public PianoForm()
    {
        InitializeComponent();

        player.CurrentlyPlayedMelody = new Empty(2, 4, 100);

        BuildKeyboard();
    }

    private void BuildKeyboard()
    {
        const int startX = 40;
        const int yWhite = 60;

        int whiteIndex = 0;

        for (int octave = StartOctave; octave < StartOctave + NumberOfOctaves; octave++)
        {
            // White keys
            foreach (var whiteNote in WhiteNotes)
            {
                var note = CreateNote($"{whiteNote}{octave}");

                if (note == null)
                {
                    continue;
                }

                int x = startX + whiteIndex * WhiteKeyWidth;

                AddKey(
                    note.Name,
                    note,
                    x,
                    yWhite,
                    WhiteKeyWidth,
                    WhiteKeyHeight,
                    Color.White,
                    Color.Black);

                whiteIndex++;
            }

            // Black keys
            foreach (var blackNote in BlackNotes)
            {
                //var note = CreateNote($"{blackNote.Name}{octave}");
                var note = CreateNote($"{blackNote.Name}{octave}_{GetFlatName(blackNote.Name)}{octave}");
                if (note == null)
                {
                    continue;
                }

                int x = startX + (int)((whiteIndex - 7 + blackNote.Offset) * WhiteKeyWidth);

                AddKey(
                    note.Name,
                    note,
                    x,
                    yWhite,
                    BlackKeyWidth,
                    BlackKeyHeight,
                    Color.Black,
                    Color.White,
                    true);
            }
        }

        Width = startX + whiteIndex * WhiteKeyWidth + 100;
        Height = 520;

        Text = $"Mtf.Music Piano ({NumberOfOctaves} Octaves)";

        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }

    private static string GetFlatName(string sharpName)
    {
        return sharpName switch
        {
            "Cs" => "Db",
            "Ds" => "Eb",
            "Fs" => "Gb",
            "Gs" => "Ab",
            "As" => "Bb",
            _ => throw new ArgumentOutOfRangeException(nameof(sharpName))
        };
    }

    private static Note? CreateNote(string typeName)
    {
        string fullName = $"Mtf.Music.Notes.{typeName}";

        var type = Assembly
            .GetAssembly(typeof(Note))?
            .GetType(fullName);

        if (type == null)
        {
            return null;
        }

        return Activator.CreateInstance(type, NoteType.Quarter) as Note;
    }

    private void AddKey(
        string text,
        Note note,
        int x,
        int y,
        int w,
        int h,
        Color back,
        Color fore,
        bool top = false)
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
        {
            btn.BringToFront();
        }
    }

    private void Play(Note note)
    {
        Task.Run(() => player.PlayNote(note));
    }
}
using System.Runtime.Versioning;

namespace Mtf.Music;

public class Player
{
    private CancellationTokenSource musicPlayerCancellationTokenSource = new();
    private readonly Lock soundPlayLock = new();

    public Melody? CurrentlyPlayedMelody { get; set; }
    
    public static void Beep(ushort frequency, ushort duration)
    {
        if (OperatingSystem.IsWindows() && frequency is >= 37 and <= 32767)
        {
            WindowsBeep(frequency, duration);
        }
        else
        {
            Thread.Sleep(duration);
        }
    }

    public void PlayNote(Note note)
    {
        ArgumentNullException.ThrowIfNull(CurrentlyPlayedMelody, nameof(CurrentlyPlayedMelody));
        var duration = CurrentlyPlayedMelody.GetNoteLength(note.NoteType);
        if (note is Fermata)
        {
            Thread.Sleep(duration);
        }
        else
        {
            var frequency = (ushort)Math.Round(note.Frequency);
            Beep(frequency, duration);
        }
    }

    public void PlayMusic(Melody melody)
    {
        lock (soundPlayLock)
        {
            if (CurrentlyPlayedMelody != null)
            {
                return;
            }

            CurrentlyPlayedMelody = melody;
        }

        musicPlayerCancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = musicPlayerCancellationTokenSource.Token;

        Task.Factory.StartNew(() =>
        {
            lock (soundPlayLock)
            {
                foreach (var note in melody)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    PlayNote(note);
                }

                CurrentlyPlayedMelody = null;
            }
        }, cancellationToken);
    }

    public void Stop()
    {
        musicPlayerCancellationTokenSource.Cancel();
    }

    [SupportedOSPlatform("windows")]
    private static void WindowsBeep(ushort frequency, ushort duration)
    {
        Console.Beep(frequency, duration);
    }
}

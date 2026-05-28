namespace Mtf.Music;

public class Player
{
    private CancellationTokenSource musicPlayerCancellationTokenSource = new();
    private readonly Lock soundPlayLock = new();

    public Melody? CurrentlyPlayedMelody { get; set; }
    
    public static void Beep(ushort frequency, ushort duration)
    {
        if (frequency is >= 37 and <= 32767)
        {
            Console.Beep(frequency, duration);
        }
        else
        {
            Thread.Sleep(duration);
        }
    }
    
    public void PlayNote(Note note)
    {
        var duration = CurrentlyPlayedMelody?.GetNoteLength(note.NoteType) ?? 0;
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
        if (CurrentlyPlayedMelody != null)
        {
            return;
        }

        musicPlayerCancellationTokenSource = new CancellationTokenSource();

        Task.Factory.StartNew(() =>
        {
            lock (soundPlayLock)
            {
                CurrentlyPlayedMelody = melody;

                foreach (var note in melody)
                {
                    if (musicPlayerCancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }

                    PlayNote(note);
                }

                CurrentlyPlayedMelody = null;
            }
        }, musicPlayerCancellationTokenSource.Token);
    }

    public void Stop()
    {
        musicPlayerCancellationTokenSource.Cancel();
    }
}

namespace Mtf.Music;

public class Fermata(NoteType noteType = NoteType.Quarter) : Note(noteType)
{
    public override double Frequency => 0;

    public override double WaveLength => 0;
}

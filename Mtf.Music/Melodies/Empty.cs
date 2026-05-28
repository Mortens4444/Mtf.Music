namespace Mtf.Music.Melodies;

public class Empty(byte numberOfQuarterNotes, byte bar, byte beatsPerMinute)
    : Melody(new TimeSignature(numberOfQuarterNotes, bar), beatsPerMinute)
{
    public override string ToString()
    {
        return "Empty";
    }
}

namespace Mtf.Music;

public class TimeSignature(byte numberOfQuarterNotes, byte bar)
{
    public byte NumberOfQuarterNotes { get; set; } = numberOfQuarterNotes;

    public byte Bar { get; set; } = bar;
}
